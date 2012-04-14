namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    using AIFGP_Game_Data;

    public class AStarGame : Microsoft.Xna.Framework.Game
    {
        public struct DrawingOrder
        {
            public const float Grass = 1.0f;
            public const float Wall = 0.9f;
            public const float Entities = 0.8f;
            public const float Bush = 0.7f;
        }

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public static GlobalSettings GlobalGameSettings;

        private Scenario currentScenario;

        public static Rectangle WorldDimensions;
        public static Vector2 WorldCenter;

        public static Vector2 ScreenCenter;

        private static Camera gameCamera;

        public static Map GameMap;

        private PlayerManager playerManager;
        private EnemyManager enemyManager;

        private GameState gameState = GameState.Play;
        private enum GameState
        {
            Play,
            BeatLevel,
            LostLevel,
            WonGame
        }

        private string scenarioName = @"Scenarios\Scenario{0:000}";
        private int scenarioNumber = 1;

        private string beatLvlMsg = "Congratulations!\nYou got the carrot!!!\n\n"
            + "Next level starting soon...";
        private Vector2 beatLvlMsgCenter;
        private Timer beatLvlMsgDisplayTimer = new Timer(3.5f);

        private string lostLvlMsg = "You let a farmer catch you!\n\n"
            + "Use bushes to hide.";
        private Vector2 lostLvlMsgCenter;
        private Timer lostLvlMsgDisplayTimer = new Timer(3.5f);

        private string wonGameMsg = "You beat the game!\nThanks for playing!\n\n"
            + "Press 'Esc' to quit.";
        private Vector2 wonGameMsgCenter;
        
        private MouseState prevMouseState;

        public AStarGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            GlobalSettings.Load(@"Settings\global_settings.xml", out GlobalGameSettings);

            Window.Title = GlobalGameSettings.Game.Name;
            IsMouseVisible = GlobalGameSettings.Game.IsMouseVisible;
            IsFixedTimeStep = GlobalGameSettings.Game.IsFixedTimeStep;

            graphics.PreferredBackBufferWidth = GlobalGameSettings.Screen.ResolutionWidth;
            graphics.PreferredBackBufferHeight = GlobalGameSettings.Screen.ResolutionHeight;
            graphics.IsFullScreen = GlobalGameSettings.Screen.IsFullScreen;
            graphics.ApplyChanges();

            ScreenCenter = new Vector2(graphics.PreferredBackBufferWidth,
                graphics.PreferredBackBufferHeight) / 2;

            // BEGIN Testing
            System.Diagnostics.Debug.WriteLine("------- BEGIN TESTING! -------");
            System.Diagnostics.Debug.WriteLine("------- DONE TESTING! -------");
            // END Testing

            base.Initialize();
        }

        protected override void LoadContent()
        {
            FontManager.LoadFonts(this);
            TextureManager.LoadTextures(this);
            
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            beatLvlMsgCenter = FontManager.BigFont.MeasureString(beatLvlMsg) / 2;
            lostLvlMsgCenter = FontManager.BigFont.MeasureString(lostLvlMsg) / 2;
            wonGameMsgCenter = FontManager.BigFont.MeasureString(wonGameMsg) / 2;

            loadScenario(String.Format(scenarioName, scenarioNumber));
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            switch (gameState)
            {
                case GameState.Play:
                    GameMap.Update(gameTime);
                    playerManager.Update(gameTime);
                    enemyManager.Update(gameTime);

                    if (playerManager.PlayerReachedCarrot)
                    {
                        gameState = GameState.BeatLevel;
                        
                        scenarioNumber++;
                        loadScenario(String.Format(scenarioName, scenarioNumber));
                        
                        beatLvlMsgDisplayTimer.Restart();
                    }

                    if (enemyManager.FarmerCaughtRabbit)
                    {
                        gameState = GameState.LostLevel;

                        playerManager.ResetPlayerPosition();
                        gameCamera.Position = playerManager.Player.Position;

                        lostLvlMsgDisplayTimer.Restart();
                    }
        
                    MouseState mouseState = Mouse.GetState();
                    if (mouseState.MiddleButton == ButtonState.Pressed)
                        gameCamera.Position += new Vector2(prevMouseState.X - mouseState.X, prevMouseState.Y - mouseState.Y);
                    
                    if (mouseState.ScrollWheelValue != prevMouseState.ScrollWheelValue)
                    {
                        // magFactor assigned either 1.1 or 0.9 depending on direction of scroll.
                        float magFactor = (mouseState.ScrollWheelValue - prevMouseState.ScrollWheelValue) / 1200.0f + 1;
                        gameCamera.Magnification *= magFactor;
                    }
         
                    prevMouseState = mouseState;
        
                    if (!playerManager.Player.Velocity.Equals(Vector2.Zero))
                        gameCamera.Position = playerManager.Player.Position;
        
                    if (playerManager.Player.IsHidden)
                        Window.Title = "Player is in a bush!";
                    else
                        Window.Title = "Player is NOT in a bush!";
                    
                        break;

                case GameState.BeatLevel:
                    if (beatLvlMsgDisplayTimer.Expired(gameTime))
                        gameState = GameState.Play;

                    break;

                case GameState.LostLevel:
                    if (lostLvlMsgDisplayTimer.Expired(gameTime))
                        gameState = GameState.Play;

                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            switch (gameState)
            {
                case GameState.Play:
                    GraphicsDevice.Clear(Color.Black);
        
                    spriteBatch.Begin(SpriteSortMode.BackToFront, null,
                        null, null, null, null, gameCamera.Transformation);
                    GameMap.Draw(spriteBatch);
                    playerManager.Draw(spriteBatch);
                    enemyManager.Draw(spriteBatch);
                    spriteBatch.End();
                    break;

                case GameState.BeatLevel:
                    GraphicsDevice.Clear(Color.Black);

                    spriteBatch.Begin();
                    spriteBatch.DrawString(FontManager.BigFont, beatLvlMsg, ScreenCenter,
                        Color.Yellow, 0.0f, beatLvlMsgCenter, 1.0f, SpriteEffects.None, 0.5f);
                    spriteBatch.End();
                    break;

                case GameState.LostLevel:
                    GraphicsDevice.Clear(Color.Black);

                    spriteBatch.Begin();
                    spriteBatch.DrawString(FontManager.BigFont, lostLvlMsg, ScreenCenter,
                        Color.Yellow, 0.0f, lostLvlMsgCenter, 1.0f, SpriteEffects.None, 0.5f);
                    spriteBatch.End();
                    break;

                case GameState.WonGame:
                    GraphicsDevice.Clear(Color.Black);

                    spriteBatch.Begin();
                    spriteBatch.DrawString(FontManager.BigFont, wonGameMsg, ScreenCenter,
                        Color.Green, 0.0f, wonGameMsgCenter, 1.0f, SpriteEffects.None, 0.5f);
                    spriteBatch.End();
                    break;
            }

            base.Draw(gameTime);
        }

        private void loadScenario(string scenarioName)
        {
            try
            {
                //currentScenario = Content.Load<Scenario>(GlobalGameSettings.Game.Scenario);
                currentScenario = Content.Load<Scenario>(scenarioName);

                WorldDimensions = new Rectangle(0, 0,
                    currentScenario.MapDescription.TilesAcross * (int)currentScenario.MapDescription.TileSize.X,
                    currentScenario.MapDescription.TilesDown * (int)currentScenario.MapDescription.TileSize.Y);
                WorldCenter = new Vector2(WorldDimensions.Width, WorldDimensions.Height) / 2;

                GameMap = new Map(currentScenario.MapDescription);
                playerManager = new PlayerManager(currentScenario.PlayerDescription);
                enemyManager = new EnemyManager(currentScenario.EnemiesDescription);

                gameCamera = new Camera(graphics.GraphicsDevice, playerManager.Player.Position);
                gameCamera.Magnification = 1.5f;
            }
            // Super hack for when scenarioNumber becomes larger than the number of scenarios.
            catch (ContentLoadException)
            {
                gameState = GameState.WonGame;
            }
        }

        public static Vector2 MousePositionInWorld()
        {
            MouseState mouse = Mouse.GetState();
            return PositionInWorld(new Vector2(mouse.X, mouse.Y));
        }

        public static Vector2 PositionInWorld(Vector2 position)
        {
            return Vector2.Transform(position, Matrix.Invert(gameCamera.Transformation));
        }

        public static Vector2 PositionInView(Vector2 position)
        {
            return Vector2.Transform(position, gameCamera.Transformation);
        }
    }
}
