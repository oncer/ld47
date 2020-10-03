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
            animTalk = new Animation(Resources.FaceTexture, 0, 3, .1, true);

            currentAnimation = animTalk;
        }

        private Animation currentAnimation;
        private Animation animTalk;

        public void Update(GameTime gt)
        {
            currentAnimation.Update(gt);
        }

        public void Draw(SpriteBatch sb, GameTime gt)
        {
            currentAnimation.Draw(sb, gt, Position, Color.White, 0, new Vector2(16), new Vector2(2), 1);
        }
    }
}
