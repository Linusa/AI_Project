namespace AIFGP_Game
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System.IO;

    /// <summary>
    /// Super basic map implementation, this will be changed
    /// in a big way later. Currently just makes a bunch of
    /// tiles based on an input file.
    /// </summary>
    public class Map : IDrawable
    {
        public enum TileType
        {
            Ground,
            Wall
        }

        public int TilesAcross;
        public int TilesDown;

        // Map tiles are 28x28 in px.
        public static readonly Vector2 TileSize
            = new Vector2(28.0f, 28.0f);
        
        private List<TileInfo> mapTiles = new List<TileInfo>();

        private Sprite<byte> grassTile;
        private Sprite<byte> wallTile;

        private string mapDirectory;

        public Map(string mapFileName)
        {
            mapDirectory = @"C:\Users\Jason\Documents\AI_Project\AIFGP_Project\AIFGP_Game\AIFGP_Game\Map\ascii_maps\";
            loadMapFromText(mapFileName);

            Rectangle tileFrame = new Rectangle(0, 0, (int)TileSize.X, (int)TileSize.Y);
            
            grassTile = new Sprite<byte>(AStarGame.GrassTile, Vector2.Zero, tileFrame);
            grassTile.AddAnimationFrame(0, tileFrame);
            grassTile.ActiveAnimation = 0;

            wallTile = new Sprite<byte>(AStarGame.WallTile, Vector2.Zero, tileFrame);
            wallTile.AddAnimationFrame(0, tileFrame);
            wallTile.ActiveAnimation = 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (TileInfo tileInfo in mapTiles)
            {
                Sprite<byte> tile;

                if (tileInfo.Type == TileType.Wall)
                    tile = wallTile;
                else
                    tile = grassTile;

                tile.Position = tileInfo.Position;
                tile.Draw(spriteBatch);
            }
        }

        private void loadMapFromText(string mapFileName)
        {
            string path = mapDirectory + mapFileName;

            if (!File.Exists(mapDirectory + mapFileName))
            {
                System.Diagnostics.Debug.WriteLine("Map '" + mapFileName
                    + "' does not exist!");
                return;
            }

            string[] textMap = File.ReadAllLines(path);

            TilesAcross = textMap[0].Length;
            TilesDown = textMap.Length;

            int xOffset = (AStarGame.ScreenDimensions.Width - TilesAcross * (int)TileSize.X) / 2;
            int yOffset = (AStarGame.ScreenDimensions.Height - TilesDown * (int)TileSize.Y) / 2;
            Vector2 offset = new Vector2(xOffset, yOffset);

            for (int i = 0; i < textMap.Length; i++)
            {
                char[] chars = textMap[i].ToCharArray();
                for (int j = 0; j < chars.Length; j++)
                {
                    TileInfo curTile = new TileInfo();
                    curTile.Position = offset + new Vector2(j * TileSize.X, i * TileSize.Y);

                    if (chars[j] == 'W')
                    {
                        // Create a Wall and register it with the WallManager.
                        Wall curWall = new Wall();
                        curWall.TopLeftPixel = curTile.Position;
                        curWall.BottomRightPixel = curTile.Position + TileSize;
                        WallManager.Instance.AddWall(curWall);

                        curTile.Type = TileType.Wall;
                    }
                    else
                        curTile.Type = TileType.Ground;

                    mapTiles.Add(curTile);
                }
            }
        }
    }
}
