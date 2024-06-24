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
    public class Star : GameObject
    {
        private float amplitude = 10f;
        private float timer = 0f;
        public event Action<Star> StarDestroyed;

        public Star(Vector2 position) : base(position) { }

        public void Shake()
        {
            MoveBy(new Vector2(this.Position.X,this.Position.Y + (float)Math.Sin(timer) * amplitude));
            timer++;
        }

        public void Destroy()
        {
            StarDestroyed(this);
        }
    }
}
