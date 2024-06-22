using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;
namespace Space
{
    internal static class Trajectory {
        private static Texture2D line;
        private const int dashLength = 30;
        private const int gap = 15;
        private const int trajectoryLength = 1000;
        private const int lineThickness = 3;
        private const int orbitDashesCount = 20;
        private const int dashesInOrbit = 10;
        public static void Initialize(GraphicsDeviceManager graphics)
        {
            line = new Texture2D(graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            line.SetData(new[] { Color.LightGray });
        }
        public static void DrawTrajectory(Rocket rocket, GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            float rotation = rocket.Rotation - MathHelper.PiOver2;
            int dashesCount = trajectoryLength / (dashLength + gap);
            for (int i = 0; i < dashesCount; i++)
            {
                float dashStartX = rocket.Position.X + i * (dashLength + gap) * (float)Math.Cos(rotation);
                float dashStartY = rocket.Position.Y + i * (dashLength + gap) * (float)Math.Sin(rotation);
                Rectangle lineRectangle = new Rectangle(
                    (int)(dashStartX - Camera.Position.X),
                    (int)(dashStartY - Camera.Position.Y),
                    dashLength,
                    lineThickness);

                spriteBatch.Draw(line, lineRectangle, null, Color.Gray, rotation, Vector2.Zero, SpriteEffects.None, 0.1f);
            }
        }
        public static void DrawOrbit(Planet planet, SpriteBatch spriteBatch, GameTime gameTime)
        {
            float rotationSpeed = 0.1f;
            for (int i = 0; i < orbitDashesCount; i++)
            {
                for (int j = 0; j < dashesInOrbit * 2; j++)
                {
                    float angle = j * MathHelper.Pi / dashesInOrbit + (float)gameTime.TotalGameTime.TotalSeconds * rotationSpeed + 0.01f * i;
                    float dashStartX = planet.Position.X + (float)Math.Cos(angle) * planet.Radius;
                    float dashStartY = planet.Position.Y + (float)Math.Sin(angle) * planet.Radius;
                    Rectangle lineRectangle = new Rectangle(
                            (int)(dashStartX - Camera.Position.X),
                            (int)(dashStartY - Camera.Position.Y),
                            lineThickness,
                            lineThickness);

                    spriteBatch.Draw(line, lineRectangle, null, Color.Gray, 0, Vector2.Zero, SpriteEffects.None, 0.1f);
                }
            }
        }
    }
}
