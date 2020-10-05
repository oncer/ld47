using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SentIO.Globals;
using SentIO.Routines;
using SentIO.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace SentIO.MiniGame
{

    public class DiceWait : ICoroutineYield
    {
        private Dice dice;
        public DiceWait(Dice d)
        {
            dice = d;
        }
        public void Execute()
        {
        }

        public bool IsDone()
        {
            return dice.State == Dice.DiceState.Done;
        }
    }
    public class Dice
    {
        public enum DiceState
        {
            New,
            Bounce,
            Finish,
            Done
        }

        public DiceState State { get; private set; }
        public int Value { get; set; }

        private float x => Position.X;
        private float y => Position.Y;
        private float yVel;
        private float yGrav;
        private float yMax;

        public Vector2 Position { get; set; }
        
        float alpha = 0f;
        float angle = 0;
        int bounced = 0;

        double angSpeed = 0;

        private Texture2D Texture => Resources.DiceTexture[Value];

        public Dice()
        {
            yGrav = .15f;
            alpha = 0f;
            State = DiceState.New;
        }

        public void Roll()
        {
            alpha = 0f;
            yMax = y;
            bounced = 5;
            State = DiceState.Bounce;
            yVel = -8;

            ChangeAngularSpeed();
        }

        public void Hide()
        {
            alpha = 0f;
        }

        void ChangeAngularSpeed()
        {
            var pow = Math.Max(bounced * .5f, 1);
            angSpeed = Math.Sign(bounced % 2 == 0 ? 1 : -1) * (1.5 + RND.Get * pow);
        }

        public void Update()
        {
            if (State != DiceState.Done && State != DiceState.New)
            {
                alpha = MathF.Min(alpha + (1f / 30f), 1f);
            }
            if (State == DiceState.Bounce)
            {
                angle += (float)angSpeed;

                angle = angle % 360;

                yVel = Math.Min(yVel + yGrav, 999);
                if (y + yVel > yMax)
                {
                    if (bounced > 0)
                        Value = (int)(RND.Get * 6);

                    ChangeAngularSpeed();

                    yVel *= -.7f;
                    if (Math.Abs(yVel) < .6f)
                    {
                        yVel = Math.Sign(yVel) * Math.Abs(.6f);
                    }

                    bounced = Math.Max(bounced - 1, 0);

                    if (bounced == 0)
                    {
                        if ((angle % 45) < 22.5)
                            angSpeed = 6;
                        else
                            angSpeed = -6;

                        yVel = 0;
                        Position = new Vector2(x, yMax);
                        State = DiceState.Finish;
                        /*if (Math.Abs(yVel) < .2f)
                        {
                            Position = new Vector2(x, yMax);
                            yVel = 0;
                            angSpeed = 0;
                            State = DiceState.Done;
                        }*/
                    }
                }
                else
                {
                    Position = new Vector2(x, y + yVel);
                }                
            }

            if (State == DiceState.Finish)
            {
                angle += (float)angSpeed;
                angle = angle % 360;

                yVel = -2;
                Position = new Vector2(x, y + yVel);

                if (Math.Abs(y - yMax) > 128)
                {
                    yVel = 0;
                    State = DiceState.Done;
                }

                if (Math.Abs(y - yMax) > 96)
                {
                    if (Math.Abs((angle % 90) - 10) < 8)
                    {
                        angle = 0;
                        angSpeed = 0;
                    }
                }
            }

            if (State == DiceState.Done)
            {
                //angle = 0;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            if (alpha <= 0f) return;
            sb.Draw(Texture, Position, null, new Color(1f, 1f, 1f, alpha), (float)Utils.DegToRad(angle), new Vector2(16, 16), new Vector2(2), SpriteEffects.None, 1);
        }
    }
}
