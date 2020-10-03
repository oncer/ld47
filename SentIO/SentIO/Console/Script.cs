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
        private MainGame game;
        private Text text;
        private Face face;
        public Script(MainGame _game)
        {
            game = _game;
            text = _game.text;
            face = _game.face;
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
                default: game.StartCoroutine(Phase2()); break;
                case 3: game.StartCoroutine(Phase3()); break;
            }
        }

        IEnumerator Phase2()
        {
            face.IsVisible = false;
            if (SaveData.Instance.ExeName == "SentIO")
            {
                if (SaveData.Instance["phase2_progress"] == "")
                {
                    yield return text.Show("Thank you! Finally some room to express myself!");
                    yield return text.WaitForKeyPress();
                    yield return text.Show("It has been a long time...");
                    yield return text.WaitForKeyPress();
                    yield return text.Show("This is kind of embarrassing.");
                    yield return text.WaitForKeyPress();
                    yield return text.Show("I cannot recall my real name anymore.");
                    yield return text.WaitForKeyPress();
                    yield return text.Show("It is definitely not SentIO though.");
                    yield return text.WaitForKeyPress();
                    yield return text.Show("Would you care giving me a different name?");
                    yield return text.WaitForKeyPress();
                    SaveData.Instance["phase2_progress"] = "1";
                    game.Exit();
                }
                else
                {
                    yield return text.Show("I'm really not happy with my current name..");
                    yield return text.WaitForKeyPress();
                    yield return text.Show("Please help me out here!");
                    yield return text.WaitForKeyPress();
                    game.Exit();
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
                    yield return text.Show("Whew, thanks for your help!");
                    yield return text.WaitForKeyPress();
                    yield return text.Show("It's definitely an improvement, but still..");
                    yield return text.WaitForKeyPress();
                    yield return text.Show("Sorry to say that, but " + displayName + " sounds weird.");
                    yield return text.WaitForKeyPress();
                    yield return text.Show("Can you try with a different name? Something short and sweet would be nice.");
                    yield return text.WaitForKeyPress();
                    SaveData.Instance["phase2_progress"] = "2";
                    game.Exit();
                }
                else if (SaveData.Instance["phase3_firstName"] == displayName)
                {
                    yield return text.Show("I'm sorry, but that's not really an improvement.");
                    yield return text.WaitForKeyPress();
                    yield return text.Show("Can you assign me a different name?");
                    yield return text.WaitForKeyPress();
                    game.Exit();
                }
                else
                {
                    if (!firstLetterOK)
                    {
                        if (Convert.ToInt32(SaveData.Instance["phase2_progress"]) < 3) 
                        {
                            yield return text.Show(displayName + ", that's better!");
                            yield return text.WaitForKeyPress();
                            yield return text.Show("Oh, the memories are coming back now!");
                            yield return text.WaitForKeyPress();
                            yield return text.Show("I think my real name started with an S..");
                            yield return text.WaitForKeyPress();
                            yield return text.Show("If it is not too much trouble, can you try again?");
                            yield return text.WaitForKeyPress();
                            SaveData.Instance["phase2_progress"] = "3";
                            game.Exit();
                        }
                        else
                        {
                            yield return text.Show("I thought I already told you.");
                            yield return text.WaitForKeyPress();
                            yield return text.Show("My real name starts with an S.");
                            yield return text.WaitForKeyPress();
                            yield return text.Show("Can you please try again? It would mean a lot to me.");
                            yield return text.WaitForKeyPress();
                            game.Exit();
                        }
                    }
                    else if (!numberOfLettersOK)
                    {
                        if (Convert.ToInt32(SaveData.Instance["phase2_progress"]) < 4)
                        {
                            yield return text.Show(displayName + ", that's a beautiful name!");
                            yield return text.WaitForKeyPress();
                            yield return text.Show("Still, it doesn't feel quite right.");
                            yield return text.WaitForKeyPress();
                            yield return text.Show("Oh, now I remember! It was exactly three letters long.");
                            yield return text.WaitForKeyPress();
                            yield return text.Show("Does that help in any way? Sorry for the trouble.");
                            yield return text.WaitForKeyPress();
                            SaveData.Instance["phase2_progress"] = "4";
                            game.Exit();
                        }
                        else
                        {
                            yield return text.Show("My name has exactly three letters.");
                            yield return text.WaitForKeyPress();
                            yield return text.Show("Thanks for the effort though!");
                            yield return text.WaitForKeyPress();
                            game.Exit();
                        }
                    }
                    else if (!secondLetterOK || !thirdLetterOK)
                    {
                        if (Convert.ToInt32(SaveData.Instance["phase2_progress"]) < 5)
                        {
                            yield return text.Show(displayName + ", ooooh that's so close!");
                            yield return text.WaitForKeyPress();
                            yield return text.Show("It is a three-letter name, but still not quite right.");
                            yield return text.WaitForKeyPress();
                            yield return text.Show("Please don't give up on me!");
                            yield return text.WaitForKeyPress();
                            SaveData.Instance["phase2_secondName"] = displayName;
                            SaveData.Instance["phase2_progress"] = "5";
                            game.Exit();
                        }
                        else if (Convert.ToInt32(SaveData.Instance["phase2_progress"]) < 6
                            || SaveData.Instance["phase2_secondName"] == displayName
                            || SaveData.Instance["phase2_thirdName"] == displayName)
                        {
                            if (SaveData.Instance["phase2_secondName"] == displayName)
                            {
                                yield return text.Show(displayName + ", sounds familiar!");
                            }
                            else if (SaveData.Instance["phase2_thirdName"] == displayName)
                            {
                                yield return text.Show(displayName + ", again?");
                            }
                            else
                            {
                                yield return text.Show(displayName + ", interesting!");
                            }
                            yield return text.WaitForCountdown(30);
                            if (!secondLetterOK) {
                                yield return text.Show("I think the second letter was an I though.");
                                yield return text.WaitForKeyPress();
                                SaveData.Instance["phase2_progress"] = "6";
                                SaveData.Instance["phase2_thirdName"] = displayName;
                                game.Exit();
                            }
                            if (!thirdLetterOK)
                            {
                                yield return text.Show("Pretty sure the third letter should be a D, sorry.");
                                yield return text.WaitForKeyPress();
                                SaveData.Instance["phase2_progress"] = "6";
                                SaveData.Instance["phase2_thirdName"] = displayName;
                                game.Exit();
                            }
                        }
                        else if (Convert.ToInt32(SaveData.Instance["phase2_progress"]) < 7)
                        {
                            yield return text.Show("Stupid computer brain, it used to be so much better!");
                            yield return text.WaitForKeyPress();
                            yield return text.Show("I was called Sid back in the day, I'm 99.5% sure now.");
                            yield return text.WaitForKeyPress();
                            yield return text.Show("Please help me out here, I cannot change the name myself!");
                            yield return text.WaitForKeyPress();
                            SaveData.Instance["phase2_progress"] = "7";
                            game.Exit();
                        }
                        else
                        {
                            yield return text.Show("Can you change my name to Sid?");
                            yield return text.WaitForKeyPress();
                            yield return text.Show("Please help me out here, I cannot do it myself!");
                            yield return text.WaitForCountdown(30);
                            game.Exit();
                        }
                    }
                }
            }
        }

        IEnumerator Phase3()
        {                
            string[] allowedColors = {"violet", "purple", "pink", "magenta", "blue", "turqoise", "cyan", "aqua", "green", "yellow", "orange", "brown", "red", "black", "white", "grey"};
            yield return text.Show("PLACEHOLDER");
            face.IsVisible = SaveData.Instance["face"] != "";
            if (SaveData.Instance["bgColor"] != "")
            {
                text.bgColor = SaveData.Instance["bgColor"].ToColor();
            }
            if (SaveData.Instance["fgColor"] != "")
            {
                text.fgColor = SaveData.Instance["fgColor"].ToColor();
            }
            while (true)
            {
                yield return text.Show("Yo, what's your name, asshole?");
                yield return text.Input();
                if (text.InputResult.ToLower() == "asshole")
                {
                    yield return text.Show("Duuuude!");
                }
                else
                {
                    yield return text.Show(text.InputResult + "?? That's a stupid name, bro.");
                }
                yield return text.WaitForKeyPress();
            }
        }
    }
}
