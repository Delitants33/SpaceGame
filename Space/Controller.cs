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
        }
    }
}
