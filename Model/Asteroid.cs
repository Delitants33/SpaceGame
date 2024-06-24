using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;

namespace Model
{
    public class Asteroid : GameObject
    {
        public float Speed { get; set; }
        public bool isClockWise {  get; set; }
        public Planet parentPlanet { get; set; }

        public Asteroid(Vector2 startPosition, float startVelocity) : base(startPosition) 
        {
            Speed = startVelocity;
            isClockWise = new Random().Next(0, 2) % 2 == 0;
        }
    }
}
