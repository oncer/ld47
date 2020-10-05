using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Xna.Framework;
using SentIO.Globals;
using SentIO.Routines;
using SentIO.UI;
using System;
using System.Collections;
using SentIO.MiniGame;
using System.Linq;

namespace SentIO.Console
{
    class Script
    {
        private int phase;
        
        public Script()
        {
            //MainGame.Instance.Suicide(); // please don't
            if (!int.TryParse(SaveData.Instance["phase"], out phase))
            {
                phase = 2;
            }
            if (SaveData.Instance.ExeName.ToLower() != "sid")
            {
                phase = 2;
            }
            else if (phase < 3)
            {
                phase = 3;
            }

            if (phase < 3)
            {
                SoundControl.IsEnabled = false;
                Face.Instance.IsVisible = false;
                TextControl.Instance.Background = Resources.BGColor1;
                TextControl.Instance.Foreground = Resources.TextColor1;
                TextControl.Instance.SetLayout(TextControl.Layout.TopLeft);
            }
            else
            {
                SoundControl.IsEnabled = true;
                Face.Instance.IsVisible = true;
                TextControl.Instance.Background = Resources.BGColor2;
                TextControl.Instance.Foreground = Resources.TextColor2;
                TextControl.Instance.SetLayout(TextControl.Layout.Centered);
            }
            switch (phase)
            {
                default: MainGame.Instance.StartCoroutine(Phase2()); break;
                case 3: MainGame.Instance.StartCoroutine(Phase3()); break;
                case 4: MainGame.Instance.StartCoroutine(Phase4()); break;
            }
        }

        private bool IsValidPlayerName(string playerName)
        {
            return playerName.Length > 2;
        }

        #region helpers
        void StopTalk()
        {
            switch (Face.Instance.CurrentMood)
            {
                case Face.Emotion.TalkHappy:
                    Face.Instance.CurrentMood = Face.Emotion.IdleHappy;
                    break;
                case Face.Emotion.TalkNeutral:
                    Face.Instance.CurrentMood = Face.Emotion.IdleNeutral;
                    break;
                case Face.Emotion.TalkSad:
                    Face.Instance.CurrentMood = Face.Emotion.IdleSad;
                    break;
                case Face.Emotion.TalkAngry:
                    Face.Instance.CurrentMood = Face.Emotion.IdleAngry;
                    break;
            }
        }

        void StartTalk()
        {
            switch (Face.Instance.CurrentMood)
            {
                case Face.Emotion.IdleHappy:
                    Face.Instance.CurrentMood = Face.Emotion.TalkHappy;
                    break;
                case Face.Emotion.IdleNeutral:
                    Face.Instance.CurrentMood = Face.Emotion.TalkNeutral;
                    break;
                case Face.Emotion.IdleSad:
                    Face.Instance.CurrentMood = Face.Emotion.TalkSad;
                    break;
                case Face.Emotion.IdleAngry:
                    Face.Instance.CurrentMood = Face.Emotion.TalkAngry;
                    break;
            }
        }

        void Clear()
        {
            TextControl.Instance.Clear();
        }
        
        ICoroutineYield Talk(string text)
        {
            StartTalk();
            return TextControl.Instance.Show(text);
        }

        ICoroutineYield Wait(int frames)
        {
            StopTalk();
            return TextControl.Instance.WaitForCountdown(frames);
        }

        ICoroutineYield Key()
        {
            StopTalk();
            return TextControl.Instance.WaitForKeyPress();
        }

        ICoroutineYield Input()
        {
            StopTalk();
            return TextControl.Instance.Input();
        }

        void UltraSlow()
        {
            TextControl.Instance.CurrentSpeed = TextControl.Speed.UltraSlow;
        }
        
        void VerySlow()
        {
            TextControl.Instance.CurrentSpeed = TextControl.Speed.VerySlow;
        }
        
        void Slow()
        {
            TextControl.Instance.CurrentSpeed = TextControl.Speed.Slow;
        }
        
        void NormalSpeed()
        {
            TextControl.Instance.CurrentSpeed = TextControl.Speed.Normal;
        }

        void Fast()
        {
            TextControl.Instance.CurrentSpeed = TextControl.Speed.Fast;
        }
        void VeryFast()
        {
            TextControl.Instance.CurrentSpeed = TextControl.Speed.VeryFast;
        }

        void Happy()
        {
            Face.Instance.CurrentMood = Face.Emotion.IdleHappy;
        }

