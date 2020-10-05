using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SentIO.Globals;
using SentIO.Routines;
using SentIO.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            Done,
            FadeOut
        }

        public DiceState State { get; private set; }
        private int value;
        public int Value => value + 1;

        private float x => Position.X;
        private float y => Position.Y;
        private float yVel;
        private float yGrav;
        private float yMax;
        
        public Vector2 Position { get; set; }

        float scale = 0;
        float alpha = 0f;
        float angle = 0;
        int bounced = 0;

        double angSpeed = 0;

        private Texture2D Texture => Resources.DiceTexture[value];

        public Dice()
        {
            yGrav = .15f;
            alpha = 0f;
            State = DiceState.New;
        }

        public void Roll()
        {
            scale = 2;
            alpha = 0f;
            yMax = y;
            bounced = 4;
            State = DiceState.Bounce;
            yVel = -8;
            value = (int)(RND.Get * 6);

            ChangeAngularSpeed();
        }

        public void Hide()
        {
            alpha = 0f;
        }

        void ChangeAngularSpeed()
        {
            var pow = Math.Max(bounced * 2f, 1);
            angSpeed = Math.Sign(bounced % 2 == 0 ? -1 : 1) * (1.5 + RND.Get * pow);
        }

        public void FadeOut() => State = DiceState.FadeOut;

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
                    int oldValue = value;
                    while (bounced > 1 && value == oldValue)
                        value = (int)(RND.Get * 6);

                    yVel *= -.7f;
                    if (Math.Abs(yVel) < .6f)
                    {
                        yVel = Math.Sign(yVel) * Math.Abs(.6f);
                    }

                    bounced = Math.Max(bounced - 1, 0);
                    ChangeAngularSpeed();

                    if (bounced == 0)
                    {
                        yVel = -2;
                        Position = new Vector2(x, yMax);
                        State = DiceState.Finish;
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

                Position = new Vector2(x, y + yVel);

                if (Math.Abs(y - yMax) > 128)
                {
                    if (Math.Abs(angSpeed) > 10) angSpeed *= .95f;
                    if ((angle < -320 || angle > 320) && Math.Abs(angSpeed) < 10)
                    {
                        if (angle < 0) angle = (angle + 360) % 360;
                        else if (angle > 0) angle = (-360 + angle) % 360;
                        angSpeed = 0;                        
                        State = DiceState.Done;
                    }
                    yVel *= .9f;
                }
                else
                {
                    yVel = Math.Max(yVel - .01f, -1.5f);
                    angSpeed = 12 * yVel / 1.5f;
                }
            }

            if (State == DiceState.Done)
            {
                Debug.WriteLine(angle);
                angle *= .85f;
                if (MathF.Abs(angle) < 5)
                {
                    angle = 0;
                }
            }

            if (State == DiceState.FadeOut)
            {
                scale = Math.Max(scale - .1f, 0);
                if (scale == 0)
                {
                    State = DiceState.New;
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            if (alpha <= 0f) return;
            sb.Draw(Texture, Position, null, new Color(1f, 1f, 1f, alpha), (float)Utils.DegToRad(angle), new Vector2(16, 16), new Vector2(scale), SpriteEffects.None, 1);
        }
    }
}
