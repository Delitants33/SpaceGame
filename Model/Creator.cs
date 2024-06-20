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

        public static Rocket CreateRocket(Vector2 startPosition,float maxSpeed = 5)
        {
            if (Rocket == null)
            {
                Rocket = new Rocket(startPosition, maxSpeed);
                return Rocket;
            }
            else
                return Rocket;
        }

        public static Planet CreateNewPlanet(Vector2 startPosition, float radius = 100) 
        {
            var newPlanet = new Planet(startPosition, radius);
            Planets.Add(newPlanet);
            NewPlanetCreated(newPlanet);
            return newPlanet;
        }
    }
}
