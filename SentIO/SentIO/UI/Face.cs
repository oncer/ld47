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
        public enum Mood
        {
            TalkNeutral,
            IdleNeutral,
            TalkHappy,
            IdleHappy
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

        private Dictionary<Mood, Animation> moods;
        private Mood mood;

        public Vector2 Position { get; set; }

        private Animation CurrentAnimation => moods[mood];

        public Mood CurrentMood
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
            moods = new Dictionary<Mood, Animation>();

            moods.Add(Mood.TalkNeutral, new Animation(Resources.FaceTexture, 0, 6, .25, true));
            moods.Add(Mood.IdleNeutral, new Animation(Resources.FaceTexture, 7, 27, .16, true));
            moods.Add(Mood.TalkHappy, new Animation(Resources.FaceTexture, 28, 34, .25, true));
            moods.Add(Mood.IdleHappy, new Animation(Resources.FaceTexture, 35, 55, .16, true));

            CurrentMood = Mood.IdleNeutral;
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
