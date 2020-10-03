using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SentIO.UI
{
    public class Animation
    {
        private readonly TextureSet frames;
        private readonly int minFrame;
        private readonly int maxFrame;
        private readonly double animationSpeed;
        private readonly bool loop;

        public Animation(TextureSet frames, int minFrame, int maxFrame, double animationSpeed, bool loop)
        {
            this.frames = frames;
            this.minFrame = minFrame;
            this.maxFrame = maxFrame;
            this.animationSpeed = animationSpeed;
            this.loop = loop;
        }

        public void Update(GameTime gt)
        {

        }

        public void Draw(SpriteBatch spriteBatch, GameTime gt)
        {

        }
    }
}
