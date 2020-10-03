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

        public event EventHandler AnimationComplete;

        public Animation(TextureSet frames, int minFrame, int maxFrame, double animationSpeed, bool loop)
        {
            this.frames = frames;
            MinFrame = minFrame;
            MaxFrame = maxFrame;
            AnimationSpeed = animationSpeed;
            this.loop = loop;
        }

        public void Restart()
        {
            currentFrame = MinFrame;
        }

        public void Update(GameTime gt)
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
                        AnimationComplete?.Invoke(this, new EventArgs());
                    }
                }
                else
                {
                    currentFrame = Math.Min(currentFrame + AnimationSpeed, MaxFrame - MinFrame);

                    if (currentFrame == MaxFrame - MinFrame && currentFrame != lastFrame)
                        AnimationComplete?.Invoke(this, new EventArgs());
                }
            }
            lastFrame = currentFrame;
        }

        public void Draw(SpriteBatch sb, GameTime gt, Vector2 position, Color color, float angle, Vector2 origin, Vector2 scale, float depth)
        {
            if (Texture != null)
            {
                sb.Draw(Texture, position, null, color, angle, origin, scale, SpriteEffects.None, depth);
            }
        }
    }
}
