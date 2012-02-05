namespace AIFGP_Game
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SensorsGame : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public static Rectangle ScreenDimensions = new Rectangle(0, 0, 800, 600);
        public static Vector2 ScreenCenter = new Vector2(ScreenDimensions.Width,
            ScreenDimensions.Height) / 2;

        public static Texture2D PlayerSpriteSheet;
        public static Texture2D NpcSpriteSheet;
        public static Texture2D GrassTile;
        public static Texture2D RadarCircle;
        public static Texture2D SingleWhitePixel;

        private Map map;
        private PlayerManager playerManager;
        private EnemyManager enemyManager;

        public static SpriteFont DebugFont;

        public SensorsGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = ScreenDimensions.Width;
            graphics.PreferredBackBufferHeight = ScreenDimensions.Height;
            graphics.ApplyChanges();

            IsMouseVisible = true;

            // Hot-fix for the every-second or so stuttering.
            //IsFixedTimeStep = false;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            PlayerSpriteSheet = Content.Load<Texture2D>(@"Images\player_arrow_with_effects");
            NpcSpriteSheet = Content.Load<Texture2D>(@"Images\npc_arrow_with_effects");
            GrassTile = Content.Load<Texture2D>(@"Images\grass_tile");
            RadarCircle = Content.Load<Texture2D>(@"Images\circle");

            SingleWhitePixel = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            SingleWhitePixel.SetData<Color>(new Color[1] { Color.White }); 

            map = new Map();
            playerManager = new PlayerManager();
            enemyManager = new EnemyManager();

            DebugFont = Content.Load<SpriteFont>(@"Fonts\Debug");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            // TODO: Add your update logic here
            playerManager.Update(gameTime);
            enemyManager.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
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
