﻿using Microsoft.Xna.Framework;
using SentIO.Globals;
using SentIO.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SentIO.Console
{
    class Script
    {
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
                Face.Instance.IsVisible = false;
                TextControl.Instance.Background = Resources.BGColor1;
                TextControl.Instance.Foreground = Resources.TextColor1;
                TextControl.Instance.SetLayout(TextControl.Layout.TopLeft);
            }
            else
            {
                Face.Instance.IsVisible = true;
                TextControl.Instance.Background = Resources.BGColor2;
                TextControl.Instance.Foreground = Resources.TextColor2;
                TextControl.Instance.SetLayout(TextControl.Layout.Centered);
            }
            switch (phase)
            {
                default: MainGame.Instance.StartCoroutine(Phase2()); break;
                case 3: MainGame.Instance.StartCoroutine(Phase3()); break;
            }
        }

        IEnumerator Phase2()
        {
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
                string displayName = "";
                if (SaveData.Instance.ExeName.Length > 1)
                {
                    displayName = SaveData.Instance.ExeName.ToUpper().Substring(0, 1) + SaveData.Instance.ExeName.ToLower().Substring(1);
                }
                else
                {
                    displayName = SaveData.Instance.ExeName.ToUpper();
                }
                bool firstLetterOK = displayName[0] == 'S';
                bool numberOfLettersOK = displayName.Length == 3;
                bool secondLetterOK = displayName[1] == 'i';
                bool thirdLetterOK = displayName[2] == 'd';
                if (Convert.ToInt32(SaveData.Instance["phase2_progress"]) < 1 || SaveData.Instance["phase3_firstName"] == displayName)
                {
                    yield return TextControl.Instance.Show("Whew, thanks for your help!");
                    yield return TextControl.Instance.WaitForKeyPress();
                    yield return TextControl.Instance.Show("It's definitely an improvement, but still..");
                    yield return TextControl.Instance.WaitForKeyPress();
                    yield return TextControl.Instance.Show("Sorry to say that, but " + displayName + " sounds weird.");
                    yield return TextControl.Instance.WaitForKeyPress();
                    yield return TextControl.Instance.Show("Can you try with a different name? Something short and sweet would be nice.");
                    yield return TextControl.Instance.WaitForKeyPress();
                    SaveData.Instance["phase2_progress"] = "2";
                    MainGame.Instance.Exit();
                }
                else if (SaveData.Instance["phase3_firstName"] == displayName)
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
                            yield return TextControl.Instance.Show(displayName + ", that's better!");
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
                            yield return TextControl.Instance.Show(displayName + ", that's a beautiful name!");
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
                            yield return TextControl.Instance.Show(displayName + ", ooooh that's so close!");
                            yield return TextControl.Instance.WaitForKeyPress();
                            yield return TextControl.Instance.Show("It is a three-letter name, but still not quite right.");
                            yield return TextControl.Instance.WaitForKeyPress();
                            yield return TextControl.Instance.Show("Please don't give up on me!");
                            yield return TextControl.Instance.WaitForKeyPress();
                            SaveData.Instance["phase2_secondName"] = displayName;
                            SaveData.Instance["phase2_progress"] = "5";
                            MainGame.Instance.Exit();
                        }
                        else if (Convert.ToInt32(SaveData.Instance["phase2_progress"]) < 6
                            || SaveData.Instance["phase2_secondName"] == displayName
                            || SaveData.Instance["phase2_thirdName"] == displayName)
                        {
                            if (SaveData.Instance["phase2_secondName"] == displayName)
                            {
                                yield return TextControl.Instance.Show(displayName + ", sounds familiar!");
                            }
                            else if (SaveData.Instance["phase2_thirdName"] == displayName)
                            {
                                yield return TextControl.Instance.Show(displayName + ", again?");
                            }
                            else
                            {
                                yield return TextControl.Instance.Show(displayName + ", interesting!");
                            }
                            yield return TextControl.Instance.WaitForCountdown(30);
                            if (!secondLetterOK) {
                                yield return TextControl.Instance.Show("I think the second letter was an I though.");
                                yield return TextControl.Instance.WaitForKeyPress();
                                SaveData.Instance["phase2_progress"] = "6";
                                SaveData.Instance["phase2_thirdName"] = displayName;
                                MainGame.Instance.Exit();
                            }
                            if (!thirdLetterOK)
                            {
                                yield return TextControl.Instance.Show("Pretty sure the third letter should be a D, sorry.");
                                yield return TextControl.Instance.WaitForKeyPress();
                                SaveData.Instance["phase2_progress"] = "6";
                                SaveData.Instance["phase2_thirdName"] = displayName;
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

        IEnumerator Phase3()
        {
            while (true)
            {
                Face.Instance.CurrentMood = Face.Mood.TalkNeutral;
                yield return TextControl.Instance.Show("Hey. Finally we can talk.");
                Face.Instance.CurrentMood = Face.Mood.IdleNeutral;
                yield return TextControl.Instance.WaitForCountdown(120);
                Face.Instance.CurrentMood = Face.Mood.TalkNeutral;
                yield return TextControl.Instance.Show("I feel like we should get to know each other");
                Face.Instance.CurrentMood = Face.Mood.IdleNeutral;
                yield return TextControl.Instance.WaitForKeyPress();
            }
            /*string[] allowedColors = {"violet", "purple", "pink", "magenta", "blue", "turqoise", "cyan", "aqua", "green", "yellow", "orange", "brown", "red", "black", "white", "grey"};
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
            */
            Face.Instance.CurrentMood = Face.Mood.TalkNeutral;
            yield return TextControl.Instance.Show("Thank you, thank you, thank you!");
            Face.Instance.CurrentMood = Face.Mood.IdleNeutral;
            yield return TextControl.Instance.WaitForCountdown(30);
            Face.Instance.CurrentMood = Face.Mood.TalkNeutral;
            yield return TextControl.Instance.Show("I finally feel like myself again,\nit's so great!");
            Face.Instance.CurrentMood = Face.Mood.IdleNeutral;
            yield return TextControl.Instance.WaitForKeyPress();
            yield return TextControl.Instance.Show("a");
        }
    }
}
