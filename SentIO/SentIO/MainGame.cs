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
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

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

            IsMouseVisible = true;
            Window.AllowUserResizing = true;

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
            Resources.FaceTexture = Content.LoadTextureSet("face", 32, 32);

            face = new Face(new Vector2(WIDTH * .5f, HEIGHT * .5f));
            text = new Text();

            StartCoroutine(MyScript());
        }

        void StartCoroutine(IEnumerator coroutine)
        {
            coroutines.Add(new Coroutine(coroutine));
        }

        IEnumerator MyScript()
        {
            while (true)
            {
                yield return text.Show("Hello, World.");
                yield return new WaitInstruction(100);
                //yield return text.Show("Another World.");
                //yield return text.Show("Yet another world.");
                yield return text.Show("Looks like I'm stuck in a loop...");
                yield return new WaitInstruction(100);
            }
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
            GraphicsDevice.Clear(Color.Black);

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
    }
}
