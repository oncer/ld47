﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SentIO.Console;
using SentIO.MiniGame;
using SentIO.Globals;
using SentIO.Routines;
using SentIO.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework.Media;

namespace SentIO
{
    public class MainGame : Game
    {
        private long framesElapsed = 0;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteBatch fontBatch;

        private Size viewSize;
        private Size screenSize;
        private float scale;

        List<Coroutine> coroutines;

        private Script script;

        private static readonly int WIDTH = 1008;
        private static readonly int HEIGHT = 624;

        private static int W;
        private static int H;

        public static MainGame Instance { get; private set; }

        public Dice Dice { get; private set; }
        public ScoreBoard ScoreBoard { get; private set; }

        public MainGame()
        {
            Instance = this;
            framesElapsed = 0;
            try {
                framesElapsed = Convert.ToInt32(SaveData.Instance["secondsPlayed"]) * 60;
            } catch (Exception) { }
            
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // window & screen setup

            W = WIDTH;
            //H = SaveData.Instance.ExeName.ToLower() == "sid" ? HEIGHT : HEIGHT_SMALL;
            H = HEIGHT;

            viewSize = new Size(W, H);
            scale = 8;
            screenSize = new Size((int)(viewSize.Width * scale), (int)(viewSize.Height * scale));

            //graphics.PreferredBackBufferWidth = screenSize.Width;
            //graphics.PreferredBackBufferHeight = screenSize.Height;

            graphics.ApplyChanges();

            //IsMouseVisible = true;
            //Window.AllowUserResizing = true;

            // time setup

            this.IsFixedTimeStep = true;
            this.TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 60.0);

            // routine setup

            coroutines = new List<Coroutine>();
            
            if (Resources.DisplayName.ToLower() != "sentio")
            {
                Window.Title = Resources.DisplayName;
            }
            //Window.Title = "Kevin"; // remove me, duh
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
            Camera.Instance.Position = new Vector2(viewSize.Width * .5f, viewSize.Height * .5f);

            // change window & camera parameters when changing window size            
            Window.ClientSizeChanged += Window_ClientSizeChanged;

            Window_ClientSizeChanged(this, new SizeChangedEventArgs(viewSize));

            var display = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
            Window.Position = new Point((int)((display.Width - Window.ClientBounds.Width) * .5f), (int)((display.Height - Window.ClientBounds.Height) * .5f));
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

            Camera.Instance.ResolutionRenderer.ScreenWidth = w;
            Camera.Instance.ResolutionRenderer.ScreenHeight = h;

            graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            fontBatch = new SpriteBatch(GraphicsDevice);
            Resources.ConsoleFont = Content.Load<SpriteFont>("console");
            Resources.FaceTexture = Content.LoadTextureSet("face", 64, 32);
            Resources.DiceTexture = Content.LoadTextureSet("dice", 32, 32);
            Resources.SfxChar = Content.Load<SoundEffect>("sfxChar");
            Resources.SfxCharStop = Content.Load<SoundEffect>("sfxCharStop");
            Resources.SongCeline = Content.Load<Song>("Celine");

            Face.Instance.Position = new Vector2(W * .5f, H * .5f);

            Dice = new Dice();            
            //Dice.Position = new Vector2(W * .25f, H * .75f);
            //Dice.Roll();

            ScoreBoard = new ScoreBoard();

            // Script should be initialized last!
            script = new Script();
        }

        public CoroutineWait StartCoroutine(IEnumerator coroutine)
        {
            var cr = new Coroutine(coroutine);
            coroutines.Add(cr);
            return new CoroutineWait(cr);
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
            
            //if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            //{
            //    if (Dice.State == Dice.DiceState.Done)
            //    {
            //        Dice.Roll();
            //    }
            //}

            Camera.Instance.Update();

            Input.Update();
            Face.Instance.Update();
            TextControl.Instance.Update();

            Dice.Update();
            ScoreBoard.Update();
            framesElapsed++;
            if (framesElapsed % 60 == 0)
            {
                SaveData.Instance.SetValueWithoutSaving("secondsPlayed", (framesElapsed / 60).ToString());
            }

            base.Update(gameTime);
        }

