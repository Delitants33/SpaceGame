using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;

namespace Model
{
    public class Rocket : GameObject
    {
        public readonly float MaxSpeed; 
        public float Rotation { get; private set; } // angles in degrees not radians
        public Vector2 velocity;

        public event Action<float> RocketRotated;
        public event Action OnTieToPlanet;

        public Rocket(Vector2 position, float maxSpeed) : base(position)
        {
            this.MaxSpeed = maxSpeed;
        }

        public void RotateBy(float angle)
        {
            this.Rotation += angle % 360;
            RocketRotated(this.Rotation);
        }

        public void RotateTo(float angle)
        {
            this.Rotation = angle % 360;
            RocketRotated(this.Rotation);
        }

        public override void RotateAround(Vector2 center, float rotationSpeed)
        {
            base.RotateAround(center, rotationSpeed);
            var toCenter = center - this.Position;
            var normalToCenter = new Vector2(-toCenter.Y, toCenter.X);
            normalToCenter.Normalize();
            normalToCenter *= Vector2.Distance(center, Position);
            Rotation = (float)Math.Atan2(normalToCenter.Y, normalToCenter.X) + 200;
            RocketRotated(Rotation);
        }
        public override void RotateAroundCounterClockwise(Vector2 center, float rotationSpeed)
        {
            base.RotateAround(center, -rotationSpeed);
            var toCenter = center - this.Position;
            var normalToCenter = new Vector2(toCenter.Y, -toCenter.X);
            normalToCenter.Normalize();
            normalToCenter *= Vector2.Distance(center, Position);
            Rotation = (float)Math.Atan2(normalToCenter.Y, normalToCenter.X) -MathHelper.PiOver2;
            RocketRotated(Rotation);
        }

        public void Launch(float speed) {
            velocity = new Vector2((float)Math.Cos(Rotation + 80), (float)Math.Sin(Rotation + 80));  //no clue where this 80 came from. 
            MoveBy(velocity * speed);

        }

        public void CheckIfReachablePlanets(ref Planet planet, ref Planet previousPlanet) 
        {
            if (Vector2.Distance(this.Position, planet.Position) < planet.Radius-1)
            {
                (planet, previousPlanet) = (previousPlanet, planet);
                velocity = new Vector2(0, 0);
                OnTieToPlanet();
            };
        }



    }
}
