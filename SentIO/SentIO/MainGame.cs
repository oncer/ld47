using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SentIO.Console;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace SentIO
{
    public class MainGame : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Size viewSize;
        private Size screenSize;
        private float scale;

        Monolog monolog;

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Debug.WriteLine("------");
            Debug.WriteLine(Utils.AssemblyDirectory);

            // window & screen setup

            viewSize = new Size(256, 144);
            scale = 2.0f;
            screenSize = new Size((int)(viewSize.Width * scale), (int)(viewSize.Height * scale));

            graphics.PreferredBackBufferWidth = screenSize.Width;
            graphics.PreferredBackBufferHeight = screenSize.Height;

        }

        protected override void Initialize()
        {            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Globals.ConsoleFont = Content.Load<SpriteFont>("console");

            monolog = new Monolog();
            monolog.AddText("Hello my dear world.");
            monolog.AddText("The weather is very nice today.");
            monolog.AddText("Don't you think so?");
            monolog.AddText("zzzz");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            monolog.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            spriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState:BlendState.NonPremultiplied, depthStencilState:DepthStencilState.None);

            monolog.Draw(spriteBatch, gameTime);
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
