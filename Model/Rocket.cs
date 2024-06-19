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
    public class Rocket
    {
        public readonly float MaxSpeed; 
        public Vector2 Position { get; private set; }
        public float Rotation { get; private set; } // angles in degrees not radians

        public event Action<Vector2> RocketMoved;
        public event Action<float> RocketRotated;

        public Rocket(Vector2 position, float maxSpeed)
        {
            this.Position = position;
            this.MaxSpeed = maxSpeed;
        }

        public void MoveTo(Vector2 position)
        {
            this.Position = position;
            RocketMoved(position);
        }

        public void MoveBy(Vector2 offset)
        {
            this.Position += offset;
            RocketMoved(Position);
        }

        public void RotateBy(float angle)
        {
            this.Rotation += angle % 360;
            Debug.Print($"{this.Rotation}");
            RocketRotated(this.Rotation);
        }

        public void RotateTo(float angle)
        {
            this.Rotation = angle % 360;
            RocketRotated(this.Rotation);
        }
    }
}
