using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;

namespace Space
{
    public class Sprite
    {
        public Texture2D texture;
        public Vector2 position;
        public float rotation;
        public float Scale  = 1f;

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
            this.rotation = 0;
            this.position = position;
            this.Scale = Scale;
        }

        public Sprite(Texture2D texture, Vector2 position, float rotation,  float Scale = 1)
        {
            this.texture = texture;
            this.rotation = rotation;
            this.position = position;
            this.Scale = Scale;
        }

        public void MoveSpriteTo(Vector2 position)
        {
            this.position = position;
        }

        public void Rotate(float angle)
        {
            rotation = angle;
        }

        public virtual void Update() { }
    }
}
