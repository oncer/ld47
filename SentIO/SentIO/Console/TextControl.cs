using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SentIO.Globals;
using SentIO.Routines;
using System;
using System.Collections.Generic;
using System.IO;
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
            public bool IsDone() => TextControl.Instance.IsDone;
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
        public string InputResult { get; private set; }


        private string[] lines = new string[2];
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
        }

        public ICoroutineYield Show(string text)
        {
            mode = Mode.Output;
            lines[0] = "";
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
            cursorLine = 1;
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
                    } else
                    {
                        mode = Mode.Nothing;
                        cursorLine = 0;
                    }
                    nextCharDelay = 4;
                }
                lines[0] = textOutput.Substring(0, index);
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
                lines[1] = textInput;
            }
            else if (mode == Mode.WaitForKeyPress)
            {
                if (Globals.Input.WasAnyKeyPressedThisFrame())
                {
                    mode = Mode.Nothing;
                }
                lines[0] = textOutput;
                lines[1] = textInput;
            }
            else if (mode == Mode.WaitForTime)
            {
                frameCountdown--;
                if (frameCountdown <= 0)
                {
                    mode = Mode.Nothing;
                }
                lines[0] = textOutput;
                lines[1] = textInput;
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
                spriteBatch.DrawString(Resources.ConsoleFont, lines[i], new Vector2(0, i * Resources.ConsoleFont.LineSpacing), Foreground, 0, Vector2.Zero, new Vector2(1), SpriteEffects.None, 1);            
            }
        }
    }
}
