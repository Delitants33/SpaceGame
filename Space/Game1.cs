﻿using Microsoft.Xna.Framework;
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
            RocketRenderer rocketRender = new RocketRenderer(rocketSprite);
            Rocket.RocketMoved += rocketSprite.MoveSpriteTo;
            Rocket.RocketRotated += rocketSprite.Rotate;
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
                _spriteBatch.Draw(
                    sprite.texture,
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
    }
}
