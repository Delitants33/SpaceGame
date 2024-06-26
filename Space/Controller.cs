using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Space
{
    internal static class Controller
    {
        public static event Action FullScreenToggled;
        public static event Action RocketLaunched;
        public static event Action StartGame;
        public static event Action OnPlanetHover;
        public static event Action OnPlanetDeHover;
        public static event Action RestartGame;
        public static bool isGameStarted = false;

        public static void Update()
        {
            MouseState ms = Mouse.GetState();
            Vector2 mousePos = new Vector2(ms.X, ms.Y);

            if (!isGameStarted && Vector2.Distance(Camera.ViewportCenter, mousePos) < 155f)
            {
                OnPlanetHover();
                if (ms.LeftButton == ButtonState.Pressed)
                {
                    isGameStarted = true;
                    StartGame();
                }
            }
            if (!isGameStarted && Vector2.Distance(Camera.ViewportCenter, mousePos) > 165f)
            {
                OnPlanetDeHover();
            }
            if (Keyboard.GetState().IsKeyDown (Keys.F11)) {
                FullScreenToggled();
            }
            if(Keyboard.GetState().IsKeyDown(Keys.Space) && isGameStarted)
            {
                RocketLaunched();
            }
            if(isGameStarted && Keyboard.GetState().IsKeyDown(Keys.R))
            {
                RestartGame();
                isGameStarted = false;
            }
        }
    }
}
