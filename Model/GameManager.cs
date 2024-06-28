using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
        public static List<Planet> activePlanets = new();
        public static List<List<Planet>> planetSystems = new();
        private static Star lastStar;
        private static List<Asteroid> Asteroids = new List<Asteroid>();
        public static bool isLaunched = false;
        private static bool isClockwise = false;
        public static int Score { get; private set; }
        private static int timer = 0;
        private static bool isFirstTime = true;

        public static event Action GameLost;
        public static event Action StarCollected;
        public static event Action OnLaunch;

        public static void Initialize()
        {
            rocket = Creator.CreateRocket(new Vector2(150, 0));
            planet = Creator.CreateNewPlanet(Vector2.Zero);
            nextPlanet = Creator.CreateNewPlanet(new Vector2(400, 200));
            activePlanets.Add(nextPlanet);
            Score = 0;

            rocket.OnTieToPlanet += Tie;
            rocket.OnTieToPlanet += SpawnNextPlanet;
            rocket.OnTieToPlanet += TryToCreateStar;
            rocket.OnTieToPlanet += TryToCreateAsteroid;
        }

        public static void Update()
        {
            if (CheckStarCollect())
            {
                StarCollected();
                Score++;
            }
            if (CheckAsteroidCrush() || CheckPlanetCrash())
                GameLost();

            foreach (var rotatingPlanets in planetSystems)
            {
                Vector2 centerPosition = (rotatingPlanets[0].Position + rotatingPlanets[1].Position) / 2;
                foreach (var rotatingPlanet in rotatingPlanets)
                {
                    rotatingPlanet.RotateAround(centerPosition, 0.02f, true);
                }
            }
            if (isLaunched)
            {
                UpdateTrajectory();
                foreach (var planet in activePlanets)
                {
                    var p = rocket.IsReachablePlanets(planet);
                    if (p != null) break;
                }

                if (IsLose())
                    GameLost();
                if (isFirstTime)
                {
                    OnLaunch();
                    isFirstTime = false;
                }
            }
            else
            {
                isFirstTime = true;
                timer = 0;
                if (planet.isInSystem)
                {
                    rocket.MoveTo(rocket.Position + planet.RotatedBy);
                }
                rocket.RotateAround(planet.Position, 0.05f, isClockwise);
            }
            foreach (var asteroid in Asteroids)
                asteroid.RotateAround(asteroid.parentPlanet.Position, asteroid.Speed, asteroid.isClockWise);
        }

        public static void Launch() => isLaunched = true;

        private static void Tie()
        {
            Score++;
            isLaunched = false;
        }

        private static bool IsClockwise(Vector2 planetPos, Vector2 rocketPos, Vector2 velocity)
        {
            Vector2 relativePos = planetPos - rocketPos;
            float crossProductZ = relativePos.X * velocity.Y - relativePos.Y * velocity.X;
            return crossProductZ < 0;
        }

        private static void UpdateTrajectory()
        {
            rocket.Launch(rocket.MaxSpeed);
            var planet = nextPlanet;
            if (activePlanets.Count > 1)
            {
                planet = Vector2.Distance(rocket.Position, activePlanets[0].Position) > Vector2.Distance(rocket.Position, activePlanets[1].Position)
                            ? activePlanets[1]
                            : activePlanets[0];
            } 
            isClockwise = IsClockwise(planet.Position, rocket.Position, rocket.velocity);
        }

        public static void SpawnNextPlanet()
        {
            Random rand = new Random();
            if (rand.Next(1, 11) == 1)
            {
                CreatePlanetarySystem();
            }
            else
            {
                var newPlanet = Creator.CreateNewPlanet(new Rectangle(
                    (int)nextPlanet.Position.X + 450,
                    (int)nextPlanet.Position.Y - 400,
                    600,
                    800));
                if (activePlanets.Count == 1)
                {
                    planet = activePlanets[0];
                }
                else
                {
                    planet = Vector2.Distance(rocket.Position, activePlanets[0].Position) > Vector2.Distance(rocket.Position, activePlanets[1].Position)
                        ? activePlanets[1]
                        : activePlanets[0];
                }

                nextPlanet = newPlanet;
                activePlanets.Clear();
                activePlanets.Add(newPlanet);
            }
        }

        private static void CreatePlanetarySystem()
        {
            
            var centralPoint = nextPlanet.Position;
            if (activePlanets.Count > 1)
            {
                centralPoint = (activePlanets[0].Position + activePlanets[1].Position) / 2;
            }
            Planet planet1 = Creator.CreateNewPlanet(new Rectangle(
                    (int)centralPoint.X + 550,
                    (int)centralPoint.Y - 400,
                    600,
                    800));
            Planet planet2 = Creator.CreateNewPlanet(planet1.Position + new Vector2(350, 0));
            planet1.isInSystem = true;
            planet2.isInSystem = true;
            if (activePlanets.Count == 1)
            {
                planet = activePlanets[0];
            }
            else
            {
                planet = Vector2.Distance(rocket.Position, activePlanets[0].Position) > Vector2.Distance(rocket.Position, activePlanets[1].Position)
                    ? activePlanets[1]
                    : activePlanets[0];
            }
            if (!planet.isInSystem)
            {
                planetSystems.Clear();
            }
            nextPlanet = planet1;
            activePlanets.Clear();
            activePlanets.Add(planet1);
            activePlanets.Add(planet2);
            planetSystems.Add(new List<Planet> { planet1, planet2 });
        }

        private static bool IsLose()
        {
            timer++;
            if (rocket.velocity.X < 0)
            {
                return timer > 45;
            }
            return timer > 200;
        }

        public static void Reset()
        {
            activePlanets.Clear();
            planetSystems.Clear();
            rocket = Creator.CreateRocket(new Vector2(150, 0));
            planet = Creator.CreateNewPlanet(Vector2.Zero);
            nextPlanet = Creator.CreateNewPlanet(new Vector2(400, 200));
            activePlanets.Add(nextPlanet);
            Asteroids.Clear();
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
                    + new Vector2(((float)rand.NextDouble() - 0.5f) * 15, (float)rand.NextDouble() - 0.5f) * 15);
            }
        }

        public static void TryToCreateAsteroid()
        {
            Random rand = new Random();
            if (activePlanets.Count > 1) return;
            if (rand.Next(1, 100) < 60)
            {
                Asteroids.Add(Creator.CreateNewAsteroid(nextPlanet.Position +
                    new Vector2(nextPlanet.Radius + (float)rand.Next(50, 120), 0)
                    , rand.Next(2, 45) / 1000f
                    , nextPlanet));
            }
        }

        public static bool CheckStarCollect()
        {
            if (lastStar != null && Vector2.Distance(lastStar.Position, rocket.Position) < 50f)
            {
                lastStar.MoveTo(new Vector2(-1000, -1000));
                lastStar = null;
                return true;
            }
            return false;
        }

        public static bool CheckAsteroidCrush()
        {
            foreach (var asteroid in Asteroids)
            {
                if (Vector2.Distance(asteroid.Position, rocket.Position) < 35)
                    return true;
            }
            return false;
        }
        private static bool CheckPlanetCrash()
        {
            foreach (var systems in planetSystems)
            {
                foreach (var planet in systems) { 
                    if (Vector2.Distance(planet.Position, rocket.Position) < 75) return true;
                }
            }
            return false;
        }
    }
}
