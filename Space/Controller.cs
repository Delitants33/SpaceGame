using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Model;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;

namespace Space
{
    internal static class Controller
    {
        private static Rocket rocket;
        static float RotationSpeed = 0.01f;

        public static void Init()
        {
            rocket = Creator.Rocket;
        }

        public static void Update()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                rocket.MoveBy(new Vector2(1,0) * rocket.MaxSpeed);
            }
            if (Keyboard.GetState().IsKeyDown (Keys.Q)) {
                rocket.RotateBy(RotationSpeed);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.E))
            {
                rocket.RotateBy(-RotationSpeed);
            }
        }
    }
}
