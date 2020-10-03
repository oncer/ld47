using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SentIO.Globals;
using System;
using System.Collections.Generic;
using System.Text;

namespace SentIO.Console
{
    public class Monolog
    {
        public List<Text> Texts { get; set; }

        int line = 0;
        Text current => Texts.Count > 0 ? Texts[line] : null;

        public Monolog()
        {
            Texts = new List<Text>();
        }

        public void AddText(string text)
        {
            //float y = Texts.Count * Globals.ConsoleFont.MeasureString("A").Y;

            var textObj = new Text(text);
            //textObj.Position = new Vector2(0, y);
            Texts.Add(textObj);
        }

        public void Update(GameTime gt)
        {
            if (current.IsDone)
            {
                if (Input.IsAnyKeyPressed())
                {
                    if (line < Texts.Count - 1)
                    {
                        Texts.RemoveAt(0);                        
                    }
                }
            }

            current.Update(gt);
        }

        public void Draw (SpriteBatch spriteBatch, GameTime gt)
        {
            current.Draw(spriteBatch, gt);
        }
    }
}
