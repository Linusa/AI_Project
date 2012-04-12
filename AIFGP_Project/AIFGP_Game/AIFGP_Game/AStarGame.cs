namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
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

        public static Rectangle WorldDimensions;// = new Rectangle(0, 0, 1932, 1120);
        public static Vector2 WorldCenter;// = new Vector2(WorldDimensions.Width,
            //WorldDimensions.Height) / 2;

        private static Camera gameCamera;

        public static Map GameMap;

        private PlayerManager playerManager;
        private EnemyManager enemyManager;

        
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

            //currentScenario = Content.Load<Scenario>(GlobalGameSettings.Game.Scenario);
            currentScenario = Content.Load<Scenario>(@"Scenarios\Scenario002");

            // This could go in a little method.
            WorldDimensions = new Rectangle(0, 0,
                currentScenario.MapDescription.TilesAcross * (int)currentScenario.MapDescription.TileSize.X,
                currentScenario.MapDescription.TilesDown * (int)currentScenario.MapDescription.TileSize.Y);
            WorldCenter = new Vector2(WorldDimensions.Width, WorldDimensions.Height) / 2;

            GameMap = new Map(currentScenario.MapDescription);
            playerManager = new PlayerManager(currentScenario.PlayerDescription);
            enemyManager = new EnemyManager(currentScenario.EnemiesDescription);

            gameCamera = new Camera(graphics.GraphicsDevice, playerManager.Player.Position);
            
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

            GameMap.Update(gameTime);
            playerManager.Update(gameTime);
            enemyManager.Update(gameTime);

            MouseState mouseState = Mouse.GetState();
            if (mouseState.MiddleButton == ButtonState.Pressed)
            {
                gameCamera.Position += new Vector2(prevMouseState.X - mouseState.X, prevMouseState.Y - mouseState.Y);

                //Vector2 mouseVec = MousePositionInWorld();
                //Window.Title = "Mouse: " + mouseVec + " | IsWorldPosWall("
                //    + mouseVec.X + ", " + mouseVec.Y + "): " + GameMap.IsWorldPosWall(mouseVec); ;
            }
            
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

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.BackToFront, null,
                null, null, null, null, gameCamera.Transformation);
                    GameMap.Draw(spriteBatch);
                    playerManager.Draw(spriteBatch);
                    enemyManager.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public static Vector2 MousePositionInWorld()
        {
            MouseState mouse = Mouse.GetState();
            return PositionInWorld(new Vector2(mouse.X, mouse.Y));
        }

        /// <summary>
        /// This should be stored in some utilities class, but for now,
        /// this is fine.
        /// </summary>
        public static Vector2 PositionInWorld(Vector2 position)
        {
            return Vector2.Transform(position, Matrix.Invert(gameCamera.Transformation));
        }

        /// <summary>
        /// This should be stored in some utilities class, but for now,
        /// this is fine.
        /// </summary>
        public static Vector2 PositionInView(Vector2 position)
        {
            return Vector2.Transform(position, gameCamera.Transformation);
        }

#if false
        /// <summary>
        /// This should be stored in some utilities class, but for now,
        /// this is fine.
        /// </summary>
        /// <param name="entity"></param>
        public static void WrapPosition(ref IGameEntity entity)
        {
            Vector2 position = entity.Position;

            int spriteWidth = entity.BoundingBox.Width;
            int spriteHeight = entity.BoundingBox.Height;

            Rectangle bounds = ScreenDimensions;
            bounds.X -= spriteWidth;
            bounds.Y -= spriteHeight;
            bounds.Width += spriteWidth;
            bounds.Height += spriteHeight;

            if (position.X < bounds.X)
                position.X = bounds.Width;
            else if (position.X > bounds.Width)
                position.X = bounds.X;

            if (position.Y < bounds.Y)
                position.Y = bounds.Height;
            else if (position.Y > bounds.Height)
                position.Y = bounds.Y;

            if (entity.Position != position)
                entity.Position = position;
        }
#endif
    }
}
