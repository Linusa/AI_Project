namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// This is the main type for your game
    /// </summary>
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

        private GraphViewer debugGraphViewer;

        public AStarGame()
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
            IsFixedTimeStep = false;

            // BEGIN Testing
            System.Diagnostics.Debug.WriteLine("------- BEGIN TESTING! -------");
            System.Diagnostics.Debug.WriteLine("------- DONE TESTING! -------");
            // END Testing

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
            WallTile = Content.Load<Texture2D>(@"Images\wall_tile");
            RadarCircle = Content.Load<Texture2D>(@"Images\circle");

            SingleWhitePixel = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            SingleWhitePixel.SetData<Color>(new Color[1] { Color.White }); 

            map = new Map("map002.txt");
            playerManager = new PlayerManager();
            enemyManager = new EnemyManager();
            
            Random rng = new Random();
            Graph<PositionalNode, Edge> graph = new Graph<PositionalNode, Edge>();

            float w = ScreenDimensions.Width;
            float h = ScreenDimensions.Height;
            Point center = ScreenDimensions.Center;

            Vector2 p0 = new Vector2(3*w/6, 1*h/5);
            Vector2 p1 = new Vector2(2*w/6, 2*h/5);
            Vector2 p2 = new Vector2(3*w/6, 2*h/5);
            Vector2 p3 = new Vector2(4*w/6, 2*h/5);
            Vector2 p4 = new Vector2(1*w/6, 3*h/5);
            Vector2 p5 = new Vector2(2*w/6, 3*h/5);
            Vector2 p6 = new Vector2(3*w/6, 3*h/5);

            graph.AddNode(new PositionalNode(graph.AvailableNodeIndex, p0));
            graph.AddNode(new PositionalNode(graph.AvailableNodeIndex, p1));
            graph.AddNode(new PositionalNode(graph.AvailableNodeIndex, p2));
            graph.AddNode(new PositionalNode(graph.AvailableNodeIndex, p3));
            graph.AddNode(new PositionalNode(graph.AvailableNodeIndex, p4));
            graph.AddNode(new PositionalNode(graph.AvailableNodeIndex, p5));
            graph.AddNode(new PositionalNode(graph.AvailableNodeIndex, p6));

            graph.AddEdge(new Edge(0, 1, (p0-p1).Length()));
            graph.AddEdge(new Edge(0, 2, (p0-p1).Length()));
            graph.AddEdge(new Edge(0, 3, (p0-p3).Length()));
            graph.AddEdge(new Edge(1, 4, (p1-p4).Length()));
            graph.AddEdge(new Edge(1, 5, (p1-p5).Length()));
            graph.AddEdge(new Edge(2, 6, (p2-p6).Length()));
            graph.AddEdge(new Edge(3, 5, (p3-p5).Length()));

            debugGraphViewer = new GraphViewer(graph);

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
                debugGraphViewer.Draw(spriteBatch);
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