        public bool ColorTransitionActive { get; private set; }
        float r;
        float g;
        float b;
        Color targetColor;
        public void StartBackgroundColorTransition(Color targetColor)
        {
            //this.currentColor = TextControl.Instance.Background;
            r = (float)TextControl.Instance.Background.R;
            g = (float)TextControl.Instance.Background.G;
            b = (float)TextControl.Instance.Background.B;
            this.targetColor = targetColor;
            ColorTransitionActive = true;
        }

        protected override void Draw(GameTime gameTime)
        {
            // prepare

            if (!ColorTransitionActive)
                GraphicsDevice.Clear(TextControl.Instance.Background);
            else
            {
                var f = 40f;

                r += (targetColor.R - r) / f;
                g += (targetColor.G - g) / f;
                b += (targetColor.B - b) / f;

                var currentColor = new Color((byte)r, (byte)g, (byte)b);
                GraphicsDevice.Clear(currentColor);
                
                if (Math.Abs(r - targetColor.R) < 2 && Math.Abs(g - targetColor.G) < 2 && Math.Abs(b - targetColor.B) < 2)
                {
                    ColorTransitionActive = false;
                    TextControl.Instance.Background = targetColor;
                }
            }

            Camera.Instance.ResolutionRenderer.SetupDraw();

            // sprite batch

            spriteBatch.BeginCamera(Camera.Instance, BlendState.NonPremultiplied, DepthStencilState.None);

            Face.Instance.Draw(spriteBatch);
            
            ScoreBoard.Draw(spriteBatch);
            Dice.Draw(spriteBatch);

            spriteBatch.End();

            // font batch

            fontBatch.Begin(blendState: BlendState.NonPremultiplied, depthStencilState:DepthStencilState.None, samplerState:SamplerState.PointClamp);
            TextControl.Instance.Draw(fontBatch);
            fontBatch.End();

            base.Draw(gameTime);
        }

        public static void LeaveURL()
        {
            string linkPath = Path.Join(SaveData.Instance.ExeDirectory, "See you!.URL");
            File.WriteAllText(linkPath, @"[InternetShortcut]
URL=https://sentio.simonparzer.com/");
        }

        public void ExitAndReopen()
        {
            string batchFileName = "SentIOExit.bat";
            string batchFilePath = Path.Join(Path.GetTempPath(), batchFileName);
            string exePath = SaveData.Instance.ExePath;

            string batchCommands = string.Empty;

            batchCommands += "@ECHO OFF\n";                         // Do not show any output
            batchCommands += "ping -n 2 127.0.0.1 > nul\n";         // Wait 1-2 seconds (so that the process is already terminated)
            batchCommands += "\"" + exePath + "\"\n";               // Start the executable again
            File.WriteAllText(batchFilePath, batchCommands);

            ProcessStartInfo batchProcess = new ProcessStartInfo();
            batchProcess.WindowStyle = ProcessWindowStyle.Hidden;
            batchProcess.CreateNoWindow = true;
            batchProcess.FileName = batchFilePath;
            Process.Start(batchProcess);
            Exit();
        }

        public static void Suicide()
        {
            string batchFileName = "SentIOSuicide.bat";
            string batchFilePath = Path.Join(Path.GetTempPath(), batchFileName);
            string exePath = SaveData.Instance.ExePath;

            string batchCommands = string.Empty;

            batchCommands += "@ECHO OFF\n";                         // Do not show any output
            batchCommands += "ping -n 2 127.0.0.1 > nul\n";         // Wait 1-2 seconds (so that the process is already terminated)
            batchCommands += "echo j | del /F \"";                    // Delete the executeable
            batchCommands += exePath + "\"\n";
            batchCommands += $"echo j | del \"{batchFileName}\"";    // Delete this bat file

            File.WriteAllText(batchFilePath, batchCommands);


            ProcessStartInfo batchProcess = new ProcessStartInfo();
            batchProcess.WindowStyle = ProcessWindowStyle.Hidden;
            batchProcess.CreateNoWindow = true;
            batchProcess.FileName = batchFilePath;
            Process.Start(batchProcess);
        }

    }
}
