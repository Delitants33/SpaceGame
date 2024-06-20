using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Space
{
    internal static class PlanetRenderer
    {
        static float planetRadius = 2.5f;
        public static float SetPlanetScale()
        {
            Random radiusNoise = new Random();
            return planetRadius + (float)radiusNoise.Next(0, 3) / 2;
        }


        public static void DrawPlanet(SpriteBatch spriteBatch, Sprite planet)
        {
            spriteBatch.Draw(
                    planet.texture,
                    planet.position,
                    null,
                    Color.White,
                    planet.rotation,
                    new Vector2(planet.texture.Width / 2, planet.texture.Height / 2),
                    planet.Scale,
                    SpriteEffects.None,
                    0f);
        }
    }
}
