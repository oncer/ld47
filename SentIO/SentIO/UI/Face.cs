using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SentIO.Globals;
using SentIO.Routines;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SentIO.UI
{
    public class Face
    {
        public enum Emotion
        {
            TalkNeutral,
            IdleNeutral,
            TalkHappy,
            IdleHappy,
            Excite
        }

        private static Face instance;
        public static Face Instance
        {
            get
            {
                if (instance == null)
                    instance = new Face();
                return instance;
            }
        }

        private Dictionary<Emotion, Animation> moods;
        private Emotion mood;

        public Vector2 Position { get; set; }

        private Animation CurrentAnimation => moods[mood];

        public Emotion CurrentMood
        {
            get { return mood; }
            set
            {
                mood = value;
                CurrentAnimation.Restart();
            }
        }

        public bool IsVisible { get; set; } = true;

        private Face()
        {
            moods = new Dictionary<Emotion, Animation>();

            moods.Add(Emotion.TalkNeutral, new Animation(Resources.FaceTexture, 0, 6, .25, true));
            moods.Add(Emotion.IdleNeutral, new Animation(Resources.FaceTexture, 7, 27, .16, true));
            moods.Add(Emotion.TalkHappy, new Animation(Resources.FaceTexture, 28, 34, .25, true));
            moods.Add(Emotion.IdleHappy, new Animation(Resources.FaceTexture, 35, 55, .16, true));
            moods.Add(Emotion.Excite, new Animation(Resources.FaceTexture, 56, 76, .2, false));

            CurrentMood = Emotion.IdleNeutral;
        }

        public void Update()
        {
            CurrentAnimation.Update();
        }

        public void Draw(SpriteBatch sb)
        {
            if (IsVisible)
                CurrentAnimation.Draw(sb, Position, Color.White, 0, new Vector2(32, 16), new Vector2(2), 1);
        }

        public class WaitForAnimation : ICoroutineYield
        {
            public WaitForAnimation() { }
            public void Execute() { }
            public bool IsDone() => Instance.CurrentAnimation.IsDone;
        }

        internal ICoroutineYield WaitForAnimationEnd()
        {
            return new WaitForAnimation();
        }
    }
}
