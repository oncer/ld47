using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SentIO.Text
{
    public class TextManager
    {
        public SpriteFont Font { get; set; }

        private string text;

        private string targetText = "Hello my dear world.";
        private int index = 0;
        private int delay = 0;

        public void Update (GameTime gt)
        {
            if (delay == 0)
            {
                if (index < targetText.Length)
                {
                    index++;
                    delay = 5;
                }
            }
            delay = Math.Max(delay - 1, 0);

            text = targetText.Substring(0, index);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gt)
        {
            spriteBatch.DrawString(Font, text, Vector2.Zero, Color.White, 0, Vector2.Zero, new Vector2(1), SpriteEffects.None, 1);
        }
    }
}
