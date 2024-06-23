using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;

namespace Model
{
    public static class Creator
    {
        public static Rocket Rocket { get; private set; }
        public static List<Planet> Planets { get; private set; } = new List<Planet>();

        public static event Action<Planet> NewPlanetCreated;
        public static event Action<Rocket> NewRocketCreated;

        public static Rocket CreateRocket(Vector2 startPosition,float maxSpeed = 8)
        {
            if (Rocket == null)
            {
                Rocket = new Rocket(startPosition, maxSpeed);
                NewRocketCreated(Rocket);
                return Rocket;
            }
            else
                return Rocket;
        }

        public static Planet CreateNewPlanet(Vector2 startPosition, float radius = 150)
        {
            var newPlanet = new Planet(startPosition, radius);
            Planets.Add(newPlanet);
            NewPlanetCreated(newPlanet);
            return newPlanet;
        }
        
    }
} 