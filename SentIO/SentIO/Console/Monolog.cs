using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace SentIO.Console
{
    public class Monolog
    {
        public List<Text> Texts { get; set; }

        int line = 0;
        Text current => Texts[line];

        public void MoveDown()
        {
            
        }

        public Monolog()
        {
            Texts = new List<Text>();
        }

        public void AddText(string text)
        {
            float y = 0;
            if (Texts.Count > 0)
            {
                y = Texts[Texts.Count - 1].Position.Y + Globals.ConsoleFont.MeasureString("A").Y;
            }
            Texts.Add(new Text(text));
            current.Position = new Vector2(current.Position.X, y);
        }

        public void Update(GameTime gt)
        {
            if (current.IsDone)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    if (line < Texts.Count - 1)
                    {
                        current.ShowCursor = false;
                        line++;                        
                    }
                }
            }

            for (var i = 0; i <= line; i++)
            {
                Texts[i].Update(gt);
            }
        }

        public void Draw (SpriteBatch spriteBatch, GameTime gt)
        {
            for(var i = 0; i <= line; i++)
            {
                Texts[i].Draw(spriteBatch, gt);
            }
        }
    }
}
