//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using SentIO.Globals;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace SentIO.Console
//{
//    public class Monolog
//    {
//        public List<Text> Texts { get; set; }
//        private Text current => Texts.Count > 0 ? Texts[0] : null;

//        public event EventHandler Complete;

//        public Monolog()
//        {
//            Texts = new List<Text>();
//        }

//        public void AddText(string text)
//        {
//            Texts.Add(new Text(text));
//        }

//        public void Update(GameTime gt)
//        {
//            if (current != null && current.IsDone)
//            {
//                if (Input.IsAnyKeyPressed())
//                {
//                    if (Texts.Count > 0)
//                    {
//                        Texts.RemoveAt(0);
//                        if (Texts.Count == 0)
//                        {
//                            Complete?.Invoke(this, new EventArgs());
//                        }
//                    }
//                }
//            }

//            current?.Update(gt);
//        }

//        public void Draw (SpriteBatch spriteBatch, GameTime gt)
//        {
//            current?.Draw(spriteBatch, gt);
//        }
//    }
//}
