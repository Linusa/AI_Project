namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class AStarGame : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public static Rectangle ScreenDimensions = new Rectangle(0, 0, 800, 600);
        //public static Rectangle ScreenDimensions = new Rectangle(0, 0, 1920, 1080);
        public static Vector2 ScreenCenter = new Vector2(ScreenDimensions.Width,
            ScreenDimensions.Height) / 2;

        public static Texture2D PlayerSpriteSheet;
        public static Texture2D NpcSpriteSheet;
        public static Texture2D GrassTile;
        public static Texture2D WallTile;
        public static Texture2D RadarCircle;
        public static Texture2D SingleWhitePixel;

        private Map map;
        private PlayerManager playerManager;
        private EnemyManager enemyManager;

        public static SpriteFont DebugFont;
        public static SpriteFont SmallDebugFont;

        public AStarGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = ScreenDimensions.Width;
            graphics.PreferredBackBufferHeight = ScreenDimensions.Height;
            graphics.ApplyChanges();

            IsMouseVisible = true;

            // Hot-fix for the every-second or so stuttering.
            IsFixedTimeStep = false;

            // BEGIN Testing
            System.Diagnostics.Debug.WriteLine("------- BEGIN TESTING! -------");
            System.Diagnostics.Debug.WriteLine("------- DONE TESTING! -------");
            // END Testing

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            PlayerSpriteSheet = Content.Load<Texture2D>(@"Images\player_arrow_with_effects");
            NpcSpriteSheet = Content.Load<Texture2D>(@"Images\npc_arrow_with_effects");
            GrassTile = Content.Load<Texture2D>(@"Images\grass_tile");
            WallTile = Content.Load<Texture2D>(@"Images\wall_tile");
            RadarCircle = Content.Load<Texture2D>(@"Images\circle");

            SingleWhitePixel = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            SingleWhitePixel.SetData<Color>(new Color[1] { Color.White }); 

            map = new Map("map003.txt");
            playerManager = new PlayerManager();
            enemyManager = new EnemyManager();
            
            DebugFont = Content.Load<SpriteFont>(@"Fonts\Debug");
            SmallDebugFont = Content.Load<SpriteFont>(@"Fonts\Debug_Small");

            Window.Title = "4: Graph | 5: Indices | Lalt+Lmouse: Source | "
                + "Ralt+Lmouse: Target | Lsh+Lmouse: Add Wall | Lsh+Rmouse: Del Wall";
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

            map.Update(gameTime);
            playerManager.Update(gameTime);
            enemyManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
                map.Draw(spriteBatch);
                playerManager.Draw(spriteBatch);
                enemyManager.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

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
    }
}
