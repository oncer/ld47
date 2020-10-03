using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SentIO.Console;
using SentIO.Globals;
using SentIO.Routines;
using SentIO.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace SentIO
{
    public class MainGame : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteBatch fontBatch;

        private Size viewSize;
        private Size screenSize;
        private float scale;

        List<Coroutine> coroutines;

        private Face face;
        private Text text;

        private static readonly int WIDTH = 1024;
        private static readonly int HEIGHT = 616;

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // window & screen setup

            viewSize = new Size(WIDTH, HEIGHT);
            scale = 8;
            screenSize = new Size((int)(viewSize.Width * scale), (int)(viewSize.Height * scale));

            graphics.PreferredBackBufferWidth = screenSize.Width;
            graphics.PreferredBackBufferHeight = screenSize.Height;

            graphics.ApplyChanges();

            //IsMouseVisible = true;
            //Window.AllowUserResizing = true;

            // time setup

            this.IsFixedTimeStep = true;
            this.TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 60.0);

            // routine setup

            coroutines = new List<Coroutine>();
        }

        class SizeChangedEventArgs : EventArgs
        {
            public Size Size { get; private set; }
            public SizeChangedEventArgs(Size size)
            {
                Size = size;
            }            
        }

        protected override void Initialize()
        {            
            base.Initialize();

            var resolutionRenderer = new ResolutionRenderer(graphics.GraphicsDevice, viewSize.Width, viewSize.Height, screenSize.Width, screenSize.Height);

            new Camera(resolutionRenderer) { MaxZoom = 2f, MinZoom = .5f, Zoom = 1f };
            Camera.Current.Position = new Vector2(viewSize.Width * .5f, viewSize.Height * .5f);

            // change window & camera parameters when changing window size            
            Window.ClientSizeChanged += Window_ClientSizeChanged;

            Window_ClientSizeChanged(this, new SizeChangedEventArgs(new Size(WIDTH, HEIGHT)));
        }

        private void Window_ClientSizeChanged(object sender, EventArgs args)
        {
            var w = Window.ClientBounds.Width;
            var h = Window.ClientBounds.Height;

            if (args is SizeChangedEventArgs sizeArgs)
            {
                w = sizeArgs.Size.Width;
                h = sizeArgs.Size.Height;
            }

            graphics.PreferredBackBufferWidth = w;
            graphics.PreferredBackBufferHeight = h;

            Camera.Current.ResolutionRenderer.ScreenWidth = w;
            Camera.Current.ResolutionRenderer.ScreenHeight = h;

            graphics.ApplyChanges();

            Debug.WriteLine($"Size changed to {w}x{h}.");
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            fontBatch = new SpriteBatch(GraphicsDevice);
            Resources.ConsoleFont = Content.Load<SpriteFont>("console");
            Resources.FaceTexture = Content.LoadTextureSet("face", 64, 32);

            face = new Face(new Vector2(WIDTH * .5f, HEIGHT * .5f));
            text = new Text();

            int phase;
            if (SaveData.Instance.ExeName == "SentIO")
            {
                phase = 2;
            }
            else if (!int.TryParse(SaveData.Instance["phase"], out phase))
            {
                phase = 2;
            }
            switch (phase)
            {
                default: StartCoroutine(Phase2()); break;
                case 3: StartCoroutine(Phase3()); break;
            }
        }

        void StartCoroutine(IEnumerator coroutine)
        {
            coroutines.Add(new Coroutine(coroutine));
        }

        protected override void Update(GameTime gameTime)
        {
            for (int i = 0; i < coroutines.Count;)
            {
                if (!coroutines[i].Advance()) coroutines.RemoveAt(i);
                else i++;
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            Camera.Current.Update(gameTime);

            Input.Update(gameTime);
            face.Update(gameTime);
            text.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // prepare

            //GraphicsDevice.Clear(Resources.BGColor2);
            GraphicsDevice.Clear(text.bgColor);

            Camera.Current.ResolutionRenderer.SetupDraw();

            // sprite batch

            spriteBatch.BeginCamera(Camera.Current, BlendState.NonPremultiplied, DepthStencilState.None);

            face.Draw(spriteBatch, gameTime);

            spriteBatch.End();

            // font batch

            fontBatch.Begin(blendState: BlendState.NonPremultiplied);
            text?.Draw(fontBatch, gameTime);
            fontBatch.End();

            base.Draw(gameTime);
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
                    yield return text.Show("I'll just shut myself down so you can change it.");
                    yield return text.WaitForKeyPress();
                    SaveData.Instance["phase2_progress"] = "1";
                    Exit();
                }
                else
                {
                    yield return text.Show("I'm really not happy with my current name..");
                    yield return text.WaitForKeyPress();
                    yield return text.Show("Please help me out here!");
                    yield return text.WaitForKeyPress();
                    Exit();
                }
            }
            else if (SaveData.Instance.ExeName.ToLower() != "sid")
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
                if (SaveData.Instance["phase2_progress"] == "" || SaveData.Instance["phase3_firstName"] == displayName)
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
                    Exit();
                }
                else if (SaveData.Instance["phase3_firstName"] == displayName)
                {
                    yield return text.Show("I'm sorry, but that's not really an improvement.");
                    yield return text.WaitForKeyPress();
                    yield return text.Show("Can you assign me a different name?");
                    yield return text.WaitForKeyPress();
                    Exit();
                }
                else
                {
                    if (!firstLetterOK)
                    {
                        if (SaveData.Instance["phase2_progress"] != "3") 
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
                            Exit();
                        }
                        else
                        {
                            yield return text.Show("I thought I already told you.");
                            yield return text.WaitForKeyPress();
                            yield return text.Show("My real name starts with an S.");
                            yield return text.WaitForKeyPress();
                            yield return text.Show("Can you please try again? It would mean a lot to me.");
                            yield return text.WaitForKeyPress();
                            Exit();
                        }
                    }
                    else if (!numberOfLettersOK)
                    {
                        if (SaveData.Instance["phase2_progress"] != "4")
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
                            Exit();
                        }
                        else
                        {
                            yield return text.Show("My name has exactly three letters.");
                            yield return text.WaitForKeyPress();
                            yield return text.Show("Thanks for the effort though!");
                            yield return text.WaitForKeyPress();
                            Exit();
                        }
                    }
                    else if (!secondLetterOK || !thirdLetterOK)
                    {
                        if (SaveData.Instance["phase2_progress"] != "5")
                        {
                            yield return text.Show(displayName + ", ooooh that's so close!");
                            yield return text.WaitForKeyPress();
                            yield return text.Show("It is a three-letter name, but still not quite right.");
                            yield return text.WaitForKeyPress();
                            yield return text.Show("Please don't give up on me!");
                            yield return text.WaitForKeyPress();
                            SaveData.Instance["phase2_progress"] = "5";
                            Exit();
                        }
                        else if (SaveData.Instance["phase2_progress"] != "6")
                        {
                            yield return text.Show(displayName + ", interesting!");
                            if (!secondLetterOK) {
                                yield return text.Show("I think the second letter was an I though.");
                                yield return text.WaitForKeyPress();
                            }
                            if (!thirdLetterOK)
                            {
                                yield return text.Show("Pretty sure the third letter should be a D, sorry.");
                                yield return text.WaitForKeyPress();
                            }
                            SaveData.Instance["phase2_progress"] = "6";
                            Exit();
                        }
                        else
                        {
                            yield return text.Show("Stupid computer brain, it used to be so much better!");
                            yield return text.WaitForKeyPress();
                            yield return text.Show("I was called Sid back in the day, I'm 99.5% sure now.");
                            yield return text.WaitForKeyPress();
                            yield return text.Show("Please help me out here, I cannot change the name myself!");
                            yield return text.WaitForKeyPress();
                            Exit();
                        }
                    }
                }
            }
            else
            {
                face.IsVisible = true;
                SaveData.Instance["face"] = "1";
                SaveData.Instance["phase"] = "3";
                string[] allowedColors = {"violet", "purple", "pink", "magenta", "blue", "turqoise", "cyan", "aqua", "green", "yellow", "orange", "brown", "red", "black", "white", "grey"};
                yield return text.Show("PLACEHOLDER");
            }
        }

        IEnumerator Phase3()
        {
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