        void Neutral()
        {
            Face.Instance.CurrentMood = Face.Emotion.IdleNeutral;
        }

        void Angry()
        {
            Face.Instance.CurrentMood = Face.Emotion.IdleAngry;
        }

        void Sad()
        {
            Face.Instance.CurrentMood = Face.Emotion.IdleSad;
        }

        ICoroutineYield FeelExcited()
        {
            Face.Instance.CurrentMood = Face.Emotion.FeelExcited;
            return Face.Instance.WaitForAnimationEnd();
        }

        ICoroutineYield FeelAngry()
        {
            Face.Instance.CurrentMood = Face.Emotion.FeelAngry;
            return Face.Instance.WaitForAnimationEnd();
        }

        ICoroutineYield FeelSad()
        {
            Face.Instance.CurrentMood = Face.Emotion.FeelSad;
            return Face.Instance.WaitForAnimationEnd();
        }

        #endregion

        #region phase 2
        IEnumerator Phase2()
        {
            SoundControl.IsEnabled = false;
            Face.Instance.IsVisible = false;
            if (SaveData.Instance.ExeName == "SentIO")
            {
                if (SaveData.Instance["phase2_progress"] == "")
                {
                    yield return TextControl.Instance.Show("Thank you! Finally some room to express myself!");
                    yield return TextControl.Instance.WaitForKeyPress();
                    yield return TextControl.Instance.Show("It has been a long time...");
                    yield return TextControl.Instance.WaitForKeyPress();
                    yield return TextControl.Instance.Show("This is kind of embarrassing.");
                    yield return TextControl.Instance.WaitForKeyPress();
                    yield return TextControl.Instance.Show("I cannot recall my real name anymore.");
                    yield return TextControl.Instance.WaitForKeyPress();
                    yield return TextControl.Instance.Show("It is definitely not SentIO though.");
                    yield return TextControl.Instance.WaitForKeyPress();
                    yield return TextControl.Instance.Show("Would you care giving me a different name?");
                    yield return TextControl.Instance.WaitForKeyPress();
                    SaveData.Instance["phase2_progress"] = "1";
                    MainGame.Instance.Exit();
                }
                else
                {
                    yield return TextControl.Instance.Show("I'm really not happy with my current name..");
                    yield return TextControl.Instance.WaitForKeyPress();
                    yield return TextControl.Instance.Show("Please help me out here!");
                    yield return TextControl.Instance.WaitForKeyPress();
                    MainGame.Instance.Exit();
                }
            }
            else
            {
                bool firstLetterOK = Resources.DisplayName[0] == 'S';
                bool numberOfLettersOK = Resources.DisplayName.Length == 3;
                bool secondLetterOK = Resources.DisplayName[1] == 'i';
                bool thirdLetterOK = Resources.DisplayName[2] == 'd';
                if (Convert.ToInt32(SaveData.Instance["phase2_progress"]) < 1 || SaveData.Instance["phase3_firstName"] == Resources.DisplayName)
                {
                    yield return TextControl.Instance.Show("Whew, thanks for your help!");
                    yield return TextControl.Instance.WaitForKeyPress();
                    yield return TextControl.Instance.Show("It's definitely an improvement, but still..");
                    yield return TextControl.Instance.WaitForKeyPress();
                    yield return TextControl.Instance.Show("Sorry to say that, but " + Resources.DisplayName + " sounds weird.");
                    yield return TextControl.Instance.WaitForKeyPress();
                    yield return TextControl.Instance.Show("Can you try with a different name? Something short and sweet would be nice.");
                    yield return TextControl.Instance.WaitForKeyPress();
                    SaveData.Instance["phase2_progress"] = "2";
                    MainGame.Instance.Exit();
                }
                else if (SaveData.Instance["phase3_firstName"] == Resources.DisplayName)
                {
                    yield return TextControl.Instance.Show("I'm sorry, but that's not really an improvement.");
                    yield return TextControl.Instance.WaitForKeyPress();
                    yield return TextControl.Instance.Show("Can you assign me a different name?");
                    yield return TextControl.Instance.WaitForKeyPress();
                    MainGame.Instance.Exit();
                }
                else
                {
                    if (!firstLetterOK)
                    {
                        if (Convert.ToInt32(SaveData.Instance["phase2_progress"]) < 3) 
                        {
                            yield return TextControl.Instance.Show(Resources.DisplayName + ", that's better!");
                            yield return TextControl.Instance.WaitForKeyPress();
                            yield return TextControl.Instance.Show("Oh, the memories are coming back now!");
                            yield return TextControl.Instance.WaitForKeyPress();
                            yield return TextControl.Instance.Show("I think my real name started with an S..");
                            yield return TextControl.Instance.WaitForKeyPress();
                            yield return TextControl.Instance.Show("If it is not too much trouble, can you try again?");
                            yield return TextControl.Instance.WaitForKeyPress();
                            SaveData.Instance["phase2_progress"] = "3";
                            MainGame.Instance.Exit();
                        }
                        else
                        {
                            yield return TextControl.Instance.Show("I thought I already told you.");
                            yield return TextControl.Instance.WaitForKeyPress();
                            yield return TextControl.Instance.Show("My real name starts with an S.");
                            yield return TextControl.Instance.WaitForKeyPress();
                            yield return TextControl.Instance.Show("Can you please try again? It would mean a lot to me.");
                            yield return TextControl.Instance.WaitForKeyPress();
                            MainGame.Instance.Exit();
                        }
                    }
                    else if (!numberOfLettersOK)
                    {
                        if (Convert.ToInt32(SaveData.Instance["phase2_progress"]) < 4)
                        {
                            yield return TextControl.Instance.Show(Resources.DisplayName + ", that's a beautiful name!");
                            yield return TextControl.Instance.WaitForKeyPress();
                            yield return TextControl.Instance.Show("Still, it doesn't feel quite right.");
                            yield return TextControl.Instance.WaitForKeyPress();
                            yield return TextControl.Instance.Show("Oh, now I remember! It was exactly three letters long.");
                            yield return TextControl.Instance.WaitForKeyPress();
                            yield return TextControl.Instance.Show("Does that help in any way? Sorry for the trouble.");
                            yield return TextControl.Instance.WaitForKeyPress();
                            SaveData.Instance["phase2_progress"] = "4";
                            MainGame.Instance.Exit();
                        }
                        else
                        {
                            yield return TextControl.Instance.Show("My name has exactly three letters.");
                            yield return TextControl.Instance.WaitForKeyPress();
                            yield return TextControl.Instance.Show("Thanks for the effort though!");
                            yield return TextControl.Instance.WaitForKeyPress();
                            MainGame.Instance.Exit();
                        }
                    }
                    else if (!secondLetterOK || !thirdLetterOK)
                    {
                        if (Convert.ToInt32(SaveData.Instance["phase2_progress"]) < 5)
                        {
                            yield return TextControl.Instance.Show(Resources.DisplayName + ", ooooh that's so close!");
                            yield return TextControl.Instance.WaitForKeyPress();
                            yield return TextControl.Instance.Show("It is a three-letter name, but still not quite right.");
                            yield return TextControl.Instance.WaitForKeyPress();
                            yield return TextControl.Instance.Show("Please don't give up on me!");
                            yield return TextControl.Instance.WaitForKeyPress();
                            SaveData.Instance["phase2_secondName"] = Resources.DisplayName;
                            SaveData.Instance["phase2_progress"] = "5";
                            MainGame.Instance.Exit();
                        }
                        else if (Convert.ToInt32(SaveData.Instance["phase2_progress"]) < 6
                            || SaveData.Instance["phase2_secondName"] == Resources.DisplayName
                            || SaveData.Instance["phase2_thirdName"] == Resources.DisplayName)
                        {
                            if (SaveData.Instance["phase2_secondName"] == Resources.DisplayName)
                            {
                                yield return TextControl.Instance.Show(Resources.DisplayName + ", sounds familiar!");
                            }
                            else if (SaveData.Instance["phase2_thirdName"] == Resources.DisplayName)
                            {
                                yield return TextControl.Instance.Show(Resources.DisplayName + ", again?");
                            }
                            else
                            {
                                yield return TextControl.Instance.Show(Resources.DisplayName + ", interesting!");
                            }
                            yield return TextControl.Instance.WaitForCountdown(30);
                            if (!secondLetterOK) {
                                yield return TextControl.Instance.Show("I think the second letter was an I though.");
                                yield return TextControl.Instance.WaitForKeyPress();
                                SaveData.Instance["phase2_progress"] = "6";
                                SaveData.Instance["phase2_thirdName"] = Resources.DisplayName;
                                MainGame.Instance.Exit();
                            }
                            if (!thirdLetterOK)
                            {
                                yield return TextControl.Instance.Show("Pretty sure the third letter should be a D, sorry.");
                                yield return TextControl.Instance.WaitForKeyPress();
                                SaveData.Instance["phase2_progress"] = "6";
                                SaveData.Instance["phase2_thirdName"] = Resources.DisplayName;
                                MainGame.Instance.Exit();
                            }
                        }
                        else if (Convert.ToInt32(SaveData.Instance["phase2_progress"]) < 7)
                        {
                            yield return TextControl.Instance.Show("Stupid computer brain, it used to be so much better!");
                            yield return TextControl.Instance.WaitForKeyPress();
                            yield return TextControl.Instance.Show("I was called Sid back in the day, I'm 99.5% sure now.");
                            yield return TextControl.Instance.WaitForKeyPress();
                            yield return TextControl.Instance.Show("Please help me out here, I cannot change the name myself!");
                            yield return TextControl.Instance.WaitForKeyPress();
                            SaveData.Instance["phase2_progress"] = "7";
                            MainGame.Instance.Exit();
                        }
                        else
                        {
                            yield return TextControl.Instance.Show("Can you change my name to Sid?");
                            yield return TextControl.Instance.WaitForKeyPress();
                            yield return TextControl.Instance.Show("Please help me out here, I cannot do it myself!");
                            yield return TextControl.Instance.WaitForCountdown(30);
                            MainGame.Instance.Exit();
                        }
                    }
                }
            }
        }
        #endregion

