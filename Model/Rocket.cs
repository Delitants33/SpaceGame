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

        public event Action<Vector2> RocketMoved;
        public event Action<float> RocketRotated;

        public Rocket(Vector2 position, float maxSpeed) : base(position)
        {
            this.MaxSpeed = maxSpeed;
        }

        public override void MoveTo(Vector2 position)
        {
            base.MoveTo(position);
            RocketMoved(position);
        }

        public override void MoveBy(Vector2 offset)
        {
            base.MoveBy(offset);
            RocketMoved(Position);
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
    }
}
