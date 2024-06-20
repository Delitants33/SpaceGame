using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;


namespace Model
{
    public static class GameManager
    {
        private static Rocket rocket; // temporaly or not)
        private static Planet planet; // temporaly or not)

        public static void Initialize() 
        {
            rocket = Creator.CreateRocket(new Vector2(100,100));
            planet = Creator.CreateNewPlanet(new Vector2(0,0));
            planet.SetRandomPosition(new Rectangle(100, 100, 300, 300));
        }

        public static void Update() 
        {
            rocket.RotateAround(planet.Position, 0.05f);
        }
    }
}
