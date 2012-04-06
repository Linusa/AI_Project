namespace AIFGP_Game_MapCreation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using System.Xml.Serialization;
    using Microsoft.Xna.Framework;
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
        private MapDescription mapDesc;

        public MapCreator(Game game, int numRows, int numCols, Vector2 tileSize)
        {
            TilesAcross = numRows;
            TilesDown = numCols;
            TileSize = tileSize;

            int[] tileTypeIndices = new int[numRows * numCols];
            for (int i = 0; i < numRows * numCols; i++)
                tileTypeIndices[i] = (int)TileType.Ground;
            
            mapDesc = new MapDescription();
            mapDesc.TilesDown = numRows;
            mapDesc.TilesAcross = numCols;
            mapDesc.TileSize = tileSize;
            mapDesc.MapTiles = tileTypeIndices;

            Map = new EditorMap(mapDesc);
            
            Camera = new MapCreatorCamera(game.GraphicsDevice, Vector2.Zero);
        }

        public void Save(string fileName)
        {
            updateMapDescription();

            Stream stream = File.Create(fileName);
            XmlSerializer serializer = new XmlSerializer(typeof(GlobalSettings));
            serializer.Serialize(stream, this);
            stream.Close();
        }

        private void updateMapDescription()
        {
            for (int i = 0; i < mapDesc.TilesDown; i++)
            {
                for (int j = 0; j < mapDesc.TilesAcross; j++)
                {
                    int idx = (i * mapDesc.TilesAcross) + j;
                    mapDesc.MapTiles[idx] = (int)Map.TileInfoAtTilePos(i, j).Type;
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            Camera.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Map.Draw(spriteBatch);
        }
    }
}
