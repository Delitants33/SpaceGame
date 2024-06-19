using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Model;
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
        }

        protected override void Initialize()
        {
            Rocket = Creator.CreateRocket(new Vector2(0,0));
            Controller.Init();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            var rocketSprite = new Sprite(Content.Load<Texture2D>("rocket"), new Vector2(0,0));
            Rocket.RocketMoved += rocketSprite.MoveSpriteTo;
            sprites.Add(rocketSprite);
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
            GraphicsDevice.Clear(Color.Bisque);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            foreach (var sprite in sprites)
            {
                _spriteBatch.Draw(sprite.texture, sprite.Rectangle, Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
