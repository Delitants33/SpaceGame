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
        public event Action<Vector2> PlanetMoved;

        public Planet(Vector2 position, float radius) : base(position)
        {
            this.Radius = radius;
        }

        public void SetRandomPosition(Rectangle rec)
        {
            Random randomPosition = new Random();
            this.Position = new Vector2(
                randomPosition.Next(rec.X, rec.X + rec.Width),
                randomPosition.Next(rec.Y, rec.Y + rec.Height));
        }

        public override void MoveTo(Vector2 position)
        {
            base.MoveTo(position);
            PlanetMoved(position);
        }

        public override void MoveBy(Vector2 offset)
        {
            base.MoveBy(offset);
            PlanetMoved(Position);
        }
    }
}
