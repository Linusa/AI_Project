namespace AIFGP_Game_MapCreation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using System.Xml;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using AIFGP_Game;
    using AIFGP_Game_Data;

    public class MapCreator : AIFGP_Game.IUpdateable, AIFGP_Game.IDrawable
    {
        public MapCreatorCamera Camera;

        public readonly int TilesAcross;
        public readonly int TilesDown;
        public readonly Vector2 TileSize;

        public EditorMap Map;
        private Scenario scenario;
        
        public MapCreatorGui Gui;

        private Dictionary<MapCreatorGui.Button, Action> buttonActions =
            new Dictionary<MapCreatorGui.Button, Action>();

        private string outFileName;
        
        private float timeSinceLastClick = 0.0f;
        private float minTimeBetweenClicks = 0.25f;

        private float timeSinceLastKey = 0.0f;
        private float minTimeBetweenKeys = 0.20f;

        private float timeSinceLastSave = 0.0f;
        private float minTimeBetweenSaves = 1.0f;

        private Vector2 clickedTile = Vector2.Zero;

        private Rabbit playerSprite = new Rabbit(TextureManager.RabbitSpriteSheet, Vector2.Zero);

        public MapCreator(Game game, string outFileName, int numRows, int numCols, Vector2 tileSize)
        {
            this.outFileName = outFileName;

            TilesAcross = numRows;
            TilesDown = numCols;
            TileSize = tileSize;

            int[] tileTypeIndices = new int[numRows * numCols];
            for (int i = 0; i < numRows * numCols; i++)
                tileTypeIndices[i] = (int)TileType.Ground;

            scenario = new Scenario();
            
            scenario.MapDescription = new MapDescription();
            scenario.MapDescription.TilesDown = numRows;
            scenario.MapDescription.TilesAcross = numCols;
            scenario.MapDescription.TileSize = tileSize;
            scenario.MapDescription.MapTiles = tileTypeIndices;

            scenario.PlayerDescription = new PlayerDescription();
            scenario.PlayerDescription.MaxSpeed = 200.0f;

            scenario.EnemiesDescription = new EnemiesDescription();

            Map = new EditorMap(scenario.MapDescription);
            
            Camera = new MapCreatorCamera(game.GraphicsDevice, Vector2.Zero);
            
            Gui = new MapCreatorGui();
            buttonActions.Add(MapCreatorGui.Button.Grass, handleGrassEditing);
            buttonActions.Add(MapCreatorGui.Button.Wall, handleWallEditing);
            buttonActions.Add(MapCreatorGui.Button.Bush, handleBushEditing);
            buttonActions.Add(MapCreatorGui.Button.Player_Position, handlePlayerEditing);
            buttonActions.Add(MapCreatorGui.Button.Save, Save);
        }

        public void Save()
        {
            if (minTimeBetweenSaves < timeSinceLastSave)
            {
                updateMapDescription();
                scenario.PlayerDescription.StartingTilePosition =
                    Map.WorldPosToTilePos(playerSprite.Position);

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;

                using (XmlWriter writer = XmlWriter.Create(outFileName, settings))
                {
                    IntermediateSerializer.Serialize(writer, scenario, null);
                }

                timeSinceLastSave = 0.0f;
            }
        }

        private void updateMapDescription()
        {
            for (int i = 0; i < scenario.MapDescription.TilesDown; i++)
            {
                for (int j = 0; j < scenario.MapDescription.TilesAcross; j++)
                {
                    int idx = (i * scenario.MapDescription.TilesAcross) + j;
                    scenario.MapDescription.MapTiles[idx] = (int)Map.TileInfoAtTilePos(i, j).Type;
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            Camera.Update(gameTime);

            Map.Update(gameTime);
            Gui.Update(gameTime);

            handleUserInput(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Map.Draw(spriteBatch);
            playerSprite.Draw(spriteBatch);
        }

        private void handleUserInput(GameTime gameTime)
        {
            timeSinceLastClick += (float)gameTime.ElapsedGameTime.TotalSeconds;
            timeSinceLastKey += (float)gameTime.ElapsedGameTime.TotalSeconds;
            timeSinceLastSave += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (minTimeBetweenClicks < timeSinceLastClick)
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    if (Gui.ButtonJustPressed && Gui.ToggledButton == MapCreatorGui.Button.Save)
                        buttonActions[MapCreatorGui.Button.Save]();
                    else if (!Gui.ButtonJustPressed && Gui.ToggledButton != MapCreatorGui.Button.Save)
                    {
                        Vector2 mouse = MapCreatorGame.MousePositionInWorld();
                        if (Map.WorldPosWithinMapBounds(mouse))
                        {
                            clickedTile = Map.WorldPosToTilePos(mouse);
                            buttonActions[Gui.ToggledButton]();

                            timeSinceLastClick = 0.0f;
                        }
                    }
                }
            }

            if (minTimeBetweenKeys < timeSinceLastKey)
            {
                KeyboardState kbState = Keyboard.GetState();
                if (kbState.IsKeyDown(Keys.G))
                {
                    Map.AreGridlinesVisible = !Map.AreGridlinesVisible;
                    timeSinceLastKey = 0.0f;
                }

                if (kbState.IsKeyDown(Keys.H))
                {
                    Map.AreHeadersVisible = !Map.AreHeadersVisible;
                    timeSinceLastKey = 0.0f;
                }
            }
        }

        private void handlePlayerEditing()
        {
            playerSprite.Position = Map.TilePosToWorldPos(clickedTile);
        }

        private void handleGrassEditing()
        {
            changeTile(TileType.Ground);
        }

        private void handleWallEditing()
        {
            changeTile(TileType.Wall);
        }

        private void handleBushEditing()
        {
            changeTile(TileType.Bush);
        }

        private void changeTile(TileType tileType)
        {
            TileInfo tileInfo = Map.TileInfoAtTilePos((int)clickedTile.Y, (int)clickedTile.X);
            tileInfo.Type = tileType;

            Map.SetTileInfoAtTilePos((int)clickedTile.Y, (int)clickedTile.X, tileInfo);
        }
    }
}
