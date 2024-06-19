using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Space
{
    internal class RocketRenderer
    {
        public readonly Sprite rocketSprite;
        public EventHandler OnRotation;

        public RocketRenderer(Sprite rocketSprite)
        {
            this.rocketSprite = rocketSprite;
        }
        public void Update(Rocket rocket)
        {
            rocketSprite.MoveSpriteTo(rocket.Position);
            rocketSprite.Rotate(rocket.Rotation);
        }
        /*
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                rocketSprite.texture,
                rocketSprite.position,
                null,
                Color.White,
                MathHelper.ToRadians(rocketSprite.rotation),
                new Vector2(rocketSprite.texture.Width / 2, rocketSprite.texture.Height / 2),
                rocketSprite.Scale,
                SpriteEffects.None,
                0f);
        }*/
    }
}
