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
    public static class Camera
    {
        public static Vector2 Position { get; set; }
        public static GraphicsDeviceManager graphics;
        public static float Zoom { get; set; }

        public static int ViewportWidth { get; set; }
        public static int ViewportHeight { get; set; }

        public static Vector2 ViewportCenter
        {
            get => new Vector2(ViewportWidth /2f, ViewportHeight / 2f);
        }

        public static Matrix TranslationMatrix
        {
            get => Matrix.CreateTranslation(new Vector3(-Position.X - ViewportWidth / 2, -Position.Y - ViewportHeight / 2, 0)) * 
                Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) * 
                Matrix.CreateTranslation(new Vector3(ViewportWidth / 2, ViewportHeight / 2, 0)); 
        }
        
        public static void Follow(Vector2 position, float followSpeed = 0.2f)
        {       
            Position = Vector2.Lerp(
                Position,
                position - new Vector2(
                graphics.PreferredBackBufferWidth / 2,
                graphics.PreferredBackBufferHeight / 2),
                followSpeed);
        }

        public static void Follow(Vector2 position) => Follow(position, 1f);
    }
}
