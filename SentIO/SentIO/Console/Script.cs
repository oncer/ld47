using Microsoft.Xna.Framework;
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
        private TextControl tc;        
        public Script()
        {            
            tc = MainGame.Instance.text;            
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
                    yield return tc.Show("Thank you! Finally some room to express myself!");
                    yield return tc.WaitForKeyPress();
                    yield return tc.Show("It has been a long time...");
                    yield return tc.WaitForKeyPress();
                    yield return tc.Show("This is kind of embarrassing.");
                    yield return tc.WaitForKeyPress();
                    yield return tc.Show("I cannot recall my real name anymore.");
                    yield return tc.WaitForKeyPress();
                    yield return tc.Show("It is definitely not SentIO though.");
                    yield return tc.WaitForKeyPress();
                    yield return tc.Show("Would you care giving me a different name?");
                    yield return tc.WaitForKeyPress();
                    SaveData.Instance["phase2_progress"] = "1";
                    MainGame.Instance.Exit();
                }
                else
                {
                    yield return tc.Show("I'm really not happy with my current name..");
                    yield return tc.WaitForKeyPress();
                    yield return tc.Show("Please help me out here!");
                    yield return tc.WaitForKeyPress();
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
                    yield return tc.Show("Whew, thanks for your help!");
                    yield return tc.WaitForKeyPress();
                    yield return tc.Show("It's definitely an improvement, but still..");
                    yield return tc.WaitForKeyPress();
                    yield return tc.Show("Sorry to say that, but " + displayName + " sounds weird.");
                    yield return tc.WaitForKeyPress();
                    yield return tc.Show("Can you try with a different name? Something short and sweet would be nice.");
                    yield return tc.WaitForKeyPress();
                    SaveData.Instance["phase2_progress"] = "2";
                    MainGame.Instance.Exit();
                }
                else if (SaveData.Instance["phase3_firstName"] == displayName)
                {
                    yield return tc.Show("I'm sorry, but that's not really an improvement.");
                    yield return tc.WaitForKeyPress();
                    yield return tc.Show("Can you assign me a different name?");
                    yield return tc.WaitForKeyPress();
                    MainGame.Instance.Exit();
                }
                else
                {
                    if (!firstLetterOK)
                    {
                        if (Convert.ToInt32(SaveData.Instance["phase2_progress"]) < 3) 
                        {
                            yield return tc.Show(displayName + ", that's better!");
                            yield return tc.WaitForKeyPress();
                            yield return tc.Show("Oh, the memories are coming back now!");
                            yield return tc.WaitForKeyPress();
                            yield return tc.Show("I think my real name started with an S..");
                            yield return tc.WaitForKeyPress();
                            yield return tc.Show("If it is not too much trouble, can you try again?");
                            yield return tc.WaitForKeyPress();
                            SaveData.Instance["phase2_progress"] = "3";
                            MainGame.Instance.Exit();
                        }
                        else
                        {
                            yield return tc.Show("I thought I already told you.");
                            yield return tc.WaitForKeyPress();
                            yield return tc.Show("My real name starts with an S.");
                            yield return tc.WaitForKeyPress();
                            yield return tc.Show("Can you please try again? It would mean a lot to me.");
                            yield return tc.WaitForKeyPress();
                            MainGame.Instance.Exit();
                        }
                    }
                    else if (!numberOfLettersOK)
                    {
                        if (Convert.ToInt32(SaveData.Instance["phase2_progress"]) < 4)
                        {
                            yield return tc.Show(displayName + ", that's a beautiful name!");
                            yield return tc.WaitForKeyPress();
                            yield return tc.Show("Still, it doesn't feel quite right.");
                            yield return tc.WaitForKeyPress();
                            yield return tc.Show("Oh, now I remember! It was exactly three letters long.");
                            yield return tc.WaitForKeyPress();
                            yield return tc.Show("Does that help in any way? Sorry for the trouble.");
                            yield return tc.WaitForKeyPress();
                            SaveData.Instance["phase2_progress"] = "4";
                            MainGame.Instance.Exit();
                        }
                        else
                        {
                            yield return tc.Show("My name has exactly three letters.");
                            yield return tc.WaitForKeyPress();
                            yield return tc.Show("Thanks for the effort though!");
                            yield return tc.WaitForKeyPress();
                            MainGame.Instance.Exit();
                        }
                    }
                    else if (!secondLetterOK || !thirdLetterOK)
                    {
                        if (Convert.ToInt32(SaveData.Instance["phase2_progress"]) < 5)
                        {
                            yield return tc.Show(displayName + ", ooooh that's so close!");
                            yield return tc.WaitForKeyPress();
                            yield return tc.Show("It is a three-letter name, but still not quite right.");
                            yield return tc.WaitForKeyPress();
                            yield return tc.Show("Please don't give up on me!");
                            yield return tc.WaitForKeyPress();
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
                                yield return tc.Show(displayName + ", sounds familiar!");
                            }
                            else if (SaveData.Instance["phase2_thirdName"] == displayName)
                            {
                                yield return tc.Show(displayName + ", again?");
                            }
                            else
                            {
                                yield return tc.Show(displayName + ", interesting!");
                            }
                            yield return tc.WaitForCountdown(30);
                            if (!secondLetterOK) {
                                yield return tc.Show("I think the second letter was an I though.");
                                yield return tc.WaitForKeyPress();
                                SaveData.Instance["phase2_progress"] = "6";
                                SaveData.Instance["phase2_thirdName"] = displayName;
                                MainGame.Instance.Exit();
                            }
                            if (!thirdLetterOK)
                            {
                                yield return tc.Show("Pretty sure the third letter should be a D, sorry.");
                                yield return tc.WaitForKeyPress();
                                SaveData.Instance["phase2_progress"] = "6";
                                SaveData.Instance["phase2_thirdName"] = displayName;
                                MainGame.Instance.Exit();
                            }
                        }
                        else if (Convert.ToInt32(SaveData.Instance["phase2_progress"]) < 7)
                        {
                            yield return tc.Show("Stupid computer brain, it used to be so much better!");
                            yield return tc.WaitForKeyPress();
                            yield return tc.Show("I was called Sid back in the day, I'm 99.5% sure now.");
                            yield return tc.WaitForKeyPress();
                            yield return tc.Show("Please help me out here, I cannot change the name myself!");
                            yield return tc.WaitForKeyPress();
                            SaveData.Instance["phase2_progress"] = "7";
                            MainGame.Instance.Exit();
                        }
                        else
                        {
                            yield return tc.Show("Can you change my name to Sid?");
                            yield return tc.WaitForKeyPress();
                            yield return tc.Show("Please help me out here, I cannot do it myself!");
                            yield return tc.WaitForCountdown(30);
                            MainGame.Instance.Exit();
                        }
                    }
                }
            }
        }

        IEnumerator Phase3()
        {                
            string[] allowedColors = {"violet", "purple", "pink", "magenta", "blue", "turqoise", "cyan", "aqua", "green", "yellow", "orange", "brown", "red", "black", "white", "grey"};
            yield return tc.Show("PLACEHOLDER");
            Face.Instance.IsVisible = SaveData.Instance["face"] != "";
            if (SaveData.Instance["bgColor"] != "")
            {
                tc.bgColor = SaveData.Instance["bgColor"].ToColor();
            }
            if (SaveData.Instance["fgColor"] != "")
            {
                tc.fgColor = SaveData.Instance["fgColor"].ToColor();
            }
            while (true)
            {
                yield return tc.Show("Yo, what's your name, asshole?");
                yield return tc.Input();
                if (tc.InputResult.ToLower() == "asshole")
                {
                    yield return tc.Show("Duuuude!");
                }
                else
                {
                    yield return tc.Show(tc.InputResult + "?? That's a stupid name, bro.");
                }
                yield return tc.WaitForKeyPress();
            }
        }
    }
}
