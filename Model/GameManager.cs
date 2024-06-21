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
        private static Planet nextPlanet;
        public static bool isLaunched = false;
        private static bool isClockwise = false;

        public static void Initialize() 
        {
            rocket = Creator.CreateRocket(new Vector2(50,800));
            planet = Creator.CreateNewPlanet(new Vector2(200,800));
            nextPlanet = Creator.CreateNewPlanet(Vector2.Zero);
            nextPlanet.SetRandomPosition(new Rectangle(
                (int)nextPlanet.Position.X + 100,
                (int)nextPlanet.Position.Y + 400,
                800,
                500)); 
            rocket.OnTieToPlanet += Tie;
            rocket.OnTieToPlanet += GetNextPlanet;
        }

        public static void Update() 
        {
            if (!isLaunched)
            {
                    rocket.RotateAround(planet.Position, 0.05f, isClockwise);
            }
            else
            {
                UpdateTrajectory();
                rocket.IsReachablePlanets(ref nextPlanet, ref planet);
                
            }

        }

        public static void Launch() => isLaunched = true;

        private static void Tie() => isLaunched = false;

        private static bool IsClockwise(Vector2 planetPos, Vector2 rocketPos, Vector2 velocity )
        {
            Vector2 relativePos = planetPos - rocketPos;
            float crossProductZ = relativePos.X * velocity.Y - relativePos.Y * velocity.X;
            return crossProductZ < 0;
        }
        private static void UpdateTrajectory()
        {
            rocket.Launch(rocket.MaxSpeed);
            isClockwise = IsClockwise(nextPlanet.Position, rocket.Position, rocket.velocity);
        }

        //TO DO
        private static void GetNextPlanet() // this method is very incomplete due to lack of time. 
        {

            var newPlanet = Creator.CreateNewPlanet(Vector2.Zero);
            newPlanet.SetRandomPosition(new Rectangle(
                (int)nextPlanet.Position.X - 400,
                (int)nextPlanet.Position.Y - 300,
                800,
                400));
            planet = nextPlanet;
            nextPlanet = newPlanet;    
        }
     }
}
