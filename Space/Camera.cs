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
    public static class Camera
    {
        public static Vector2 Position { get; set; }
        public static GraphicsDeviceManager graphics;

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
