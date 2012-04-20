namespace AIFGP_Game_MapCreation
{
    using System;
    using System.Collections.Generic;
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
        private SmartFarmer enemySprite =
            new SmartFarmer(TextureManager.FarmerSprite, Vector2.Zero, new List<Vector2>());
        private VisualRectangle selectionSprite;
        private Carrot carrotSprite = new Carrot(TextureManager.CarrotSprite, Vector2.Zero);

        private int selectedEnemy;

        public MapCreator(Game game, string outFileName, int numRows, int numCols, Vector2 tileSize)
        {
            this.outFileName = outFileName;

            TilesAcross = numRows;
            TilesDown = numCols;
            TileSize = tileSize;

            int[] tileTypeIndices = new int[numRows * numCols];
            for (int i = 0; i < numRows * numCols; i++)
                tileTypeIndices[i] = (int)TileType.Ground;

            string[] fileNames = Directory.GetFiles(".", "Scenario*.xml", SearchOption.TopDirectoryOnly);

            if (fileNames.Length > 0)
            {
                this.outFileName = fileNames[0];
                Load(this.outFileName);
            }
            else
            {
                scenario = new Scenario();

                scenario.MapDescription = new MapDescription();
                scenario.MapDescription.TilesDown = numRows;
                scenario.MapDescription.TilesAcross = numCols;
                scenario.MapDescription.TileSize = tileSize;
                scenario.MapDescription.MapTiles = tileTypeIndices;

                scenario.PlayerDescription = new PlayerDescription();
                scenario.PlayerDescription.MaxSpeed = 200.0f;

                scenario.EnemiesDescription = new EnemiesDescription();
                scenario.EnemiesDescription.EnemiesInfo = new List<EnemiesDescription.EnemyInfo>();
            }

            Map = new EditorMap(scenario.MapDescription);
            
            Camera = new MapCreatorCamera(game.GraphicsDevice, Vector2.Zero);
            
            Gui = new MapCreatorGui();
            buttonActions.Add(MapCreatorGui.Button.Grass, handleGrassEditing);
            buttonActions.Add(MapCreatorGui.Button.Wall, handleWallEditing);
            buttonActions.Add(MapCreatorGui.Button.Bush, handleBushEditing);
            buttonActions.Add(MapCreatorGui.Button.Player_Position, handlePlayerEditing);
            buttonActions.Add(MapCreatorGui.Button.Carrot_Position, handleCarrotEditing);
            buttonActions.Add(MapCreatorGui.Button.Enemies, handleEnemyEditing);
            buttonActions.Add(MapCreatorGui.Button.Save, Save);

            selectionSprite = new VisualRectangle(enemySprite.BoundingBox);
            selectionSprite.FillColor = Color.Transparent;
            selectionSprite.EdgeColor = Color.Transparent;

            // Handle updating anything in the editor if scenario XML was loaded.
            playerSprite.Position = Map.TilePosToWorldPos(scenario.PlayerDescription.StartingTilePosition);
            carrotSprite.Position = Map.TilePosToWorldPos(scenario.PlayerDescription.CarrotTilePosition);
        }

        public void Save()
        {
            if (minTimeBetweenSaves < timeSinceLastSave)
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;

                using (XmlWriter writer = XmlWriter.Create(outFileName, settings))
                {
                    IntermediateSerializer.Serialize(writer, scenario, null);
                }

                timeSinceLastSave = 0.0f;
            }
        }

        public void Load(string file)
        {
            using (XmlReader reader = XmlReader.Create(file))
            {
                scenario = IntermediateSerializer.Deserialize<Scenario>(reader, null);
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
            carrotSprite.Draw(spriteBatch);

            foreach (EnemiesDescription.EnemyInfo enemy in scenario.EnemiesDescription.EnemiesInfo)
            {
                enemySprite.Position = Map.TilePosToWorldPos(enemy.StartingTilePosition);
                enemySprite.Draw(spriteBatch);
            }

            if (Gui.ToggledButton == MapCreatorGui.Button.Enemies)
            {
                selectionSprite.Draw(spriteBatch);

                if (scenario.EnemiesDescription.EnemiesInfo.Count > 0)
                {
                    // Longest line ever.
                    int patrolCount = 1;
                    foreach (Vector2 patrolPos in scenario.EnemiesDescription.EnemiesInfo[selectedEnemy].PatrolTilePositions)
                    {
                        string patCountStr = patrolCount.ToString();
                        Vector2 patrolWorld = Map.TilePosToWorldPos(patrolPos) - FontManager.DebugFont.MeasureString(patCountStr) / 2;
                        spriteBatch.DrawString(FontManager.DebugFont, patrolCount.ToString(), patrolWorld, Color.Orange);
                        patrolCount++;
                    }
                }
            }
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

                if (scenario.EnemiesDescription.EnemiesInfo.Count > 0 &&
                    Gui.ToggledButton == MapCreatorGui.Button.Enemies &&
                    Mouse.GetState().RightButton == ButtonState.Pressed)
                {
                    Vector2 mouse = MapCreatorGame.MousePositionInWorld();
                    if (Map.WorldPosWithinMapBounds(mouse))
                    {
                        clickedTile = Map.WorldPosToTilePos(mouse);
                        addEnemyPatrolPath();

                        timeSinceLastClick = 0.0f;
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
            scenario.PlayerDescription.StartingTilePosition = clickedTile;
        }

        private void handleCarrotEditing()
        {
            carrotSprite.Position = Map.TilePosToWorldPos(clickedTile);
            scenario.PlayerDescription.CarrotTilePosition = clickedTile;
        }

        private void handleEnemyEditing()
        {
            Vector2 rectInWorld = new Vector2(selectionSprite.Rectangle.X, selectionSprite.Rectangle.Y);
            Vector2 rectCenter = new Vector2(selectionSprite.Rectangle.Width, selectionSprite.Rectangle.Height) / 2;
            Vector2 tileInWorld = Map.TilePosToWorldPos(clickedTile);
            
            selectionSprite.Translate(tileInWorld - rectInWorld - rectCenter);
            selectionSprite.EdgeColor = Color.Orange;

            for (int i = 0; i < scenario.EnemiesDescription.EnemiesInfo.Count; i++)
            {
                if (scenario.EnemiesDescription.EnemiesInfo[i].StartingTilePosition == clickedTile)
                {
                    selectedEnemy = i;
                    return;
                }
            }

            EnemiesDescription.EnemyInfo info = new EnemiesDescription.EnemyInfo();
            info.StartingTilePosition = clickedTile;
            info.MaxSpeed = 200.0f;
            info.PatrolTilePositions = new List<Vector2>();

            scenario.EnemiesDescription.EnemiesInfo.Add(info);
            selectedEnemy = scenario.EnemiesDescription.EnemiesInfo.Count - 1;
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
            int row = (int)clickedTile.Y;
            int col = (int)clickedTile.X;

            TileInfo tileInfo = Map.TileInfoAtTilePos(row, col);
            tileInfo.Type = tileType;

            Map.SetTileInfoAtTilePos(row, col, tileInfo);
            
            int idx = (row * scenario.MapDescription.TilesAcross) + col;
            scenario.MapDescription.MapTiles[idx] = (int)tileType;
        }

        private void addEnemyPatrolPath()
        {
            scenario.EnemiesDescription.EnemiesInfo[selectedEnemy].PatrolTilePositions.Add(clickedTile);
        }
    }
}
