using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SentIO.Globals;
using SentIO.Routines;
using SentIO.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace SentIO.Console
{
    public class TextControl
    {
        private static TextControl instance;
        public static TextControl Instance
        {
            get
            {
                if (instance == null)
                    instance = new TextControl();
                return instance;
            }
        }

        public class Wait : ICoroutineYield
        {
            public Wait() { }        
            public void Execute() { }
            public bool IsDone() => Instance.IsDone;
        }

        public Color Background { get; set; } = Color.Black;
        public Color Foreground { get; set; } = Color.White;

        public bool IsDone => mode == Mode.Nothing;
        public Vector2 Position { get; set; } = Vector2.Zero;

        public enum Mode
        {
            Nothing,
            Output,
            Input,
            WaitForKeyPress,
            WaitForTime
        }
        private Mode mode;

        public enum Layout
        {
            TopLeft,
            Centered
        }
        private Layout layout;
        public string InputResult { get; private set; }


        private string[] lines = new string[2];
        private Vector2[] linePos = new Vector2[2];
        private string inputLine;
        private int cursorY;
        private string textOutput;
        private string textInput;
        private int index;
        private int nextCharDelay;
        private int blinkDelay;
        private int frameCountdown;

        private bool showBlink;
        private static char CursorChar = '_';
        private static int CursorBlinkDelay = 20;
        public enum Speed
        {
            Slow,
            Normal,
            Fast,
            UltraFast
        }
        public Speed CurrentSpeed { get; set; } = Speed.Normal;

        private TextControl()
        {
            mode = Mode.Nothing;
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = "";
            }
            textOutput = "";
            textInput = "";
            index = 0;
            nextCharDelay = 0;
            blinkDelay = 0;
            cursorY = -1;
            showBlink = false;
            frameCountdown = 0;
            SetLayout(Layout.TopLeft);
        }

        public void SetLayout(Layout _layout)
        {
            layout = _layout;
        }

        public ICoroutineYield Show(string text)
        {
            mode = Mode.Output;
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = "";
            }
            inputLine = "";
            textOutput = text;
            index = 0;
            nextCharDelay = 0;
            blinkDelay = 0;
            cursorY = -1;
            showBlink = false;
            return new Wait();
        }

        public ICoroutineYield Input()
        {
            mode = Mode.Input;
            textInput = "";
            InputResult = "";
            cursorY = lines.Length;
            showBlink = false;
            return new Wait();
        }

        public ICoroutineYield WaitForKeyPress()
        {
            mode = Mode.WaitForKeyPress;
            return new Wait();
        }

        public ICoroutineYield WaitForCountdown(int frames)
        {
            mode = Mode.WaitForTime;
            frameCountdown = frames;
            return new Wait();
        }

        private void SetOutput(string textOutput, int index=-1)
        {
            string[] fullLines = textOutput.Split('\n');
            
            lines = new string[fullLines.Length];
            linePos = new Vector2[fullLines.Length + 1];
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = "";
            }

            if (index < 0) index = textOutput.Length;
            string textOutputPart = textOutput.Substring(0, index);
            string[] partLines = textOutputPart.Split('\n');
            for (int i = 0; i < partLines.Length; i++)
            {
                lines[i] = partLines[i];
            }

            if (layout == Layout.TopLeft)
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    linePos[i].X = 0;
                    linePos[i].Y = Resources.ConsoleFont.LineSpacing * i;
                }
            }
            else if (layout == Layout.Centered)
            {
                int windowWidth = MainGame.Instance.Window.ClientBounds.Width;
                int windowHeight = MainGame.Instance.Window.ClientBounds.Height;
                for (int i = 0; i < fullLines.Length; i++)
                {
                    linePos[i].X = (int) ((windowWidth - Resources.ConsoleFont.MeasureString(fullLines[i]).X) / 2f);
                    linePos[i].Y = windowHeight / 2 - Resources.ConsoleFont.LineSpacing * (fullLines.Length - i + 2);
                }
                linePos[lines.Length].X = 252;
                linePos[lines.Length].Y = MainGame.Instance.Window.ClientBounds.Height - Resources.ConsoleFont.LineSpacing * 5;
            }
        }

        public void Update()
        {
            nextCharDelay = Math.Max(nextCharDelay - 1, 0);
            
            if (cursorY >= 0 && cursorY <= lines.Length)
            {
                blinkDelay = Math.Max(blinkDelay - 1, 0);
                if (blinkDelay == 0)
                {
                    showBlink = !showBlink;
                    blinkDelay = CursorBlinkDelay;
                }
            }
            
            if (mode == Mode.Output)
            {
                if (nextCharDelay == 0)
                {
                    if (index < textOutput.Length)
                    {
                        index++;
                    }
                    switch (CurrentSpeed)
                    {
                        case Speed.Normal:
                            nextCharDelay = 4;
                            break;
                        case Speed.Slow:
                            nextCharDelay = 8;
                            break;
                        case Speed.Fast:
                            nextCharDelay = 2;
                            break;
                        case Speed.UltraFast:
                            nextCharDelay = 1;
                            break;
                    }
                }
                SetOutput(textOutput, index);
                if (index >= textOutput.Length)
                {
                    mode = Mode.Nothing;
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].Length > 0) cursorY = i;
                    }
                }
            }
            else if (mode == Mode.Input)
            {
                foreach (Keys key in Globals.Input.KeysPressedThisFrame())
                {
                    if (key >= Keys.A && key <= Keys.Z) {
                        if (Globals.Input.IsKeyCurrentlyPressed(Keys.LeftShift) || Globals.Input.IsKeyCurrentlyPressed(Keys.RightShift))
                        {
                            textInput += key.ToString();
                        }
                        else
                        {
                            textInput += key.ToString().ToLower();
                        }
                    }
                    else if (key == Keys.Space)
                    {
                        textInput += " ";
                    }
                    else if (key == Keys.Back && textInput.Length > 0)
                    {
                        textInput = textInput.Remove(textInput.Length - 1);
                    }
                    else if (key == Keys.Enter)
                    {
                        mode = Mode.Nothing;
                        InputResult = textInput;
                        textInput = "";
                    }
                }
                inputLine = textInput;
            }
            else if (mode == Mode.WaitForKeyPress)
            {
                if (Globals.Input.WasAnyKeyPressedThisFrame())
                {
                    mode = Mode.Nothing;
                }
                SetOutput(textOutput);
                inputLine = textInput;
            }
            else if (mode == Mode.WaitForTime)
            {
                frameCountdown--;
                if (frameCountdown <= 0)
                {
                    mode = Mode.Nothing;
                }
                SetOutput(textOutput);
                inputLine = textInput;
            }
            else if (mode == Mode.Nothing)
            {
                SetOutput(textOutput);
                inputLine = textInput;
            }

            if (showBlink)
            {
                if (cursorY >= 0 && cursorY < lines.Length)
                {
                    lines[cursorY] += CursorChar;
                }
                else if (cursorY == lines.Length)
                {
                    inputLine += CursorChar;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                spriteBatch.DrawString(Resources.ConsoleFont, lines[i], linePos[i],
                    Foreground, 0, Vector2.Zero, new Vector2(1), SpriteEffects.None, 1);
            }
            spriteBatch.DrawString(Resources.ConsoleFont, inputLine, linePos[lines.Length],
                Foreground, 0, Vector2.Zero, new Vector2(1), SpriteEffects.None, 1);
        }
    }
}
