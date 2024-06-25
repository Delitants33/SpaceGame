using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;

namespace Space
{
    internal class Cutscene
    {
        private SpriteBatch spriteBatch;
        private GraphicsDevice graphicsDevice;
        private Texture2D texture;
        private Vector2 originalPos;
        private const float rotationSpeed = 0.05f;
        private float rotation = 0f;
        private float timer = 0f;
        private string message = "";
        private float messageOffset = 0;
        private Game1 game;

        public Cutscene(GraphicsDevice graphics, Texture2D ship, Game1 game)
        {
            spriteBatch = new SpriteBatch(graphics);
            graphicsDevice = graphics;
            int screenWidth = graphicsDevice.PresentationParameters.BackBufferWidth;
            int screenHeight = graphicsDevice.PresentationParameters.BackBufferHeight;
            originalPos = new Vector2(screenWidth / 2f - 1000, screenHeight / 2f - 50);
            this.game = game;
            texture = ship;
            
        }

        public void Play(SpriteFont font)
        {
            Update();
            spriteBatch.Begin(samplerState: SamplerState.PointWrap);
            spriteBatch.Draw(
                texture,
                originalPos,
                null,
                Color.White,
                rotation,
                new Vector2(texture.Width / 2f, texture.Height / 2f),
                6f,
                SpriteEffects.None,
                0f);
            spriteBatch.DrawString(
                font,
                message,
                new Vector2(graphicsDevice.PresentationParameters.BackBufferWidth / 2 - messageOffset, graphicsDevice.PresentationParameters.BackBufferHeight / 2 - 70),
                Color.DarkRed,
                0f,
                Vector2.Zero,
                1.8f,
                SpriteEffects.None,
                0f);
            spriteBatch.End(); 
        }

        private void Update()
        {
            Camera.Follow(originalPos + new Vector2(0.1f, 0.1f));
            string mes = "You have lost";
            timer += 1f;
            int screenWidth = graphicsDevice.PresentationParameters.BackBufferWidth;
            int screenHeight = graphicsDevice.PresentationParameters.BackBufferHeight;
            originalPos = Vector2.Lerp(originalPos, new Vector2(screenWidth / 2f + 1150, screenHeight / 2f - 50), 0.015f);
            rotation = rotationSpeed * timer;
            if (timer % 10 == 0)
            {
                int len = (int)timer / 10;
                if (len > mes.Length)
                {
                    len = mes.Length;
                }
                else
                {
                    messageOffset += 17;
                }
                message = mes[..len];
            }
            if (Vector2.Distance(originalPos, new Vector2(screenWidth / 2f + 1150, screenHeight / 2f - 50)) < 70f)
            {
                game.RestartGame();
            }
        }
        public void Reset()
        {
            int screenWidth = graphicsDevice.PresentationParameters.BackBufferWidth;
            int screenHeight = graphicsDevice.PresentationParameters.BackBufferHeight;
            originalPos = new Vector2(screenWidth / 2f - 900, screenHeight / 2f - 50);
            message = "";
            timer = 0;
            messageOffset = 0;
        }
    }
}
