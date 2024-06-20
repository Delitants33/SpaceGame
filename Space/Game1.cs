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
        private Rocket Rocket;
        List<Sprite> sprites = new();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.IsFullScreen = false;
        }

        protected override void Initialize()
        {
            Rocket = Creator.CreateRocket(new Vector2(100,100));
            Creator.NewPlanetCreated += LoadNewPlanet;
            Creator.CreateNewPlanet(new Vector2(100,100));
            Controller.Init();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            LoadRocket();

            Controller.FullScreenToggled += ToggleFullScreen;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Controller.Update();

            foreach (var sprite in sprites)
            {
                sprite.Update();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Random radiusNoise = new Random();
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
            //_graphics.PreferredBackBufferWidth = GraphicsDevice.Adapter.CurrentDisplayMode.Width;
            //_graphics.PreferredBackBufferHeight = GraphicsDevice.Adapter.CurrentDisplayMode.Height;
            _graphics.IsFullScreen = !_graphics.IsFullScreen;
            _graphics.ApplyChanges();
        }

        #region LoadNewContent
        private void LoadRocket()
        {
            var rocketSprite = new Sprite(Content.Load<Texture2D>("rocket"), Rocket.Position,0.2f);
            Rocket.RocketMoved += rocketSprite.MoveSpriteTo;
            Rocket.RocketRotated += rocketSprite.Rotate;
            sprites.Add(rocketSprite);
        }

        private void LoadNewPlanet(Planet planet)
        {
            var rand = new Random();
            var planetSplite = new Sprite(Content.Load<Texture2D>("Planet" + rand.Next(1,8)), planet.Position,1.5f);
            planet.PlanetMoved += planetSplite.MoveSpriteTo;
            sprites.Add(planetSplite);
        }
        #endregion LoadNewContent
    }
}
