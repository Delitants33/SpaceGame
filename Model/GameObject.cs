using Microsoft.Xna.Framework;
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

        public GameObject (Vector2 startPosition)
        {
            Position = startPosition;
        }

        public virtual void MoveTo(Vector2 position)
        {
            this.Position = position;
        }

        public virtual void MoveBy(Vector2 offset)
        {
            this.Position += offset;
        }
    }
}
