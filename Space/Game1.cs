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
            
            Controller.Init();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            var rocketSprite = new Sprite(Content.Load<Texture2D>("rocket"), Rocket.Position);
            Random random = new Random();
            var randomNumber = random.Next(1, 8);
            var planetSprite = new Sprite(Content.Load<Texture2D>("Planet" + randomNumber.ToString()), new Vector2(0, 0));
            planetSprite.Scale = PlanetRenderer.SetPlanetScale();
            planetSprite.position = PlanetRenderer.SetRandomPosition(GraphicsDevice, planetSprite);
            Rocket.RocketMoved += rocketSprite.MoveSpriteTo;
            Rocket.RocketRotated += rocketSprite.Rotate;
            Controller.FullScreenToggled += ToggleFullScreen;
            sprites.Add(rocketSprite);
            sprites.Add(planetSprite);
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

            var sprite = sprites[0]; //this is a bit tricky now (i think we'll change it). But the first element of the list is the rocket
            _spriteBatch.Draw( //and all next elements are planets. And i want to draw them separately
                sprite.texture,
                sprite.position,
                null,
                Color.White,
                sprite.rotation,
                new Vector2(sprite.texture.Width / 2, sprite.texture.Height / 2), 
                0.5f,
                SpriteEffects.None,
                0f);
            
            for (int i = 1; i < sprites.Count; i++)
            {
                var planetSprite = sprites[i];
                PlanetRenderer.DrawPlanet(_spriteBatch, planetSprite);
                
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
    }
}
