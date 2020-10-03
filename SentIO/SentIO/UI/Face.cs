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
        public enum Mood
        {
            TalkNeutral,
            IdleNeutral,
        }

        public Vector2 Position { get; set; }

        public Animation CurrentAnimation => moods[CurrentMood];
        
        private Mood mood;
        public Mood CurrentMood
        {
            get { return mood; }
            set
            {
                mood = value;
                CurrentAnimation.Restart();
            }
        }

        private Dictionary<Mood, Animation> moods;

        public Face(Vector2 position)
        {
            Position = position;

            moods = new Dictionary<Mood, Animation>();

            moods.Add(Mood.TalkNeutral, new Animation(Resources.FaceTexture, 0, 6, .25, true));
            moods.Add(Mood.IdleNeutral, new Animation(Resources.FaceTexture, 7, 13, .15, true));;

            CurrentMood = Mood.TalkNeutral;
        }

        public void Update(GameTime gt)
        {
            CurrentAnimation.Update(gt);
        }

        public void Draw(SpriteBatch sb, GameTime gt)
        {
            CurrentAnimation.Draw(sb, gt, Position, Color.White, 0, new Vector2(16), new Vector2(2), 1);
        }
    }
}
