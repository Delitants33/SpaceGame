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
        public static Rocket rocket; 
        public static Planet planet { get; private set; }
        public static Planet nextPlanet { get; private set; }
        public static bool isLaunched = false;
        private static bool isClockwise = false;


        public static void Initialize() 
        {
            rocket = Creator.CreateRocket(new Vector2(150,0));
            planet = Creator.CreateNewPlanet(Vector2.Zero);
            nextPlanet = Creator.CreateNewPlanet(new Vector2(300, 200));

            rocket.OnTieToPlanet += Tie;
            rocket.OnTieToPlanet += SpawnNextPlanet;
        }

        public static void Update() 
        {
            if (isLaunched)
            {
                UpdateTrajectory();
                rocket.IsReachablePlanets(nextPlanet,planet);
            }
            else
            {
                rocket.RotateAround(planet.Position, 0.05f, isClockwise);
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

        private static void SpawnNextPlanet() 
        {
            var newPlanet = Creator.CreateNewPlanet(Vector2.Zero);
            newPlanet.SetRandomPosition(new Rectangle(
                (int)nextPlanet.Position.X + 350,
                (int)nextPlanet.Position.Y - 400,
                600,
                800));
            planet = nextPlanet;
            nextPlanet = newPlanet;
        }
     }
}
