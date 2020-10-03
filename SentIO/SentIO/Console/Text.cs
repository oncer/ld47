using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SentIO.Globals;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace SentIO.Console
{
    public class Text
    {
        public bool IsDone { get; private set; } = false;
        public Vector2 Position { get; set; } = Vector2.Zero;
        public event EventHandler Complete;

        private string shownText;
        private string text;
        private int index;
        private int nextCharDelay;
        private int blinkDelay;

        private bool invoked;
        private bool showBlink;

        public Text(string text)
        {
            this.text = text;
        }

        private Text()
        {
            shownText = "";
            text = "";
            index = 0;
            nextCharDelay = 0;
            blinkDelay = 0;
            invoked = false;
            showBlink = false;
            Next = null;
        }

        private void ResetWith(Text next)
        {
            IsDone = false;
            shownText = "";
            text = next.text;
            index = 0;
            nextCharDelay = 0;
            blinkDelay = 0;
            invoked = false;
            showBlink = false;
            Complete = Next.Complete;
            Next = Next.Next;
        }

        public Text Next { get; set; }

        public void Update (GameTime gt)
        {
            nextCharDelay = Math.Max(nextCharDelay - 1, 0);
            
            if (IsDone)
            {
                blinkDelay = Math.Max(blinkDelay - 1, 0);
                if (blinkDelay == 0)
                {
                    showBlink = !showBlink;
                    blinkDelay = 32;
                }
            }
            
            if (nextCharDelay == 0)
            {
                if (index < text.Length)
                {
                    index++;
                } else
                {
                    IsDone = true;
                }
                nextCharDelay = 4;
            }
            
            shownText = text.Substring(0, index);

            if (index == text.Length)
            {
                if (showBlink) shownText += "_";                
            }

            if (IsDone && !invoked)
            {
                if (Input.IsAnyKeyPressed())
                {
                    invoked = true;
                    Complete?.Invoke(this, new EventArgs());
                    if (Next != null)
                    {
                        ResetWith(Next);                        
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gt)
        {
            if (shownText == null)
                return;

            spriteBatch.DrawString(Resources.ConsoleFont, shownText, Position, Color.White, 0, Vector2.Zero, new Vector2(1), SpriteEffects.None, 1);            
        }
    }
}
