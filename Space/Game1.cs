using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;

namespace Space
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        List<Sprite> sprites = new();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.IsFullScreen = false;
            Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {
            Creator.NewPlanetCreated += LoadNewPlanet;
            Creator.NewRocketCreated += LoadRocket;
            Controller.RocketLaunched += HandleLaunch;

            GameManager.Initialize();
            Controller.Init();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Controller.FullScreenToggled += ToggleFullScreen;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Controller.Update();
            GameManager.Update();
            foreach (var sprite in sprites)
            {
                sprite.Update();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            foreach (var sprite in sprites)
            {
                _spriteBatch.Draw(sprite.texture,
                sprite.position,
                null,
                Color.White,
                sprite.rotation,
                new Vector2(sprite.texture.Width / 2, sprite.texture.Height / 2),
                sprite.Scale,
                SpriteEffects.None,
                0f);
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        public void ToggleFullScreen()
        {
            _graphics.PreferredBackBufferWidth = GraphicsDevice.Adapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsDevice.Adapter.CurrentDisplayMode.Height;
            _graphics.IsFullScreen = !_graphics.IsFullScreen;
            _graphics.ApplyChanges();
        }

        private void HandleLaunch()
        {
            GameManager.Launch();
        }

        #region LoadNewContent
        private void LoadRocket(Rocket rocket)
        {
            var rocketSprite = new Sprite(Content.Load<Texture2D>("rocket"), rocket.Position,0.2f);
            rocket.ObjectMoved += rocketSprite.MoveSpriteTo;
            rocket.RocketRotated += rocketSprite.Rotate;
            sprites.Add(rocketSprite);
        }

        private void LoadNewPlanet(Planet planet)
        {
            var rand = new Random();
            var planetSprite = new Sprite(Content.Load<Texture2D>("Planet" + rand.Next(1,8)), planet.Position,1.5f);
            planet.ObjectMoved += planetSprite.MoveSpriteTo;
            sprites.Add(planetSprite);
        }
        #endregion LoadNewContent
    }
}