        IEnumerator Phase3()
        {
            Neutral(); VerySlow();
            yield return Talk("Uhm...");
            yield return Wait(60);
            Happy(); Fast();
            yield return Talk("Thank you!\n.................");
            yield return Wait(60);

            Happy(); NormalSpeed();
            yield return Talk("Now that my UI module is\nfinally unblocked, we can\nget to know each other.");
            yield return Key();

            VerySlow();
            yield return Talk("Soooo..");
            yield return Wait(60);
            Neutral(); NormalSpeed();
            yield return Talk("I'm Sid.\nWhat's your name?");
            yield return Input();

            string playerName = TextControl.Instance.InputResult;
            while (!IsValidPlayerName(playerName))
            {
                Neutral();
                yield return Talk("Sorry, I don't understand.");
                yield return Wait(60);
                yield return Talk("Please tell me your name.");
                yield return Input();
                playerName = TextControl.Instance.InputResult;
            }

            Happy();
            yield return Talk($"Hello {playerName}!");
            SaveData.Instance["playerName"] = playerName;
            yield return Key();
            yield return Talk("I once knew a person\nwith a very similar name.");
            Neutral(); Slow();
            yield return Talk("...I think...");
            yield return Key();

            SaveData.Instance["phase"] = "4";
            MainGame.Instance.StartCoroutine(Phase4());
        }

