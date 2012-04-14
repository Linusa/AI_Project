namespace AIFGP_Game_MapCreation
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using AIFGP_Game;

    public class MapCreatorGame : Microsoft.Xna.Framework.Game
    {
        private MapCreator mapCreator;
        
        private SpriteBatch spriteBatch;
        private GraphicsDeviceManager graphics;

        private static Camera gameCamera;

        private int screenWidth = 1280;
        private int screenHeight = 720;

        private string windowTitle = "R:{0,4:000}, C:{1,4:000}, | " +
            "Toggle Gridlines: G | Toggle Headers: H";

        public MapCreatorGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.ApplyChanges();

            IsMouseVisible = true;
            IsFixedTimeStep = false;
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            FontManager.LoadFonts(this);
            TextureManager.LoadTextures(this);
            
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            mapCreator = new MapCreator(this, "Scenario000.xml", 60, 80,
                new Vector2(28.0f, 28.0f));

            gameCamera = mapCreator.Camera;
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            mapCreator.Update(gameTime);

            updateWindowTitle();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, null,
                null, null, null, null, mapCreator.Camera.Transformation);
            mapCreator.Draw(spriteBatch);
            spriteBatch.End();

            // Hack. The MapCreator should draw its own gui.
            spriteBatch.Begin();
            mapCreator.Gui.Draw(spriteBatch);
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

        private void updateWindowTitle()
        {
            Vector2 mouse = MousePositionInWorld();
            mouse = mapCreator.Map.WorldPosToTilePos(mouse);

            int x = (int)mouse.X;
            int y = (int)mouse.Y;

            if (mapCreator.Map.TilePosWithinMapBounds(y, x))
                Window.Title = String.Format(windowTitle, mouse.Y, mouse.X);
            else
                Window.Title = String.Format(windowTitle, 0, 0);
        }
    }
}
