using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SentIO.Console
{
    public class Text
    {
        public bool IsDone { get; private set; } = false;

        private string shownText;
        private string text;
        private int index = 0;
        private int nextCharDelay = 0;        
        private int blinkDelay = 0;

        private bool showBlink = false;

        public Vector2 Position { get; set; } = Vector2.Zero;

        public Text(string text)
        {
            this.text = text;
        }

        private Text() { }

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
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gt)
        {
            spriteBatch.DrawString(Resources.ConsoleFont, shownText, Position, Color.White, 0, Vector2.Zero, new Vector2(1), SpriteEffects.None, 1);            
        }
    }
}
