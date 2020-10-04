using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SentIO.UI
{
    public class Animation
    {
        private TextureSet frames;
        
        private double lastFrame = 0;
        private double currentFrame = 0;
        public int AnimationFrame
        {
            get => MinFrame + (int)Math.Floor(currentFrame);
        }
        public int MinFrame { get; protected set; }
        public int MaxFrame { get; protected set; }
        public double AnimationSpeed { get; protected set; }
        private bool loop = false;

        public Texture2D Texture
        {
            get => (frames != null) ? frames[AnimationFrame] : null;
            set
            {
                if (frames == null || value != frames[0])
                    frames = TextureSet.FromTexture(value);
                currentFrame = 0;
            }
        }

        public bool IsDone { get; private set; }

        public Animation(TextureSet frames, int minFrame, int frameCount, double animationSpeed, bool loop)
        {
            this.frames = frames;
            MinFrame = minFrame;
            MaxFrame = minFrame + frameCount - 1;
            AnimationSpeed = animationSpeed;
            this.loop = loop;

            IsDone = false;
        }

        public void Restart()
        {
            lastFrame = -1;
            currentFrame = 0;
            IsDone = false;
        }

        public void Update()
        {
            if (MaxFrame > MinFrame && MaxFrame > 0)
            {
                if (loop)
                {
                    var last = currentFrame;
                    currentFrame = currentFrame + AnimationSpeed;
                    if (currentFrame > (MaxFrame - MinFrame) + 1 - AnimationSpeed)
                    {
                        currentFrame -= ((MaxFrame - MinFrame) + 1 - AnimationSpeed);
                        IsDone = true;
                    }
                }
                else
                {
                    currentFrame = Math.Min(currentFrame + AnimationSpeed, MaxFrame - MinFrame);

                    if (currentFrame == MaxFrame - MinFrame && currentFrame != lastFrame)
                        IsDone = true;
                }
            }
            lastFrame = currentFrame;
        }

        public void Draw(SpriteBatch sb, Vector2 position, Color color, float angle, Vector2 origin, Vector2 scale, float depth)
        {
            if (Texture != null)
            {
                sb.Draw(Texture, position, null, color, angle, origin, scale, SpriteEffects.None, depth);
            }
        }
    }
}
