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

        public event Action<float> RocketRotated;

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
            var normalToCenter = new Vector2(-toCenter.Y,toCenter.X);
            Rotation = (float)Math.Atan2(normalToCenter.Y, normalToCenter.X) + 200;
            RocketRotated(Rotation);
        }
    }
}
