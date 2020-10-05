using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SentIO.Globals;
using SentIO.Routines;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SentIO.MiniGame
{

    public class ScoreBoard
    {    
        const int ScoreDelayFrames = 4;
        public string PlayerName {get; set;}

        public bool PlayerVisible { get; set; }
        public bool SidVisible { get; set; }

        float playerAlpha = 0f;
        float sidAlpha = 0f;

        public int PlayerScore { get; private set; }
        public int SidScore { get; private set; }

        private int visiblePlayerScore;
        private int visibleSidScore;

        private int scoreDelay;

        public bool IsDone => visiblePlayerScore == PlayerScore && visibleSidScore == SidScore;
        public ScoreBoard()
        {
            Reset();
        }

        public void Reset()
        {
            PlayerScore = 0;
            SidScore = 0;
            PlayerVisible = false;
            SidVisible = false;
            playerAlpha = 0f;
            sidAlpha = 0f;
            scoreDelay = 0;
        }

        public Wait AddPlayerScore(int score)
        {
            PlayerScore += score;
            PlayerVisible = true;
            return new Wait(this);
        }

        public Wait AddSidScore(int score)
        {
            SidScore += score;
            SidVisible = true;
            return new Wait(this);
        }

        public void Update()
        {
            if (PlayerVisible)
            {
                playerAlpha = MathF.Min(playerAlpha + (1f/30f), 1f);
            }
            if (SidVisible)
            {
                sidAlpha = MathF.Min(sidAlpha + (1f/30f), 1f);
            }
            
            if (scoreDelay <= 0)
            {
                if (visiblePlayerScore < PlayerScore) visiblePlayerScore++;
                if (visibleSidScore < SidScore) visibleSidScore++;
                scoreDelay = ScoreDelayFrames;
            }
            else scoreDelay--;

        }

        public void Draw(SpriteBatch spriteBatch)
        {                
            int windowWidth = MainGame.Instance.Window.ClientBounds.Width;
            int windowHeight = MainGame.Instance.Window.ClientBounds.Height;
            float y = windowHeight * 0.6f;
            string playerStr = $"{PlayerName}";
            string playerScoreStr = visiblePlayerScore.ToString();
            Vector2 playerPos = new Vector2(windowWidth * 0.2f, y);
            Vector2 playerStrPos = playerPos - new Vector2(Resources.ConsoleFont.MeasureString(playerStr).X / 2, 0);
            Vector2 playerScorePos = playerPos + new Vector2(-Resources.ConsoleFont.MeasureString(playerScoreStr).X / 2, Resources.ConsoleFont.LineSpacing);
            Color playerColor = Resources.TextColor2;
            playerColor.A = (byte)(playerAlpha * 255f);
            Vector2 sidPos = new Vector2(windowWidth * 0.8f, y);
            string sidStr = $"{Resources.DisplayName}";
            string sidScoreStr = visibleSidScore.ToString();
            Vector2 sidStrPos = sidPos - new Vector2(Resources.ConsoleFont.MeasureString(sidStr).X / 2, 0);
            Vector2 sidScorePos = sidPos + new Vector2(-Resources.ConsoleFont.MeasureString(sidScoreStr).X / 2, Resources.ConsoleFont.LineSpacing);
            Color sidColor = Resources.TextColor2;
            sidColor.A = (byte)(sidAlpha * 255f);

            spriteBatch.DrawString(Resources.ConsoleFont, playerStr, playerStrPos, playerColor, 0, Vector2.Zero, new Vector2(1), SpriteEffects.None, 1);
            spriteBatch.DrawString(Resources.ConsoleFont, playerScoreStr, playerScorePos, playerColor, 0, Vector2.Zero, new Vector2(1), SpriteEffects.None, 1);
            spriteBatch.DrawString(Resources.ConsoleFont, sidStr, sidStrPos, sidColor, 0, Vector2.Zero, new Vector2(1), SpriteEffects.None, 1);
            spriteBatch.DrawString(Resources.ConsoleFont, sidScoreStr, sidScorePos, sidColor, 0, Vector2.Zero, new Vector2(1), SpriteEffects.None, 1);
        }

        public class Wait: ICoroutineYield
        {
            ScoreBoard scoreBoard;
            public Wait(ScoreBoard _scoreBoard)
            {
                scoreBoard = _scoreBoard;
            }
            public void Execute()
            {
            }
            public bool IsDone()
            {
                return scoreBoard.IsDone;
            }
        }
    }
}
