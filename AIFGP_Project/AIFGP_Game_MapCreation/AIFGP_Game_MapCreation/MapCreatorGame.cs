using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using AIFGP_Game;

namespace AIFGP_Game_MapCreation
{
    public class MapCreatorGame : Microsoft.Xna.Framework.Game
    {
        private MapCreator mapCreator;
        private MapCreatorGui gui;
        
        private SpriteBatch spriteBatch;
        private GraphicsDeviceManager graphics;

        private static Camera gameCamera;

        public MapCreatorGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
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
            
            mapCreator = new MapCreator(this, 60, 80, new Vector2(28.0f, 28.0f));
            gui = new MapCreatorGui();

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

            spriteBatch.Begin();
            gui.Draw(spriteBatch);
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

            Window.Title = "R:" + mouse.Y + " C:" + mouse.X;
        }
    }
}