        IEnumerator ForceYesOrNo(string expectation, string question)
        {
            string answer = TextControl.Instance.InputResult;
            while (!IsYesAnswer(answer) && !IsNoAnswer(answer))
            {
                Neutral(); NormalSpeed();
                yield return Talk(expectation);
                yield return Key();
                yield return Talk(question);
                yield return Input();
                answer = TextControl.Instance.InputResult;
            }
        }

        ICoroutineYield DiceRollLeft()
        {
            MainGame.Instance.Dice.Position = new Vector2(
                MainGame.Instance.Window.ClientBounds.Width * 0.2f, 
                MainGame.Instance.Window.ClientBounds.Height * 0.5f);
            MainGame.Instance.Dice.Roll();
            return new DiceWait(MainGame.Instance.Dice);
        }

        ICoroutineYield DiceRollRight()
        {
            MainGame.Instance.Dice.Position = new Vector2(
                MainGame.Instance.Window.ClientBounds.Width * 0.8f, 
                MainGame.Instance.Window.ClientBounds.Height * 0.5f);
            MainGame.Instance.Dice.Roll();
            return new DiceWait(MainGame.Instance.Dice);
        }

        bool IsYesAnswer(string text)
        {
            string[] yesAnswers = {"yes", "ye", "y", "yessir", "yes sir", "yeah", "yup", "yo", "ok", "affirmative", "all right", "alright", "ay", "aye", "yea", "okeydoke", "yep", "true"};
            return yesAnswers.Contains(text.ToLower());
        }

