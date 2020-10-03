using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
            Input
        }
        private Mode mode;


        private string[] lines = new string[2];
        private int cursorLine;
        private string textOutput;
        private int index;
        private int nextCharDelay;
        private int blinkDelay;

        private bool showBlink;

        public Text()
        {
            mode = Mode.Nothing;
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = "";
            }
            textOutput = "";
            index = 0;
            nextCharDelay = 0;
            blinkDelay = 0;
            cursorLine = -1;
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
            return new Wait(this);
        }

        public ICoroutineYield Input()
        {
            mode = Mode.Input;

            return new Wait(this);
        }

        public void Update (GameTime gt)
        {
            nextCharDelay = Math.Max(nextCharDelay - 1, 0);
            
            if (mode == Mode.Nothing)
            {
                blinkDelay = Math.Max(blinkDelay - 1, 0);
                if (blinkDelay == 0)
                {
                    showBlink = !showBlink;
                    blinkDelay = 32;
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
            
            if (cursorLine >= 0 && cursorLine < lines.Length)
            {
                lines[cursorLine] += "_";
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
