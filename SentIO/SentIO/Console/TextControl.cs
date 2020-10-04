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
        private int cursorLine;
        private string textOutput;
        private string textInput;
        private int index;
        private int nextCharDelay;
        private int blinkDelay;
        private int frameCountdown;

        private bool showBlink;
        private static char CursorChar = '_';
        private static int CursorBlinkDelay = 20;

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
            cursorLine = -1;
            showBlink = false;
            frameCountdown = 0;
            SetLayout(Layout.TopLeft);
        }

        public void SetLayout(Layout _layout)
        {
            layout = _layout;
            switch (layout)
            {
                case Layout.TopLeft:
                    lines = new string[2];
                    linePos = new Vector2[2];
                    break;
                case Layout.Centered:
                    lines = new string[3];
                    linePos = new Vector2[3];
                    break;
            }
        }

        public ICoroutineYield Show(string text)
        {
            mode = Mode.Output;
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = "";
            }
            textOutput = text;
            index = 0;
            nextCharDelay = 0;
            blinkDelay = 0;
            cursorLine = -1;
            showBlink = false;
            return new Wait();
        }

        public ICoroutineYield Input()
        {
            mode = Mode.Input;
            textInput = "";
            InputResult = "";
            cursorLine = lines.Length - 1;
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
            if (index < 0) index = textOutput.Length;
            string textOutputPart = textOutput.Substring(0, index);
            if (lines.Length > 2)
            {
                string[] partLines = textOutputPart.Split('\n');
                for (int i = 0; i < lines.Length - 2; i++)
                {
                    lines[i] = partLines[i];
                }
                lines[lines.Length - 2] = string.Join("", partLines.Skip(lines.Length - 2).ToArray());
            }
            else
            {
                lines[0] = textOutputPart;
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
                string[] fullLines = new string[lines.Length];
                if (fullLines.Length > 2)
                {
                    string[] outLines = textOutput.Split('\n');
                    for (int i = 0; i < lines.Length - 2; i++)
                    {
                        fullLines[i] = outLines[i];
                    }
                    fullLines[fullLines.Length - 2] = string.Join("", outLines.Skip(fullLines.Length - 2).ToArray());
                }
                else
                {
                    fullLines[0] = textOutput;
                }
                for (int i = 0; i < lines.Length - 1; i++)
                {
                    linePos[i].X = (int) ((MainGame.Instance.Window.ClientBounds.Width - Resources.ConsoleFont.MeasureString(fullLines[i]).X) / 2f);
                    linePos[i].Y = 36 + Resources.ConsoleFont.LineSpacing * i;
                }
                linePos[lines.Length - 1].X = 252;
                linePos[lines.Length - 1].Y = MainGame.Instance.Window.ClientBounds.Height - Resources.ConsoleFont.LineSpacing * 5;
            }
        }

        public void Update()
        {
            nextCharDelay = Math.Max(nextCharDelay - 1, 0);
            
            if (cursorLine >= 0 && cursorLine < lines.Length)
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
                    nextCharDelay = 4;
                }
                SetOutput(textOutput, index);
                if (index >= textOutput.Length)
                {
                    mode = Mode.Nothing;
                    for (int i = 0; i < lines.Length - 1; i++)
                    {
                        if (lines[i].Length > 0) cursorLine = i;
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
                lines[lines.Length - 1] = textInput;
            }
            else if (mode == Mode.WaitForKeyPress)
            {
                if (Globals.Input.WasAnyKeyPressedThisFrame())
                {
                    mode = Mode.Nothing;
                }
                SetOutput(textOutput);
                lines[lines.Length - 1] = textInput;
            }
            else if (mode == Mode.WaitForTime)
            {
                frameCountdown--;
                if (frameCountdown <= 0)
                {
                    mode = Mode.Nothing;
                }
                SetOutput(textOutput);
                lines[lines.Length - 1] = textInput;
            }
            else if (mode == Mode.Nothing)
            {
                SetOutput(textOutput);
                lines[lines.Length - 1] = textInput;
            }

            if (showBlink && cursorLine >= 0 && cursorLine < lines.Length)
            {
                lines[cursorLine] += CursorChar;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                spriteBatch.DrawString(Resources.ConsoleFont, lines[i], linePos[i], Foreground, 0, Vector2.Zero, new Vector2(1), SpriteEffects.None, 1);            
            }
        }
    }
}
