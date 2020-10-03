using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SentIO.Globals;
using System;
using System.Collections.Generic;
using System.Text;

namespace SentIO.UI
{
    public class Face
    {
        public Vector2 Position { get; set; }
        public Face(Vector2 position)
        {
            Position = position;
            animation = new Animation(Resources.FaceTexture, 0, 1, .2, true);
        }

        private Animation animation;

        public void Update(GameTime gt)
        {
            animation.Update(gt);
        }

        public void Draw(SpriteBatch sb, GameTime gt)
        {
            animation.Draw(sb, gt, Position, Color.White, 0, new Vector2(0), new Vector2(1), 1);
        }
    }
}
