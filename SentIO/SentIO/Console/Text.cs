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
    public class Text
    {
        public class Wait : ICoroutineYield
        {
            Text parent;
            public Wait(Text _parent)
            {
                parent = _parent;
            }

            public void Execute()
            {
            }

            public bool IsDone()
            {
                return parent.IsDone;
            }
        }
        public bool IsDone => mode == Mode.Nothing;
        public Vector2 Position { get; set; } = Vector2.Zero;

        public enum Mode
        {
            Nothing,
            Output,
            Input,
            WaitForKeyPress
        }
        private Mode mode;
        public string InputResult { get; private set; }


        private string[] lines = new string[2];
        private int cursorLine;
        private string textOutput;
        private int index;
        private int nextCharDelay;
        private int blinkDelay;

        private bool showBlink;
        private static char CursorChar = '_';
        private static int CursorBlinkDelay = 20;

        public Text()
        {
            mode = Mode.Nothing;
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = "";
            }
            textOutput = "";
            InputResult = "";
            index = 0;
            nextCharDelay = 0;
            blinkDelay = 0;
            cursorLine = -1;
            showBlink = false;
        }

        public ICoroutineYield Show(string _text)
        {
            mode = Mode.Output;
            lines[0] = "";
            textOutput = _text;
            index = 0;
            nextCharDelay = 0;
            blinkDelay = 0;
            cursorLine = -1;
            showBlink = false;
            return new Wait(this);
        }

        public ICoroutineYield Input()
        {
            mode = Mode.Input;
            lines[1] = "";
            InputResult = "";
            cursorLine = 1;
            showBlink = false;
            return new Wait(this);
        }

        public ICoroutineYield WaitForKeyPress()
        {
            mode = Mode.WaitForKeyPress;
            return new Wait(this);
        }

        public void Update (GameTime gt)
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
                            InputResult += key.ToString();
                        }
                        else
                        {
                            InputResult += key.ToString().ToLower();
                        }
                    }
                    else if (key == Keys.Space)
                    {
                        InputResult += " ";
                    }
                    else if (key == Keys.Back && InputResult.Length > 0)
                    {
                        InputResult = InputResult.Remove(InputResult.Length - 1);
                    }
                    else if (key == Keys.Enter)
                    {
                        mode = Mode.Nothing;
                    }
                }
                if (mode == Mode.Input)
                {
                    lines[1] = InputResult;
                } 
                else
                {
                    lines[1] = "";
                }
            }
            else if (mode == Mode.WaitForKeyPress)
            {
                if (Globals.Input.WasAnyKeyPressedThisFrame())
                {

                }
            }

            if (showBlink && cursorLine >= 0 && cursorLine < lines.Length)
            {
                lines[cursorLine] += CursorChar;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gt)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                spriteBatch.DrawString(Resources.ConsoleFont, lines[i], new Vector2(0, i * Resources.ConsoleFont.LineSpacing), Color.White, 0, Vector2.Zero, new Vector2(1), SpriteEffects.None, 1);            
            }
        }
    }
}
