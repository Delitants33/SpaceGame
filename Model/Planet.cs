using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;

namespace Model
{
    public class Planet
    {
        public Vector2 Position { get; private set; }
        public float Radius;

        public event Action<Vector2> PlanetMoved;

        public Planet(Vector2 position, float radius) 
        {
            this.Position = position;
            this.Radius = radius;
        }

        public void MoveTo(Vector2 position)
        {
            this.Position = position;
            PlanetMoved(position);
        }

        public void MoveBy(Vector2 offset)
        {
            this.Position += offset;
            PlanetMoved(Position);
        }
    }
}
