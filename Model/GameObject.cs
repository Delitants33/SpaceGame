﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;

namespace Model
{
    public class GameObject
    {
        public Vector2 Position { get; protected set; }
        public event Action<Vector2> ObjectMoved;
        public Vector2 RotatedBy { get; protected set; }


        public GameObject() : this(Vector2.Zero)
        {
        }

        public GameObject (Vector2 startPosition)
        {
            Position = startPosition;   
        }

        public virtual void MoveTo(Vector2 position)
        {
            this.Position = position;
            ObjectMoved(Position);
        }

        public virtual void MoveTo(float x, float y) => MoveTo(new Vector2(x, y));

        public virtual void MoveBy(Vector2 offset)
        {
            this.Position += offset;
            ObjectMoved(Position);
        }

        public virtual void RotateAround(Vector2 center, float rotationSpeed, bool isClockWise = true)
        {
            var initialPosition = this.Position; 

            var distanceToCenter = Vector2.Distance(center, Position);
            var angle = Math.Atan2(this.Position.Y - center.Y, this.Position.X - center.X);
            angle = isClockWise ?
                angle + rotationSpeed :
                angle - rotationSpeed;
            var ToObject = new Vector2((float)Math.Cos(angle) * distanceToCenter, (float)Math.Sin(angle) * distanceToCenter);
            this.MoveTo(ToObject + center);
            this.RotatedBy = this.Position - initialPosition; 

        }

    }
}
