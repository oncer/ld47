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
        public string PlayerName { get; set; }

        public bool PlayerVisible { get; set; }
        public bool SidVisible { get; set; }

        float playerAlpha = 0f;
        float playerRoundAlpha = 0f;
        float sidAlpha = 0f;
        float sidRoundAlpha = 0f;

        public int PlayerOnes { get; private set; }
        public int SidOnes { get; private set; }

        public int PlayerScore { get; private set; }
        public int SidScore { get; private set; }

        private int visiblePlayerScore;
        private int visibleSidScore;

        public int PlayerRoundScore { get; private set; }
        public int SidRoundScore { get; private set; }

        private int visiblePlayerRoundScore;
        private int visibleSidRoundScore;


        private int scoreDelay;

        public bool IsDone => 
            visiblePlayerScore == PlayerScore 
            && visibleSidScore == SidScore
            && visiblePlayerRoundScore == PlayerRoundScore
            && visibleSidRoundScore == SidRoundScore;
        public ScoreBoard()
        {
            Reset();
        }

        public void Reset()
        {
            PlayerScore = 0;
            SidScore = 0;
            visibleSidScore = 0;
            visiblePlayerScore = 0;
            PlayerRoundScore = 0;
            SidRoundScore = 0;
            visibleSidRoundScore = 0;
            visiblePlayerRoundScore = 0;
            PlayerVisible = false;
            SidVisible = false;
            playerAlpha = 0f;
            sidAlpha = 0f;
            scoreDelay = 0;
            PlayerOnes = 0;
            SidOnes = 0;
        }

        public void Hide()
        {
            PlayerVisible = false;
            SidVisible = false;
        }

        public Wait AddPlayerRoundScore(int score)
        {
            PlayerRoundScore += score;
            return new Wait(this);
        }

        public Wait ZeroPlayerRoundScore()
        {
            PlayerRoundScore = 0;
            PlayerOnes++;
            return new Wait(this);
        }

        public Wait HoldPlayerRoundScore()
        {
            PlayerScore += PlayerRoundScore;
            PlayerRoundScore = 0;
            return new Wait(this);
        }

        public Wait AddPlayerScore(int score)
        {
            PlayerScore += score;
            return new Wait(this);
        }

        public Wait AddSidRoundScore(int score)
        {
            SidRoundScore += score;
            return new Wait(this);
        }

        public Wait ZeroSidRoundScore()
        {
            SidRoundScore = 0;
            SidOnes++;
            return new Wait(this);
        }

        public Wait HoldSidRoundScore()
        {
            SidScore += SidRoundScore;
            SidRoundScore = 0;
            return new Wait(this);
        }

        public Wait AddSidScore(int score)
        {
            SidScore += score;
            return new Wait(this);
        }

        public void Update()
        {
            if (PlayerVisible)
            {
                playerAlpha = MathF.Min(playerAlpha + (1f/30f), 1f);
            }
            else
            {
                playerAlpha = MathF.Max(playerAlpha - (1f/20f), 0f);
            }

            if (SidVisible)
            {
                sidAlpha = MathF.Min(sidAlpha + (1f/30f), 1f);
            }
            else
            {
                sidAlpha = MathF.Max(sidAlpha - (1f/20f), 0f);
            }
            
            if (SidRoundScore > 0 && sidAlpha >= 1f)
            {
                sidRoundAlpha = MathF.Min(sidRoundAlpha + (1f/20f), 1f);
            }
            else if ((visibleSidRoundScore <= 0 && sidAlpha >= 1f) || !SidVisible)
            {
                sidRoundAlpha = MathF.Max(sidRoundAlpha - (1f/20f), 0f);
            }

            if (PlayerRoundScore > 0 && playerAlpha >= 1f)
            {
                playerRoundAlpha = MathF.Min(playerRoundAlpha + (1f/30f), 1f);
            }
            else if ((visiblePlayerRoundScore <= 0 && playerAlpha >= 1f) || !PlayerVisible)
            {
                playerRoundAlpha = MathF.Max(playerRoundAlpha - (1f/30f), 0f);
            }

            if (scoreDelay <= 0)
            {
                if (visiblePlayerScore < PlayerScore) visiblePlayerScore++;
                if (visibleSidScore < SidScore) visibleSidScore++;
                if (playerRoundAlpha >= 1f)
                {
                    if (visiblePlayerRoundScore < PlayerRoundScore) visiblePlayerRoundScore++;
                    if (visiblePlayerRoundScore > PlayerRoundScore) visiblePlayerRoundScore--;
                }
                if (sidRoundAlpha >= 1f)
                {
                    if (visibleSidRoundScore < SidRoundScore) visibleSidRoundScore++;
                    if (visibleSidRoundScore > SidRoundScore) visibleSidRoundScore--;
                }
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
            string playerRoundScoreStr = $"+{visiblePlayerRoundScore}";
            Vector2 playerPos = new Vector2(windowWidth * 0.2f, y);
            Vector2 playerStrPos = playerPos - new Vector2(Resources.ConsoleFont.MeasureString(playerStr).X / 2, 0);
            Vector2 playerScorePos = playerPos + new Vector2(-Resources.ConsoleFont.MeasureString(playerScoreStr).X / 2, Resources.ConsoleFont.LineSpacing);
            Vector2 playerRoundScorePos = playerPos + new Vector2(-Resources.ConsoleFont.MeasureString(playerRoundScoreStr).X / 2, Resources.ConsoleFont.LineSpacing * 2);
            Color playerColor = Resources.TextColor2;
            playerColor.A = (byte)(playerAlpha * 255f);
            Color playerRoundColor = Resources.TextColor2;
            playerRoundColor.A = (byte)(playerRoundAlpha * 255f);
            Vector2 sidPos = new Vector2(windowWidth * 0.8f, y);
            string sidStr = $"{Resources.DisplayName}";
            string sidScoreStr = visibleSidScore.ToString();
            string sidRoundScoreStr = $"+{visibleSidRoundScore}";
            Vector2 sidStrPos = sidPos - new Vector2(Resources.ConsoleFont.MeasureString(sidStr).X / 2, 0);
            Vector2 sidScorePos = sidPos + new Vector2(-Resources.ConsoleFont.MeasureString(sidScoreStr).X / 2, Resources.ConsoleFont.LineSpacing);
            Vector2 sidRoundScorePos = sidPos + new Vector2(-Resources.ConsoleFont.MeasureString(sidRoundScoreStr).X / 2, Resources.ConsoleFont.LineSpacing * 2);
            Color sidColor = Resources.TextColor2;
            sidColor.A = (byte)(sidAlpha * 255f);
            Color sidRoundColor = Resources.TextColor2;
            sidRoundColor.A = (byte)(sidRoundAlpha * 255f);

            spriteBatch.DrawString(Resources.ConsoleFont, playerStr, playerStrPos, playerColor, 0, Vector2.Zero, new Vector2(1), SpriteEffects.None, 1);
            spriteBatch.DrawString(Resources.ConsoleFont, playerScoreStr, playerScorePos, playerColor, 0, Vector2.Zero, new Vector2(1), SpriteEffects.None, 1);
            spriteBatch.DrawString(Resources.ConsoleFont, playerRoundScoreStr, playerRoundScorePos, playerRoundColor, 0, Vector2.Zero, new Vector2(1), SpriteEffects.None, 1);
            
            spriteBatch.DrawString(Resources.ConsoleFont, sidStr, sidStrPos, sidColor, 0, Vector2.Zero, new Vector2(1), SpriteEffects.None, 1);
            spriteBatch.DrawString(Resources.ConsoleFont, sidScoreStr, sidScorePos, sidColor, 0, Vector2.Zero, new Vector2(1), SpriteEffects.None, 1);
            spriteBatch.DrawString(Resources.ConsoleFont, sidRoundScoreStr, sidRoundScorePos, sidRoundColor, 0, Vector2.Zero, new Vector2(1), SpriteEffects.None, 1);
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
