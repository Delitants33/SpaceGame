using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace Space
{
    public static class Paralax
    {
        private const int countOfStars = 50;
        private static Texture2D starTexture;
        private static List<Rectangle> stars = new List<Rectangle>();
        private static float LastDraw = -screenWidth;
        private static float screenWidth;

        private static float[] ParalaxPower = new float[] { 0.4f, 0.44f, 0.48f, 0.52f }; 
        private static Color[] starColors = new Color[] { Color.Yellow,Color.White,Color.Aqua, Color.Wheat};

        private static Vector2 BGSize = new Vector2(100,2500);

        public static void Initialize(GraphicsDeviceManager graphics)
        {
            screenWidth = graphics.PreferredBackBufferWidth;
            starTexture = new Texture2D(graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            starTexture.SetData(new[] { Color.LightGray });
            LastDraw = -screenWidth * 2;
            DrawNewStars();
        }

        public static void DrawNewStars()
        {
            var rectangle = new Rectangle(new Point((int)LastDraw,0) + new Point(0, - (int)BGSize.Y/2), BGSize.ToPoint());
            for (int i = 0; i < countOfStars; i++)
            {
                Random rnd = new Random();
                int starSize = rnd.Next(1, 3);
                var rect = new Rectangle(
                    rnd.Next(rectangle.X, rectangle.X + rectangle.Width),
                    rnd.Next(rectangle.Y, rectangle.Y + rectangle.Height),
                    starSize, starSize);
                stars.Add(rect);
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            Random rnd = new Random();
            for (int i = 0; i < stars.Count; i++)
            {
                spriteBatch.Draw(starTexture,
                         new Rectangle(stars[i].X - (int)(Camera.Position.X * ParalaxPower[i % ParalaxPower.Length]),
                         stars[i].Y - (int)(Camera.Position.Y * ParalaxPower[i % ParalaxPower.Length]),
                         stars[i].Width, stars[i].Height),
                         starColors[i%starColors.Length]);
            }
        }

        public static void CheckToDrawNewStars()
        {
            if (Camera.Position.X > LastDraw + BGSize.X - 600 - screenWidth * 1.5)
            {
                LastDraw += BGSize.X;
                DrawNewStars();
            }
        }
    }
}
