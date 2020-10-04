using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SentIO.Globals;
using SentIO.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace SentIO.MiniGame
{
    public class Dice
    {
        public enum DiceState
        {
            New,
            Bounce,
            Done
        }

        public DiceState State { get; private set; }

        private float x => Position.X;
        private float y => Position.Y;
        private float yVel;
        private float yGrav;
        private float yMax;

        public Vector2 Position { get; set; }
        
        int diceValue = 0;
        float angle = 0;
        int bounced = 0;

        double angSpeed = 0;

        private Texture2D Texture => Resources.DiceTexture[diceValue];

        public Dice()
        {
            yGrav = .15f;
        }

        public void Roll()
        {
            yMax = y;
            bounced = 5;
            State = DiceState.Bounce;
            yVel = -8;

            ChangeAngularSpeed();
        }

        void ChangeAngularSpeed()
        {
            var pow = Math.Max(bounced * .5f, 2);
            angSpeed = -pow + RND.Get * 2 * pow;
        }

        public void Update()
        {
            if (State == DiceState.Bounce)
            {
                angle += (float)angSpeed;

                angle = angle % 360;

                yVel = Math.Min(yVel + yGrav, 999);
                if (y + yVel > yMax)
                {
                    diceValue = (int)(RND.Get * 6);

                    ChangeAngularSpeed();

                    yVel *= -.7f;
                    bounced--;

                    if (bounced == 0)
                    {
                        Position = new Vector2(x, yMax);
                        yVel = 0;
                        angSpeed = 0;
                        State = DiceState.Done;
                    }
                }
                else
                {
                    Position = new Vector2(x, y + yVel);
                }                
            }

            if (State == DiceState.Done)
            {
                //angle = 0;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(Texture, Position, null, Color.White, (float)Utils.DegToRad(angle), new Vector2(16, 16), new Vector2(2), SpriteEffects.None, 1);
        }
    }
}
