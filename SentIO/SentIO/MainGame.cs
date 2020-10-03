using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SentIO.Console;
using SentIO.Globals;
using SentIO.UI;
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

        private Face face;

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // window & screen setup

            viewSize = new Size(256, 144);
            scale = 4;
            screenSize = new Size((int)(viewSize.Width * scale), (int)(viewSize.Height * scale));

            graphics.PreferredBackBufferWidth = screenSize.Width;
            graphics.PreferredBackBufferHeight = screenSize.Height;

            //graphics.GraphicsDevice.Viewport = new Viewport(0, 0, 512, 144);
            
            this.IsFixedTimeStep = true;
            this.TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 60.0);
        }

        protected override void Initialize()
        {            
            base.Initialize();

        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Resources.ConsoleFont = Content.Load<SpriteFont>("console");
            Resources.FaceTexture = Content.LoadTextureSet("face", 64, 64);

            face = new Face(new Vector2(64, 64));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Input.Update(gameTime);

            Script.DisplayText("Hallo");
            Script.DisplayText("New text");
            var answer = Script.DisplayQuestion("New question?");
            if (answer == "Yes")
            {
                Script.DisplayText("New text");
            }

            //text?.Update(gameTime);

            face.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Resources.BGColor2);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState:BlendState.NonPremultiplied, depthStencilState:DepthStencilState.None);

            face.Draw(spriteBatch, gameTime);

            //text?.Draw(spriteBatch, gameTime);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
