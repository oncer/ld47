using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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

        private Script script;

        private static readonly int WIDTH = 1008;
        private static readonly int HEIGHT = 624;

        private static int W;
        private static int H;

        public static MainGame Instance { get; private set; }

        public MainGame()
        {
            Instance = this;

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
            Camera.Instance.Position = new Vector2(viewSize.Width * .5f, viewSize.Height * .5f);

            // change window & camera parameters when changing window size            
            Window.ClientSizeChanged += Window_ClientSizeChanged;

            Window_ClientSizeChanged(this, new SizeChangedEventArgs(viewSize));
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

            var display = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
            Window.Position = new Point((int)((display.Width - w) * .5f), (int)((display.Height - h) * .5f));
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            fontBatch = new SpriteBatch(GraphicsDevice);
            Resources.ConsoleFont = Content.Load<SpriteFont>("console");
            Resources.FaceTexture = Content.LoadTextureSet("face", 64, 32);
            Resources.SfxChar = Content.Load<SoundEffect>("sfxChar");
            Resources.SfxCharStop = Content.Load<SoundEffect>("sfxCharStop");

            Face.Instance.Position = new Vector2(W * .5f, H * .5f);
            script = new Script();
        }

        public void StartCoroutine(IEnumerator coroutine)
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
            
            Camera.Instance.Update();

            Input.Update();
            Face.Instance.Update();
            TextControl.Instance.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // prepare

            GraphicsDevice.Clear(TextControl.Instance.Background);

            Camera.Instance.ResolutionRenderer.SetupDraw();

            // sprite batch

            spriteBatch.BeginCamera(Camera.Instance, BlendState.NonPremultiplied, DepthStencilState.None);

            Face.Instance.Draw(spriteBatch);

            spriteBatch.End();

            // font batch

            fontBatch.Begin(blendState: BlendState.NonPremultiplied);
            TextControl.Instance.Draw(fontBatch);
            fontBatch.End();

            base.Draw(gameTime);
        }
    }
}
