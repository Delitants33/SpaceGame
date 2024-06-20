﻿using Microsoft.Xna.Framework;
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
        private static Planet nextPlanet; //temporary
        public static bool isLaunched = false;


        public static void Initialize() 
        {
            rocket = Creator.CreateRocket(new Vector2(100,100));
            planet = Creator.CreateNewPlanet(new Vector2(0,0));
            nextPlanet = Creator.CreateNewPlanet(new Vector2(0, 0));
            planet.SetRandomPosition(new Rectangle(100, 100, 300, 300));
            nextPlanet.SetRandomPosition(new Rectangle(150, 100, 600, 350));
            rocket.OnTieToPlanet += Launch;
            

        }

        public static void Update() 
        {
            if (!isLaunched)
            {
                rocket.RotateAround(planet.Position, 0.05f);
            }
            else
            {
                rocket.Launch(10f);
                rocket.CheckIfReachablePlanets(ref nextPlanet, ref planet); //probably will be foreach planet. or list of planets will be passed. UPD: awful name after i added second plane to the func
                
            }

        }

        public static void Launch()
        {
            isLaunched = !isLaunched;
        }
    }
}