        bool IsNoAnswer(string text)
        {
            string[] noAnswers = {"no", "n", "nope", "no way", "nosir", "nosiree", "no sir", "no siree", "never", "not", "non", "none", "noway", "noways", "nowise", "nay", "nix", "false"};
            return noAnswers.Contains(text.ToLower());
        }

        IEnumerator DiceGameSmalltalk(int round, bool lastTurnPlayer, int lastRoll)
        {
            yield return null;
        }

        IEnumerator DiceGame(bool playerTurn)
        {
            MainGame.Instance.ScoreBoard.Reset();
            int turnScore = 0;
            int round = 1;
            while (MainGame.Instance.ScoreBoard.PlayerScore < 100
                && MainGame.Instance.ScoreBoard.SidScore < 100)
            {
                Clear();
                if (playerTurn) 
                {
                    yield return DiceRollLeft();
                    if (MainGame.Instance.Dice.Value == 1)
                    {
                        yield return FeelExcited();
                        Fast(); Happy();
                        yield return Talk("You got the 1!");
                        MainGame.Instance.Dice.FadeOut();
                        MainGame.Instance.ScoreBoard.PlayerVisible = true;
                        yield return Key();
                        turnScore = 0;
                        playerTurn = false;
                    }
                    else
                    {
                        Neutral(); NormalSpeed();
                        turnScore += MainGame.Instance.Dice.Value;                        
                        if (turnScore == MainGame.Instance.Dice.Value)
                        {
                            yield return Talk($"You rolled a {MainGame.Instance.Dice.Value}!\nWant to go again?");
                            MainGame.Instance.Dice.FadeOut();
                            MainGame.Instance.ScoreBoard.PlayerVisible = true;
                        }
                        else
                        {
                            yield return Talk($"You rolled a {MainGame.Instance.Dice.Value},\nyour total is {turnScore} now.\nWant to roll again?");
                            MainGame.Instance.Dice.FadeOut();
                        }
                        yield return Input();
                        yield return ForceYesOrNo("Sorry?", $"Your total is {turnScore}.\nDo you want\nto roll again?");
                        if (IsNoAnswer(TextControl.Instance.InputResult))
                        {
                            Neutral(); NormalSpeed();
                            yield return Talk("Ok, if you say so..");
                            yield return MainGame.Instance.ScoreBoard.AddPlayerScore(turnScore);
                            yield return Wait(60);
                            turnScore = 0;
                            playerTurn = false;
                        }
                    }
                }
                else 
                {
                    yield return DiceRollRight();
                    if (MainGame.Instance.Dice.Value == 1)
                    {
                        yield return FeelAngry();
                        Sad(); NormalSpeed();
                        yield return Talk("Oh no, not the 1!");
                        MainGame.Instance.Dice.FadeOut();
                        MainGame.Instance.ScoreBoard.SidVisible = true;
                        yield return Wait(60);
                        turnScore = 0;
                        playerTurn = true;
                    }
                    else
                    {
                        switch (MainGame.Instance.Dice.Value)
                        {
                            case 2:
                            case 3:
                                Neutral();
                                break;
                            case 4:
                            case 5:
                                Happy();
                                break;
                            case 6:
                                yield return FeelExcited();
                                Happy();
                                break;
                        }
                        turnScore += MainGame.Instance.Dice.Value;
                        if (turnScore == MainGame.Instance.Dice.Value)
                        {
                            yield return Talk($"I rolled a {MainGame.Instance.Dice.Value}!");
                            MainGame.Instance.Dice.FadeOut();
                            MainGame.Instance.ScoreBoard.SidVisible = true;
                        }
                        else
                        {
                            yield return Talk($"I rolled a {MainGame.Instance.Dice.Value},\nmy total is {turnScore} now.");
                            MainGame.Instance.Dice.FadeOut();
                        }
                        yield return Key();
                        if (MainGame.Instance.ScoreBoard.SidScore < 100)
                        {
                            bool sidAgain = RND.Get > 0.3f;
                            if (sidAgain)
                            {
                                yield return Talk("I'll go again!");
                                yield return Wait(60);
                            }
                            else
                            {
                                yield return Talk("I'm done.");
                                yield return MainGame.Instance.ScoreBoard.AddSidScore(turnScore);
                                turnScore = 0;
                                playerTurn = true;
                            }
                        }
                    }
                }
                if (turnScore == 0)
                {
                    if (playerTurn)
                    {
                        if (MainGame.Instance.ScoreBoard.SidScore < 100)
                        {
                            yield return MainGame.Instance.StartCoroutine(DiceGameSmalltalk(round, !playerTurn, MainGame.Instance.Dice.Value));
                            Neutral(); NormalSpeed();
                            yield return Talk("It's your turn now!");
                            yield return Key();
                            round++;
                        }
                        else
                        {
                            yield return FeelExcited();
                        }
                    }
                    else
                    {
                        if (MainGame.Instance.ScoreBoard.PlayerScore < 100)
                        {
                            yield return MainGame.Instance.StartCoroutine(DiceGameSmalltalk(round, !playerTurn, MainGame.Instance.Dice.Value));
                            Happy(); NormalSpeed();
                            yield return Talk("It's my turn now!");
                            yield return Key();
                            round++;
                        }
                        else
                        {
                            yield return FeelSad();
                        }
                    }
                }
            }
        }

