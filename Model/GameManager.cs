using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using static System.Formats.Asn1.AsnWriter;


namespace Model
{
    public static class GameManager
    {
        public static Rocket rocket; 
        public static Planet planet { get; private set; }
        public static Planet nextPlanet { get; private set; }
        private static Star lastStar;
        private static List<Asteroid> Asteroids = new List<Asteroid>();
        public static bool isLaunched = false;
        private static bool isClockwise = false;
        public static int Score { get; private set; }
        private static int timer = 0;

        public static event Action GameLost;

        public static void Initialize() 
        {
            rocket = Creator.CreateRocket(new Vector2(150,0));
            planet = Creator.CreateNewPlanet(Vector2.Zero);
            nextPlanet = Creator.CreateNewPlanet(new Vector2(400, 200));
            Score = 0;

            rocket.OnTieToPlanet += Tie;
            rocket.OnTieToPlanet += SpawnNextPlanet;
            rocket.OnTieToPlanet += TryToCreateStar;
            rocket.OnTieToPlanet += TryToCreateAsteroid;
        }

        public static void Update() 
        {
            if (CheckStarCollect())
                Score++;
            if (CheckAsteroidCrush())
                GameLost();
            if (isLaunched)
            {
                UpdateTrajectory();
                rocket.IsReachablePlanets(nextPlanet);
                if (IsLose()) 
                    GameLost();
            }
            else
            {
                timer = 0;
                rocket.RotateAround(planet.Position, 0.05f,isClockwise);
            }
            foreach (var asteroid in Asteroids)
                asteroid.RotateAround(asteroid.parentPlanet.Position, asteroid.Speed,asteroid.isClockWise);
        }

        public static void Launch() => isLaunched = true;

        private static void Tie() 
        {
            Score++;
            isLaunched = false; 
        }

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
            var newPlanet = Creator.CreateNewPlanet(new Rectangle(
                (int)nextPlanet.Position.X + 350,
                (int)nextPlanet.Position.Y - 400,
                600,
                800));
            planet = nextPlanet;
            nextPlanet = newPlanet;
        }

        private static bool IsLose()
        {
            timer++;
            return timer > 200;
        } 
       
        public static void Reset()
        {
            rocket = Creator.CreateRocket(new Vector2(150, 0));
            planet = Creator.CreateNewPlanet(Vector2.Zero);
            nextPlanet = Creator.CreateNewPlanet(new Vector2(400, 200));
            timer = 0;
            Score = 0;
            isLaunched = false;
        }

        public static void TryToCreateStar()
        {
            Random rand = new Random();
            if (rand.Next(1, 100) < 40)
            {
                float starDistance = rand.Next(33, 66) / 100f;
                lastStar = Creator.CreateNewStar(planet.Position * starDistance + nextPlanet.Position * (1 - starDistance)
                    + new Vector2(((float)rand.NextDouble()-0.5f) * 15, (float)rand.NextDouble() - 0.5f) * 15);
            }
        }

        public static void TryToCreateAsteroid()
        {
            Random rand = new Random();
            if (rand.Next(1, 100) < 60)
            {
                Asteroids.Add( Creator.CreateNewAsteroid(nextPlanet.Position + 
                    new Vector2(nextPlanet.Radius + (float)rand.Next(50,120),0)
                    ,rand.Next(2,50)/1000f
                    ,nextPlanet));
            }
        }

        public static bool CheckStarCollect()
        {
            if (lastStar != null && Vector2.Distance(lastStar.Position, rocket.Position) < 50f)
            {
                lastStar.MoveTo( new Vector2(-1000, -1000));
                lastStar = null;
                return true;    
            }
            return false;
        }

        public static bool CheckAsteroidCrush() 
        {
            foreach (var asteroid in Asteroids)
            {
                if(Vector2.Distance(asteroid.Position, rocket.Position) < 35)
                    return true;
            }
            return false;
        }
    }
}
