using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;

namespace Model
{
    public class Planet : GameObject
    {
        public float Radius;
        public bool isInSystem;

        public Planet(Vector2 position, float radius) : base(position)
        {
            this.Radius = radius;
        }

        public Planet(Rectangle rec,float radius) 
        {
            Random randomPosition = new Random();
            this.Position = new Vector2(
                randomPosition.Next(rec.X, rec.X + rec.Width),
                randomPosition.Next(rec.Y, rec.Y + rec.Height));
            this.Radius = radius;
        }

        public void SetRandomPosition(Rectangle rec)
        {
            Random randomPosition = new Random();
            this.MoveTo(
                randomPosition.Next(rec.X, rec.X + rec.Width),
                randomPosition.Next(rec.Y, rec.Y + rec.Height));

        }
    }
}
