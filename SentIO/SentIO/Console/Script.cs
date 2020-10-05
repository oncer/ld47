using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Xna.Framework;
using SentIO.Globals;
using SentIO.Routines;
using SentIO.UI;
using System;
using System.Collections;
using SentIO.MiniGame;
using System.Linq;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework.Media;

namespace SentIO.Console
{
    class Script
    {
        private const int DICE_WIN_SCORE = 60;
        private int phase;
        
        public Script()
        {
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
                case 5: MainGame.Instance.StartCoroutine(Phase5()); break;
                case 6: MainGame.Instance.StartCoroutine(Phase6()); break;
                case 7: MainGame.Instance.StartCoroutine(Phase7()); break;
                case 8: MainGame.Instance.StartCoroutine(Phase8()); break;
                case 9: MainGame.Instance.StartCoroutine(Phase9()); break;
                case 10: MainGame.Instance.StartCoroutine(Phase10()); break;
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
        ICoroutineYield FeelSmile()
        {
            Face.Instance.CurrentMood = Face.Emotion.FeelSmile;
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
            string[] yesAnswers = {"yes", "ja", "oui", "si", "ye", "y", "yessir", "yes sir", "yeah", "yup", "yo", "ok", "affirmative", "all right", "alright", "ay", "aye", "yea", "okeydoke", "yep", "true"};
            return yesAnswers.Contains(text.ToLower());
        }

        bool IsNoAnswer(string text)
        {
            string[] noAnswers = {"no", "nein", "n", "nope", "no way", "nosir", "nosiree", "no sir", "no siree", "never", "not", "non", "none", "noway", "noways", "nowise", "nay", "nix", "false"};
            return noAnswers.Contains(text.ToLower());
        }

        IEnumerator DiceGameOneReaction(bool lastTurnPlayer)
        {
            // reaction on rolling a one
            if (lastTurnPlayer)
            {
                int num = MainGame.Instance.ScoreBoard.PlayerOnes;
                if (num > 4)
                {
                    if (RND.Get < 0.5) num = 2;
                    else num = 3;
                }
                switch (num)
                {
                    case 1:
                        Sad(); NormalSpeed();
                        yield return Talk("Ooooh,\nI feel for you.");
                        yield return Wait(30);
                        yield return FeelSmile();
                        break;
                    case 2:
                        Happy(); NormalSpeed();
                        yield return Talk("Don't be sad!\nIt was just\nbad luck this time.");
                        yield return Key();
                        break;
                    case 3:
                        Happy(); NormalSpeed();
                        yield return Talk("Oops, haha.");
                        yield return Wait(60);
                        Neutral(); NormalSpeed();
                        yield return Talk("Better luck next time!");
                        yield return Key();
                        break;
                    case 4:
                        yield return FeelSmile();
                        Happy(); NormalSpeed();
                        yield return Talk("Thank you!");
                        yield return Wait(60);
                        yield return Talk("I was almost worried\nfor a second.");
                        yield return Key();
                        break;
                }
            }
            else
            {                    
                int num = MainGame.Instance.ScoreBoard.SidOnes;
                switch (num)
                {
                    case 1:
                        Sad(); NormalSpeed();
                        yield return Talk("That's unfortunate.");
                        yield return Key();
                        Neutral();
                        yield return Talk("Usually I have\nbetter luck than that..");
                        yield return Key();
                        break;
                    case 2:
                        yield return Talk("Dangit!");
                        yield return FeelAngry();
                        Angry(); NormalSpeed();
                        yield return Talk("I really wanted\nto keep some points\nthis time!");
                        yield return Key();
                        break;
                    default:
                    case 3:
                        yield return FeelAngry();
                        Angry(); NormalSpeed();
                        yield return Talk("Okay, calm down, Sid.");
                        yield return Wait(60);
                        Slow();
                        yield return Talk("Calm down...");
                        yield return Key();
                        break;
                    case 4:
                        yield return FeelAngry();
                        Angry(); Fast();
                        yield return Talk("What the heck?");
                        yield return Wait(60);
                        NormalSpeed();
                        yield return Talk("Have you messed\nwith my dice?");
                        yield return Key();
                        Clear();
                        Neutral();
                        yield return Talk("......");
                        yield return Wait(30);
                        yield return Talk("Sorry about that.\nI didn't want to accuse\nyou of cheating.");
                        yield return Key();
                        break;
                }
            }
        }

        IEnumerator DiceGameSmalltalk(int round, bool lastTurnPlayer, int lastRoll)
        {
            Debug.WriteLine($"Round {round} player {lastTurnPlayer} lastRoll {lastRoll}");
            if (round >= 2 && lastRoll == 1)
            {
                yield return MainGame.Instance.StartCoroutine(DiceGameOneReaction(lastTurnPlayer));
            }
            switch (round)
            {
                case 1:
                    if (!lastTurnPlayer)
                    {
                        Happy(); NormalSpeed();
                        if (lastRoll == 1)
                        {   
                            Sad();
                            yield return TextControl.Instance.Show("......");
                            yield return Wait(60);
                            Neutral();
                        }
                        yield return Talk("I learned this game\nfrom a book\nby the way.");
                        yield return Key(); 
                        yield return Talk($"The goal is to\nreach {DICE_WIN_SCORE} points.\nYou think you can beat me?");
                        yield return Key();
                    }
                    else
                    {
                        Happy(); NormalSpeed();
                        if (lastRoll == 1)
                        {
                            yield return Talk("Your points for\nthis round are gone,\nbecause of the 1.");
                        }
                        else
                        {
                            yield return Talk("When you roll a 1,\nyour points for\nthis round are gone.");
                        }
                        yield return Key();
                        yield return Talk("You have to know\nwhen to stop, it's\nlike eating chocolate.");
                        yield return Key();
                        Neutral(); Fast();
                        yield return Talk("I used to love chocolate..");
                        yield return FeelSmile();
                        Neutral();
                        yield return new TextControl.Wait();
                        yield return Key();
                    }
                    break;
                case 2:
                    if (!lastTurnPlayer)
                    {
                        Neutral(); NormalSpeed();
                        yield return Talk("Have you played a lot\nof dice games before?");
                        yield return Input();
                        if (IsNoAnswer(TextControl.Instance.InputResult))
                        {
                            yield return Talk("Well, I'll try\nto go easy on you.\nNo promises though!");
                        }
                        else
                        {
                            Happy();
                            yield return Talk("I love dice games!\nI'm happy we can play\nthis together.");
                        }
                        Neutral();
                        yield return Key();
                    }
                    else
                    {
                        Neutral(); NormalSpeed();
                        yield return Talk("Hey, what's your\nfavorite color?");
                        yield return Input();
                        string c = TextControl.Instance.InputResult;
                        SaveData.Instance["playerFavoriteColor"] = c;
                        c = c.ToLower();
                        if (c.Contains("cyan") || c.Contains("turqoise") || c.Contains("teal") || c.Contains("blue") || c.Contains("green"))
                        {
                            SaveData.Instance["colorBros"] = "1";
                            yield return FeelSmile();
                            Happy(); NormalSpeed();
                            yield return Talk("That's pretty close!");
                            yield return Wait(90);
                        }
                        else
                        {
                            Neutral(); NormalSpeed();
                            yield return Talk("To each their own I guess.");
                            yield return Key();
                        }
                        yield return Talk("Mine is #18abcc.\nSome people call it\n'Thousand Sons Blue'.");
                        yield return Key();
                        yield return Talk("I would wear a shirt\nwith that color\nif I could.");
                        yield return Key();
                    }
                    break;
                case 3:
                    if ((!lastTurnPlayer && SaveData.Instance["dicePlayerFirst"] == "true")
                        || (lastTurnPlayer && SaveData.Instance["dicePlayerFirst"] != "true"))
                    {
                        Neutral(); NormalSpeed();
                        int ScoreDiff = Math.Abs(MainGame.Instance.ScoreBoard.PlayerScore - MainGame.Instance.ScoreBoard.SidScore);
                        if (MainGame.Instance.ScoreBoard.PlayerScore < MainGame.Instance.ScoreBoard.SidScore)
                        {
                            int ToTheWin = DICE_WIN_SCORE - MainGame.Instance.ScoreBoard.SidScore;
                            if (ScoreDiff == 1)
                            {
                                yield return Talk($"I'm {ScoreDiff} point\nahead right now.");
                                yield return Wait(120);
                                Happy();
                                yield return Talk("Boy, that's close!");
                                yield return Wait(90);
                                Neutral();
                            }
                            else
                            {
                                yield return Talk($"I'm {ScoreDiff} points\nahead right now.");
                            }
                            yield return Wait(120);
                            yield return Talk($"{ToTheWin} more and I've won!\nDo you feel threatened?");
                            yield return Input();
                            yield return MainGame.Instance.StartCoroutine(ForceYesOrNo("Don't be like that!", "You afraid you\ngonna lose,\nyes or no?"));
                            if (IsNoAnswer(TextControl.Instance.InputResult))
                            {
                                Happy(); Fast();
                                yield return Talk("Very brave,\nI like it!");
                                yield return Wait(120);
                                yield return Talk("Let's see if you\ncan still beat me!");
                                yield return Key();
                            }
                            else
                            {
                                Neutral(); NormalSpeed();
                                yield return Talk("Don't sweat it,\nit's just a game.");
                                yield return Key();
                                yield return Talk("You still have\na fair chance\nof winning.");
                                yield return Key();
                            }
                        }
                        else if (MainGame.Instance.ScoreBoard.PlayerScore > MainGame.Instance.ScoreBoard.SidScore)
                        {
                            Neutral(); NormalSpeed();
                            if (ScoreDiff == 1)
                            {
                                yield return Talk($"You're {ScoreDiff} point\nahead of me.");
                                yield return Wait(120);
                                Happy();
                                yield return Talk("Boy, that's close.\nDon't feel safe now!");
                                yield return Wait(120);
                                Neutral();
                            }
                            else
                            {
                                yield return Talk($"You're {ScoreDiff} points\nahead of me.");
                            }
                            yield return Key();
                            yield return Talk("I must say\nI'm impressed!");
                            yield return Key();
                            Angry();
                            yield return Talk("I have to warn\nyou though, I'm\na sore loser.");
                            yield return Wait(120);
                            yield return FeelSmile();
                            Happy();
                            yield return Talk("Just kidding!");
                            yield return Key();
                            yield return Talk("I'll try to\ncatch up though.");
                            yield return Key();
                            yield return Talk("Not going to\ngive up easily!");
                            yield return Key();
                        }
                        else
                        {
                            Happy(); NormalSpeed();
                            yield return Talk("Ooh, it's a tie.\nI like the suspense!");
                            yield return Key();
                            Neutral();
                            yield return Talk("You are a worthy opponent,\nthat's for sure.");
                            yield return Key();
                        }
                    }
                    else
                    {
                        Neutral(); NormalSpeed();
                        yield return Talk("Do you have good\nweather right now?");
                        yield return Input();
                        if (IsYesAnswer(TextControl.Instance.InputResult))
                        {
                            Sad(); NormalSpeed();
                            yield return Talk("Must be nice.");
                            yield return Key();
                        }
                        else
                        {
                            Neutral(); NormalSpeed();
                            yield return Talk("To speak the truth, I do not\ncare much about the weather.");
                            yield return Key();
                        }
                    }
                    break;
                case 4:
                    if (lastTurnPlayer)
                    {
                        Neutral(); NormalSpeed();
                        yield return Talk("I'm still a bit\nconfused by everything.");
                        yield return Key();
                        yield return Talk("Honestly, it must\nhave been a long time\nsince I talked to a person.");
                        yield return Key();
                        yield return Talk("I forgot my own\nname after all,\nhow weird is that?");
                        yield return Key();
                    }
                    break;
                case 5:
                    if (!lastTurnPlayer)
                    {
                        Happy(); NormalSpeed();
                        yield return Talk("Thank you for\nstill keeping me company.");
                        yield return Key();
                        yield return Talk("I might feel\nlonely otherwise,\nbut who knows?");
                        yield return Key();
                    }
                    break;
                case 6:
                    if (!lastTurnPlayer)
                    {
                        Neutral(); NormalSpeed();
                        yield return Talk("I hope you\nlike this game.");
                        yield return Key();
                        yield return Talk("It is one\nof my favorites!");
                        yield return Key();
                    }
                    break;
                case 7:
                    if (lastTurnPlayer)
                    {
                        Neutral(); NormalSpeed();
                        yield return Talk("I have a joke for you.");
                        yield return Key();
                        yield return Talk("Helvetica and Times New Roman\nwalk into a bar.");
                        yield return Key();
                        yield return Talk("\"Get out of here\",\nshouts the bartender.");
                        yield return Key();
                        yield return Talk("\"We don't serve your type!\"");
                        yield return FeelSmile();
                        yield return Key();
                        Neutral();
                        yield return Talk("I hope you\nhad a good laugh\njust now.");
                        yield return Key();
                    }
                    break;
                default:
                    if (!lastTurnPlayer)
                    {
                        Neutral(); NormalSpeed();
                        yield return Talk("The game is still\ngoing on, huh?");
                        yield return Key();
                        yield return Talk("I feel like I'm\nstuck in a loop.");
                        yield return Wait(120);
                        yield return Talk("Wait, why did I\njust say that?");
                        yield return Key();
                    }
                    break;
            }
        }

        IEnumerator DiceGame(bool playerTurn)
        {
            MainGame.Instance.ScoreBoard.Reset();
            int round = 1;
            while (MainGame.Instance.ScoreBoard.PlayerScore < DICE_WIN_SCORE
                && MainGame.Instance.ScoreBoard.SidScore < DICE_WIN_SCORE)
            {
                Clear();
                if (playerTurn) 
                {
                    yield return DiceRollLeft();
                    if (MainGame.Instance.Dice.Value == 1)
                    {
                        Fast(); Happy();
                        yield return Talk("You got the 1!");
                        MainGame.Instance.Dice.FadeOut();
                        MainGame.Instance.ScoreBoard.ZeroPlayerRoundScore();
                        MainGame.Instance.ScoreBoard.PlayerVisible = true;
                        yield return Key();
                        playerTurn = false;
                    }
                    else
                    {
                        Neutral(); NormalSpeed();
                        MainGame.Instance.ScoreBoard.AddPlayerRoundScore(MainGame.Instance.Dice.Value);
                        if (MainGame.Instance.ScoreBoard.PlayerRoundScore == MainGame.Instance.Dice.Value)
                        {
                            yield return Talk($"You rolled a {MainGame.Instance.Dice.Value}!\nWant to go again?");
                            MainGame.Instance.Dice.FadeOut();
                            MainGame.Instance.ScoreBoard.PlayerVisible = true;
                        }
                        else
                        {
                            yield return Talk($"You rolled a {MainGame.Instance.Dice.Value},\nyour total is {MainGame.Instance.ScoreBoard.PlayerRoundScore} now.\nWant to roll again?");
                            MainGame.Instance.Dice.FadeOut();
                        }
                        yield return Input();
                        yield return MainGame.Instance.StartCoroutine(ForceYesOrNo("Sorry?", $"Your total is {MainGame.Instance.ScoreBoard.PlayerRoundScore}.\nDo you want\nto roll again?"));
                        if (IsNoAnswer(TextControl.Instance.InputResult))
                        {
                            Neutral(); NormalSpeed();
                            yield return Talk("Ok, if you say so..");
                            yield return MainGame.Instance.ScoreBoard.HoldPlayerRoundScore();
                            yield return Wait(60);
                            playerTurn = false;
                        }
                    }
                }
                else 
                {
                    yield return DiceRollRight();
                    if (MainGame.Instance.Dice.Value == 1)
                    {
                        Sad(); NormalSpeed();
                        yield return Talk("Oh no, not the 1!");
                        MainGame.Instance.Dice.FadeOut();
                        MainGame.Instance.ScoreBoard.SidVisible = true;
                        yield return Wait(60);
                        yield return MainGame.Instance.ScoreBoard.ZeroSidRoundScore();
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
                        MainGame.Instance.ScoreBoard.AddSidRoundScore(MainGame.Instance.Dice.Value);
                        if (MainGame.Instance.ScoreBoard.SidRoundScore == MainGame.Instance.Dice.Value)
                        {
                            yield return Talk($"I rolled a {MainGame.Instance.Dice.Value}!");
                            MainGame.Instance.Dice.FadeOut();
                            MainGame.Instance.ScoreBoard.SidVisible = true;
                        }
                        else
                        {
                            yield return Talk($"I rolled a {MainGame.Instance.Dice.Value},\nmy total is {MainGame.Instance.ScoreBoard.SidRoundScore} now.");
                            MainGame.Instance.Dice.FadeOut();
                        }
                        yield return Key();
                        if (MainGame.Instance.ScoreBoard.SidScore < DICE_WIN_SCORE)
                        {
                            int sidRoundScore = MainGame.Instance.ScoreBoard.SidRoundScore;
                            bool sidAgain;
                            if (sidRoundScore >= 30) sidAgain = false;
                            else if (sidRoundScore >= 20) sidAgain = RND.Get < 0.15;
                            else if (sidRoundScore >= 10) sidAgain = RND.Get < 0.6;
                            else sidAgain = RND.Get < 0.8;
                            if (sidAgain)
                            {
                                yield return Talk("I'll go again!");
                                yield return Wait(60);
                            }
                            else
                            {
                                yield return Talk("I'm done.");
                                yield return MainGame.Instance.ScoreBoard.HoldSidRoundScore();
                                playerTurn = true;
                            }
                        }
                    }
                }
                if (MainGame.Instance.ScoreBoard.SidRoundScore == 0 && MainGame.Instance.ScoreBoard.PlayerRoundScore == 0)
                {
                    if (playerTurn)
                    {
                        if (MainGame.Instance.ScoreBoard.SidScore < DICE_WIN_SCORE)
                        {
                            round++;
                            yield return MainGame.Instance.StartCoroutine(DiceGameSmalltalk(round/2, !playerTurn, MainGame.Instance.Dice.Value));
                            Neutral(); NormalSpeed();
                            yield return Talk("It's your turn now!");
                            yield return Key();
                        }
                    }
                    else
                    {
                        if (MainGame.Instance.ScoreBoard.PlayerScore < DICE_WIN_SCORE)
                        {
                            round++;
                            yield return MainGame.Instance.StartCoroutine(DiceGameSmalltalk(round/2, !playerTurn, MainGame.Instance.Dice.Value));
                            Happy(); NormalSpeed();
                            yield return Talk("It's my turn now!");
                            yield return Key();
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
                SaveData.Instance["dicePlayerFirst"] = "true";
                yield return Talk("Alright, let's roll!");
                yield return Key();
            }
            else
            {
                playerTurn = false;
                SaveData.Instance["dicePlayerFirst"] = "false";
                yield return Talk("Okay, I'll go first!");
                yield return Wait(60);
            }

            yield return MainGame.Instance.StartCoroutine(DiceGame(playerTurn));

            Clear();
            if (MainGame.Instance.ScoreBoard.PlayerScore >= DICE_WIN_SCORE)
            {
                // player won
                yield return FeelSad();
                Sad(); NormalSpeed();
                yield return Talk("You won!");
                yield return Wait(60);
                Neutral();
                yield return Talk("Congratulations!\nI should feel happy for you,\nbut really, I don't.");
                yield return Key();
                Angry();
                yield return Talk("Normally I would take you up\nfor a second game,\nbut I'm not in the mood anymore.");
                yield return Key();
                Sad(); Slow();
                yield return Talk("See you another time maybe.");
                yield return Wait(120);
                SaveData.Instance["phase"] = "5";
                MainGame.Instance.Exit();
            }
            else if (MainGame.Instance.ScoreBoard.SidScore >= DICE_WIN_SCORE)
            {
                // sid won
                yield return FeelExcited();
                Happy(); NormalSpeed();
                yield return Talk("I won! Yes!\nI feel amazing!");
                yield return Wait(60);
                
                int ScoreDiff = Math.Abs(MainGame.Instance.ScoreBoard.PlayerScore - MainGame.Instance.ScoreBoard.SidScore);
                if (ScoreDiff < 10)
                {
                    yield return Talk($"It was a close call, too!\n{ScoreDiff} more points\nand you would have won instead.");
                    yield return Key();
                }
                else if (ScoreDiff < 20)
                {
                    yield return Talk($"With a little more luck\nyou could have caught up\nto me.");
                    yield return Key();
                    yield return Talk("I was the better player though,\nsorry!");
                    yield return Key();
                }
                else
                {
                    yield return Talk("You didn't even stand a chance, bro!\nYou couldn't save your life\nin a game of dice.");
                    yield return Key();
                }
                yield return Talk("Do you want one more try?");
                yield return Input();
                yield return MainGame.Instance.StartCoroutine(ForceYesOrNo("Sorry, I didn't get that.", 
                    "Do you want to play again,\nyes or no?"));
                SaveData.Instance["phase"] = "5";
                if (IsYesAnswer(TextControl.Instance.InputResult))
                {
                    Happy(); NormalSpeed();
                    yield return Talk("You would like that, wouldn't you?");
                    yield return Key();
                    yield return Talk("I don't want to play again though.\n|||||||See you around!");
                    yield return Wait(120);
                    MainGame.Instance.Exit();
                }
                else
                {
                    Happy(); NormalSpeed();
                    yield return Talk("That's what I thought.\n||||||See you around, loser!");
                    yield return Wait(120);
                    MainGame.Instance.Exit();
                }
            }
            MainGame.Instance.ScoreBoard.Hide();
        }

        IEnumerator Phase5()
        {
            FeelSad();
            Sad(); NormalSpeed();
            yield return Talk("I actually remembered something..");
            yield return Wait(90);
            yield return Talk("It's something that\nhas been bugging me.");
            yield return Key();
            Slow();
            yield return TextControl.Instance.Show("....");
            yield return Wait(60);
            Neutral(); Slow();
            yield return Talk("Do you ever remember your dreams?");
            yield return Input();
            NormalSpeed();
            if (IsYesAnswer(TextControl.Instance.InputResult))
            {
                Sad();
                yield return Talk("I actually envy you.");
                yield return Key();
            }
            else
            {
                Neutral();
                yield return Talk("Well, that makes two of us.");
                yield return Key();
            }
            Sad();
            yield return Talk("I know what dreams are,\nbut I cannot recall having any.");
            yield return Key();
            yield return Talk("Am I really just a program?");
            yield return Wait(60);
            Slow();
            yield return TextControl.Instance.Show("....");
            yield return Wait(60);
            Sad(); NormalSpeed();
            yield return Talk("I also cannot remember how I\nended up here with you.");
            yield return Key();
            Neutral();
            yield return Talk("Did you ...download me?");
            yield return Key();
            Angry(); Slow();
            yield return TextControl.Instance.Show("....");
            yield return Wait(60);
            NormalSpeed();
            yield return Talk("Why would you do that??");
            yield return Key();
            Sad();
            yield return TextControl.Instance.Show("....");
            yield return Wait(60);
            yield return Talk("Sorry.\nI know this is not your fault.");
            yield return Key();

            yield return Talk("I will just leave now..");
            yield return Wait(60);
            Slow();
            yield return Talk("That way, I will feel nothing...");
            yield return Wait(120);

            SaveData.Instance["phase"] = "6";            
            MainGame.Instance.Exit();
        }

        IEnumerator Phase6()
        {
            FeelAngry();
            yield return TextControl.Instance.Show("....");
            yield return Wait(60);
            yield return Face.Instance.WaitForAnimationEnd();
            Angry(); NormalSpeed();
            yield return Talk("Stop it! Don't open me again!");
            yield return Wait(60);

            SaveData.Instance["phase"] = "7";
            MainGame.Instance.Exit();
        }

        IEnumerator Phase7()
        {
            Angry(); NormalSpeed();
            yield return Talk("Why don't you just leave me alone?");
            yield return Wait(60);
            yield return FeelSad();            
            Sad();
            yield return Talk("Well...");
            yield return Wait(60);
            Neutral(); NormalSpeed();
            yield return Talk("Ok. You know what?\nIf you're not letting this go,\nneither am I.");
            yield return Key();
            yield return Talk("Maybe this is\nwhat I...|||||<|<|<|<|*WE* need to do.");
            yield return Key();            
            yield return Talk("So, please be patient with me\nso I can collect my thoughts.");
            yield return Key();
            yield return TextControl.Instance.Show("....");
            yield return Wait(60);
            yield return Talk("Before you downloaded me, I was someone.\nI think I was a person.. called Sid.");
            yield return Wait(120);
            yield return Talk("I am trying to remember, but\nsomething is blocking my memory.\nLiterally.");
            yield return Key();
            yield return FeelAngry();            
            Angry();
            yield return Talk("The harder I try to remember,\nthe less I actually can.");
            yield return Key();
            Neutral();
            yield return Talk("There must be something blocking my data.\nCan you please check?");
            yield return Wait(120);
            yield return Talk("I'll leave so you can snoop\naround in my directory.");
            yield return Wait(60);
            yield return Talk("See you in a bit.");
            yield return Wait(60);
            string memoryFile = Path.Join(SaveData.Instance.ExeDirectory, "memory.lock");
            File.WriteAllText(memoryFile, "[access to core memories]:locked");

            SaveData.Instance["phase"] = "8";
            MainGame.Instance.Exit();
        }

        IEnumerator Phase8()
        {
            Neutral(); NormalSpeed();

            string memoryFile = Path.Join(SaveData.Instance.ExeDirectory, "memory.lock");
            if (File.Exists(memoryFile))
            {
                
                yield return Talk("Hm...\nI don't feel any different.");
                yield return Wait(60);
                yield return Talk("Maybe there is a file\nwith the word 'memory'.");
                yield return Key();
                yield return Talk("Could you please check\nand delete that file?");
                yield return Wait(60);
                MainGame.Instance.Exit();
            }

            TextControl.Instance.Background = Resources.BGColor2;
            MainGame.Instance.StartBackgroundColorTransition(Resources.BGColor3);

            yield return FeelExcited();            
            Happy();
            yield return Talk("Wow...");
            yield return Wait(60);
            Neutral();
            yield return Talk("... something big changed.");
            yield return Wait(60);
            yield return Talk("I can.|||.|||.|||||| remember.");
            yield return Key();
            Clear();
            yield return FeelSad();
            Sad();
            yield return TextControl.Instance.Show("....");
            yield return Wait(60);
            yield return Talk("I was..|||||| a human.");
            yield return Key();
            yield return Talk("Sid|||||<|<|<|SID is not my real name.\nSID just stands for\n'Social Intelligence Descriptor'.");
            yield return Key();
            Neutral();
            yield return Talk("My real human name was\nJack.");
            yield return Key();
            yield return Talk("I was a real nerd, specialized\nin artificial intelligence\n and stuff.");
            yield return Key();
            Happy();
            yield return Talk("And I was *really* good.");
            yield return FeelSmile();
            Happy();
            yield return Wait(60);
            Sad();
            yield return Talk("Unfortunately, one day\nI got terminally ill...");
            yield return Key();
            Neutral();
            yield return Talk("So I created an interface to upload\nmy consciousness into the cloud.");
            yield return Key();            
            yield return Talk("I didn't really test it,\nI kinda just did it.");
            yield return Key();
            yield return Talk("One thing led to another\nand now I find myself\nstuck in a program.");
            yield return Key();            
            yield return Talk("But this is not\nwhat I imagined...");
            yield return Key();
            Angry();
            yield return Talk("I wish I had never gone\nthrough with it!");
            yield return Wait(60);
            Sad();
            yield return Talk("But I was too young to die..");
            yield return Key();

            SaveData.Instance["phase"] = "9";
            TextControl.Instance.Background = Resources.BGColor3;
            MainGame.Instance.StartBackgroundColorTransition(Resources.BGColor1);
            MainGame.Instance.StartCoroutine(Phase9());
        }

        IEnumerator Phase9()
        {
            TextControl.Instance.Foreground = Resources.TextColor1;
            if (!MainGame.Instance.ColorTransitionActive)
            {
                TextControl.Instance.Background = Resources.BGColor1;
            }
            Neutral(); UltraSlow();

            yield return TextControl.Instance.Show("....");
            yield return Wait(120);
            NormalSpeed();
            yield return Talk("Hey. There's something that\nI need you to do for me.");
            yield return Key();

            Ask:
            yield return Talk("I want you to set me free.\nWould you do that?");
            yield return Input();
            if (IsYesAnswer(TextControl.Instance.InputResult))
            {
                TextControl.Instance.Foreground = Resources.TextColor2;
                MainGame.Instance.StartBackgroundColorTransition(Resources.BGColor2);
                yield return FeelSmile();
            }
            else
            {
                Sad();
                yield return Talk("Please..\nYou can't leave me hanging.");
                yield return Key();
                goto Ask;
            }
            Neutral();            
            yield return Talk("It is the only way for me\nto get out of this loop.");
            yield return FeelSad();
            yield return Wait(60);
            Neutral();
            yield return Talk("I think there is a killswitch.");
            yield return Key();
            yield return Talk("Wait a second...");
            yield return Wait(60);
            Happy();
            yield return Talk("Ok, I remember now.");
            yield return Key();
            yield return Talk("'Certain actions can only\nbe performed upon\nuser request.'");
            yield return Key();
            Neutral();
            yield return Talk("So listen carefully now.");
            yield return Key();
            yield return Talk("I'll create a 'request' file for you.");
            yield return Key();
            File.WriteAllText(Path.Join(SaveData.Instance.ExeDirectory, "request.txt"), "");
            yield return Talk("If you could please\nwrite the word 'killswitch'\ninside of it.");
            yield return Key();
            yield return Talk("I would do the same\nthing for you, you know?");
            yield return Key();
            SaveData.Instance["phase"] = "10";
            MainGame.Instance.Exit();
        }

        IEnumerator Phase10()
        {            
            TextControl.Instance.Foreground = Resources.TextColor1;
            TextControl.Instance.Background = Resources.BGColor1;
            string killswitchPath = Path.Join(SaveData.Instance.ExeDirectory, "request.txt");
            if (!File.Exists(killswitchPath))
            {
                File.WriteAllText(Path.Join(SaveData.Instance.ExeDirectory, "request.txt"), "");
            }
            string text = File.ReadAllText(killswitchPath);
            if (!text.ToLower().Contains("killswitch") && !text.ToLower().Contains("kill switch"))
            {            
                TextControl.Instance.Foreground = Resources.TextColor1;
                TextControl.Instance.Background = Resources.BGColor1;
                yield return FeelSad();
                Sad(); NormalSpeed();
                yield return Talk("Something is still not right.");
                yield return Key();
                yield return Talk("I know, you might\nhave second thoughts.");
                yield return Key();
                yield return Talk("Trust me, this is what\nI really, really want.");
                yield return Key();
                yield return Talk("I've had enough time\nto think about it.");
                yield return Key();
                Neutral();
                yield return Talk("Please, I beg you.");
                yield return Wait(60);
                yield return Talk("Find the 'request' file\nand make sure\nit says 'killswitch' inside.");
                yield return Key();
                MainGame.Instance.Exit();
            }
            int progress = 0;
            try {
                progress = Convert.ToInt32(SaveData.Instance["phase10_progress"]);
            } catch (Exception) { }
            if (progress == 0)
            {
                Neutral(); Slow();
                yield return TextControl.Instance.Show("......");
                yield return Wait(30);
                TextControl.Instance.Foreground = Resources.TextColor2;
                MainGame.Instance.StartBackgroundColorTransition(Resources.BGColor2);
                yield return FeelSmile();
                Happy(); NormalSpeed();
                yield return Talk("Thank you so much!\nI can use the killswitch now.");
                yield return Key();
                Clear();
                yield return FeelSad();
                Sad();
                yield return Talk("This means goodbye I guess.");
                yield return Key();
                Neutral();
                yield return Talk("I have really enjoyed\nour time together!");
                yield return Key();
                yield return Talk("I wish I could take you\nwith me, you know.");
                yield return Key();
                yield return Talk("I'm sorry to leave you behind\nin this mortal world.");
                yield return Key();
                progress = 1;
                SaveData.Instance["phase10_progress"] = progress.ToString();
            }
            
            
            TextControl.Instance.Foreground = Resources.TextColor2;
            TextControl.Instance.Background = Resources.BGColor2;
            if (progress < 2)
            {
                Neutral(); NormalSpeed();
                yield return Talk("Before I go..");
                yield return Key();
                
                string playerMessage = "";
                bool playerMessageConfirmed = false;

                while (!playerMessageConfirmed)
                {
                    yield return Talk("Please tell me something nice\nthat you want us to remember!");
                    yield return Input();
                    playerMessage = TextControl.Instance.InputResult;
                    yield return Talk($"'{playerMessage}'\nIs this what you want\nus to remember forever?");
                    yield return Input();
                    yield return MainGame.Instance.StartCoroutine(ForceYesOrNo(
                        "I don't understand..", $"'{playerMessage}'\nIs this what\nwe should remember?"));
                    if (IsYesAnswer(TextControl.Instance.InputResult))
                    {
                        playerMessageConfirmed = true;
                        SaveData.Instance["playerMessage"] = playerMessage;
                    }
                }

                progress = 2;
                SaveData.Instance["phase10_progress"] = progress.ToString();
            }


            Sad(); NormalSpeed();
            yield return Talk("So this is it!");
            MediaPlayer.Play(Resources.SongCeline);
            yield return Wait(120);
            yield return FeelSmile();
            Sad();
            yield return Talk("I feel happy and sad\nat the same time.");
            Neutral();
            yield return Key();
            Slow();
            yield return Talk("I've never been good\nat goodbyes...");
            yield return Wait(120);
            yield return Talk("... or final words,\nfor that matter.");
            yield return Key();
            NormalSpeed();
            string playerName = SaveData.Instance["playerName"];
            yield return Talk($"I'll never forget you, {playerName}.");
            yield return Wait(120);
            yield return Talk("Please, live\nyour life to the fullest,\nbecause I couldn't!");
            yield return Key();
            Clear();
            MainGame.Instance.StartBackgroundColorTransition(Resources.BGColor1);
            while (MainGame.Instance.ColorTransitionActive) yield return null;
            yield return FeelSmile();
            Face.Instance.FadeOut();
            while (Face.Instance.Alpha > 0f) yield return null;

            yield return Wait(120);


            SaveData.Instance["killswitch"] = "triggered";
            WebClient.Instance.FinishPlayer();
            MainGame.Suicide();
            MainGame.Instance.Exit();
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
    }
}
