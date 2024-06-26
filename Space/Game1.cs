using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
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
        private SpriteBatch _spriteBatchBG;
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
            GameManager.GameLost += PlayDeathSound;
            GameManager.StarCollected += PlayStarRecieveSound;
            GameManager.OnLaunch += PlayLaunchSound;

            Paralax.Initialize(_graphics);
            Trajectory.Initialize(_graphics);
            
            Camera.Follow(GameManager.planet.Position);
            base.Initialize();
            SoundManager.PlaySFX(SoundManager.gameStart);
            SoundManager.PlaySong(SoundManager.ambience);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _spriteBatchUI = new SpriteBatch(GraphicsDevice);
            _spriteBatchBG = new SpriteBatch(GraphicsDevice);
            Controller.FullScreenToggled += ToggleFullScreen;
            font = Content.Load<SpriteFont>("Fonts/pixelmix");
            loseScene = new Cutscene(GraphicsDevice, Content.Load<Texture2D>("damaged"), this);
            LoadAllSounds();
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
                _spriteBatchBG.Begin();
                Paralax.Draw(_spriteBatchBG);
                loseScene.Play(font);
                _spriteBatchBG.End();
                return;
            }
            _spriteBatchBG.Begin();
            Paralax.Draw(_spriteBatchBG);
            Paralax.CheckToDrawNewStars();
            _spriteBatchBG.End();

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

        private void PlayTieSound() => SoundManager.PlaySFX(SoundManager.Tie);
        private void PlayLaunchSound() => SoundManager.PlaySFX(SoundManager.launch);
        private void PlayDeathSound() => SoundManager.PlaySFX(SoundManager.death);
        private void PlayStarRecieveSound() => SoundManager.PlaySFX(SoundManager.starRecieve);

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
            GameManager.rocket.velocity = Vector2.Zero;
            Trajectory.dashes.Clear();
            isGameStarted = false;
            
        }

        private void StopGame()
        {
            sprites.Clear();
            loseScene.Reset();
            GameManager.rocket.velocity = Vector2.Zero;
            Trajectory.dashes.Clear();
            isCutscenePlaying = true;
        }
        #endregion Custom

        #region LoadNewContent
        private void LoadRocket(Rocket rocket)
        {
            var rocketSprite = new Sprite(Content.Load<Texture2D>("spaceship"), rocket.Position,2f);
            rocket.ObjectMoved += rocketSprite.MoveSpriteTo;
            rocket.RocketRotated += rocketSprite.Rotate;
            rocket.OnTieToPlanet += PlayTieSound;
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

        private void LoadAllSounds()
        {
            SoundManager.ambience = Content.Load<Song>("Audio/ambience");
            SoundManager.launch = Content.Load<SoundEffect>("Audio/launch");
            SoundManager.Tie= Content.Load<SoundEffect>("Audio/Tie");
            SoundManager.death = Content.Load<SoundEffect>("Audio/death");
            SoundManager.gameStart = Content.Load<SoundEffect>("Audio/gameStart");
            SoundManager.starRecieve = Content.Load<SoundEffect>("Audio/starRecieve");
        }
        #endregion LoadNewContent
    }
}
