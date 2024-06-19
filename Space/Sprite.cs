using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;

namespace Space
{
    internal class Sprite
    {
        public Texture2D texture;
        public Vector2 position;
        public float Scale  = 1;

        public Rectangle Rectangle 
        {
            get => new Rectangle((int)position.X,
                    (int)position.Y,
                    (int)(texture.Width * Scale),
                    (int)(texture.Height * Scale));
        }

        public Sprite(Texture2D texture, Vector2 position, float Scale = 1)
        {
            this.texture = texture;
            this.position = position;
            this.Scale = Scale;
        }

        public void MoveSpriteTo(Vector2 position)
        {
            this.position = position;
        }

        public virtual void Update() 
        {
        }
    }
}
