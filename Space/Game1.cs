using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;

namespace Space
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteBatch _spriteBatchUI;
        private SpriteFont font;
        private Sprite startedPlanet;
        private bool isGameStarted = false;
        private bool isCutscenePlaying = false;
        List<Sprite> sprites = new();
        private Cutscene loseScene;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferHeight = 900;
            _graphics.PreferredBackBufferWidth = 1600;
            Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {
            Camera.ViewportHeight = _graphics.GraphicsDevice.Viewport.Height;
            Camera.ViewportWidth = _graphics.GraphicsDevice.Viewport.Width;
            Camera.graphics = _graphics;
            Camera.Zoom = 3f;
            Creator.NewPlanetCreated += LoadNewPlanet;
            Creator.NewRocketCreated += LoadRocket;
            Creator.NewStarCreated += LoadNewStar;
            Creator.NewAsteroidCreated += LoadNewAsteroid;
            Controller.RocketLaunched += HandleLaunch;
            Controller.StartGame += StartGame;
            Controller.OnPlanetHover += OnPlanetHover;
            Controller.OnPlanetDeHover += OnPlanetDeHover;
            Controller.RestartGame += RestartGame;
            GameManager.Initialize();
            GameManager.GameLost += StopGame;

            Trajectory.Initialize(_graphics);
            

            Camera.Follow(GameManager.planet.Position);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _spriteBatchUI = new SpriteBatch(GraphicsDevice);
            Controller.FullScreenToggled += ToggleFullScreen;
            font = Content.Load<SpriteFont>("Fonts/pixelmix");
            loseScene = new Cutscene(GraphicsDevice, Content.Load<Texture2D>("damaged"), this);
        }

        protected override void Update(GameTime gameTime)
        {
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Controller.Update();
            if (isCutscenePlaying) return;
            GameManager.Update();
            
            foreach (var sprite in sprites)
            {
                sprite.Update();
            }

            if (isGameStarted)
            {
                Camera.Zoom = MathHelper.Lerp(Camera.Zoom,1f,0.05f);
                Camera.Follow((GameManager.planet.Position + GameManager.nextPlanet.Position) / 2, 0.05f);
            }
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            if (isCutscenePlaying)
            {
                loseScene.Play(font); 
                return;
            }

            _spriteBatch.Begin(samplerState: SamplerState.PointWrap, transformMatrix: Camera.TranslationMatrix);
            if (isGameStarted)
            {
                Trajectory.DrawTrajectory(GameManager.rocket, _spriteBatch);
                Trajectory.DrawOrbit(GameManager.nextPlanet, _spriteBatch, gameTime);
            }

            foreach (var sprite in sprites)
            {
                _spriteBatch.Draw(sprite.texture,
                sprite.position,
                null,
                Color.White,
                sprite.rotation,
                new Vector2(sprite.texture.Width / 2, sprite.texture.Height / 2),
                sprite.Scale,
                SpriteEffects.None,
                1.0f);
            }

            _spriteBatch.End();

            if (isGameStarted) 
            { 
            _spriteBatchUI.Begin();
            _spriteBatchUI.DrawString(font, "Score " + GameManager.Score,
                new Vector2(_graphics.PreferredBackBufferWidth / 2 - 50, 20), Color.White);
            _spriteBatchUI.End();
            }
            base.Draw(gameTime);
        }
        #region Custom
        public void ToggleFullScreen()
        {
            _graphics.PreferredBackBufferWidth = GraphicsDevice.Adapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsDevice.Adapter.CurrentDisplayMode.Height;
            _graphics.IsFullScreen = !_graphics.IsFullScreen;
            _graphics.ApplyChanges();
        }

        private void HandleLaunch() => GameManager.Launch();

        private void StartGame() => isGameStarted = true;

        private void OnPlanetHover() => startedPlanet.Scale = 1.1f;

        private void OnPlanetDeHover() => startedPlanet.Scale = 1f;

        public void RestartGame()
        {
            isCutscenePlaying = false;
            Controller.isGameStarted = false;
            startedPlanet = null;
            sprites.Clear();
            GameManager.Reset();
            Camera.Zoom = 3f;
            Trajectory.Initialize(_graphics);
            Camera.Follow(GameManager.planet.Position);
            LoadRocket(GameManager.rocket);
            GameManager.rocket.MoveTo(150, 0);
            isGameStarted = false;
            
        }

        private void StopGame()
        {
            sprites.Clear();
            loseScene.Reset();
            isCutscenePlaying = true;
        }
        #endregion Custom

        #region LoadNewContent
        private void LoadRocket(Rocket rocket)
        {
            var rocketSprite = new Sprite(Content.Load<Texture2D>("spaceship"), rocket.Position,2f);
            rocket.ObjectMoved += rocketSprite.MoveSpriteTo;
            rocket.RocketRotated += rocketSprite.Rotate;
            sprites.Add(rocketSprite);
        }

        private void LoadNewPlanet(Planet planet)
        {
            Sprite planetSprite;
            if (planet.Position == Vector2.Zero)
                planetSprite = new Sprite(Content.Load<Texture2D>("startPlanet"), planet.Position, 1.25f);
            else
            {
                var rand = new Random();
                planetSprite = new Sprite(Content.Load<Texture2D>("Planet" + rand.Next(1, 32)), planet.Position, 1.25f);
            }
            planet.ObjectMoved += planetSprite.MoveSpriteTo;
            sprites.Add(planetSprite);
            if (startedPlanet == null)
                startedPlanet = planetSprite;
        }

        private void LoadNewStar(Star star)
        {
            var starSprite = new Sprite(Content.Load<Texture2D>("Star"), star.Position, 0.05f);
            star.ObjectMoved += starSprite.MoveSpriteTo;
            sprites.Add(starSprite);
        }

        private void LoadNewAsteroid(Asteroid asteroid)
        {
            var rand = new Random();
            var asteroidSprite = new Sprite(Content.Load<Texture2D>("Asteroid" + + rand.Next(1, 5)), asteroid.Position, 1f);
            asteroid.ObjectMoved += asteroidSprite.MoveSpriteTo;
            sprites.Add(asteroidSprite);
        }
        #endregion LoadNewContent
    }
}