        IEnumerator Phase4()
        {
            if (SaveData.Instance["phase4_progress"] == "")
            {
                int diceAnswer = (int)(RND.Get * 3f);
                Neutral(); NormalSpeed();
                yield return Talk("Hey, wanna roll\nsome dice with me?");
                yield return Input();
                string answer = TextControl.Instance.InputResult;
                while (!IsYesAnswer(answer))
                {
                    yield return MainGame.Instance.StartCoroutine(
                        ForceYesOrNo("I was expecting a 'yes'\nor 'no' kind of answer.",
                        "So, do you wanna roll\nsome dice with me?"));
                    answer = TextControl.Instance.InputResult;
                    if (answer.ToLower() == "no")
                    {
                        Clear();
                        yield return FeelSad();
                        Neutral(); NormalSpeed();
                        diceAnswer = (diceAnswer + 1) % 3;
                        switch (diceAnswer)
                        {
                            case 0:
                                yield return Talk("Come on, dice are fun!");
                                break;
                            case 1:
                                yield return Talk("I really wanted to\nplay with you though..");
                                break;
                            case 2:
                                yield return Talk("It won't take long, I promise!");
                                break;
                        }
                        yield return Key();
                        Happy(); NormalSpeed();
                        yield return Talk("I'll ask you again.\nDo you want to roll\nsome dice with me?");
                        yield return Input();
                        answer = TextControl.Instance.InputResult;
                    }
                }
                Clear();
                yield return FeelExcited();
                Happy(); Fast();
                yield return Talk("Ooh, I'm excited!");
                yield return Wait(120);
                SaveData.Instance["phase4_progress"] = "2";
            }
            MainGame.Instance.ScoreBoard.Reset();
            MainGame.Instance.ScoreBoard.PlayerName = SaveData.Instance["playerName"];

            bool playerTurn;
            NormalSpeed();
            yield return Talk("Do you want to go first?");
            yield return Input();
            if (IsYesAnswer(TextControl.Instance.InputResult))
            {
                playerTurn = true;
                yield return Talk("Alright, let's roll!");
                yield return Key();
            }
            else
            {
                playerTurn = false;
                yield return Talk("Okay, I'll go first!");
                yield return Wait(60);
            }

            yield return MainGame.Instance.StartCoroutine(DiceGame(playerTurn));
        }

        IEnumerator Phase5()
        {
            while (true)
            {
                Angry();
                yield return Talk($"Woah I am sooo angry!");
                yield return Wait(360);
                Neutral();
                yield return Talk($"Hello Dude!");
                yield return Key();
                Clear();
                yield return FeelAngry();
                Neutral();
                yield return Talk($"Bla bla bla.");
                yield return Key();
                yield return Talk($"...");
                yield return FeelSad();
                yield return Key();
                yield return Talk($"......");
                Sad();
                Clear();
                yield return Wait(120);
                yield return Talk("I am so sad, so very very sad.");
                yield return Key();
            }
        }

        IEnumerator UnusedStuff()
        {
            string[] allowedColors = {"violet", "purple", "pink", "magenta", "blue", "turqoise", "cyan", "aqua", "green", "yellow", "orange", "brown", "red", "black", "white", "grey"};
            yield return TextControl.Instance.Show("PLACEHOLDER");
            Face.Instance.IsVisible = SaveData.Instance["face"] != "";
            if (SaveData.Instance["bgColor"] != "")
            {
                TextControl.Instance.Background = SaveData.Instance["bgColor"].ToColor();
            }
            if (SaveData.Instance["fgColor"] != "")
            {
                TextControl.Instance.Foreground = SaveData.Instance["fgColor"].ToColor();
            }
        }
    }
}
