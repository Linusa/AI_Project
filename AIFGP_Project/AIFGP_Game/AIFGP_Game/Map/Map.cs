namespace AIFGP_Game
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System.IO;

    /// <summary>
    /// Super basic map implementation, this will be changed
    /// in a big way later. Currently just makes a bunch of
    /// grass tiles to fill the background.
    /// </summary>
    public class Map : IDrawable
    {
        public int TilesAcross;
        public int TilesDown;

        private Sprite<byte>[,] backgroundTiles;

        private string mapDirectory;

        // Pretty big constructor, could be re-organized at some point.
        public Map(string mapFileName)
        {
            mapDirectory = @"C:\Users\Jason\Documents\AI_Project\AIFGP_Project\AIFGP_Game\AIFGP_Game\Map\ascii_maps\";
            loadMapFromText(mapFileName);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Sprite<byte> tile in backgroundTiles)
            {
                tile.Draw(spriteBatch);
            }
        }

        private void loadMapFromText(string mapFileName)
        {
            if (!File.Exists(mapFileName))
            {
                System.Diagnostics.Debug.WriteLine("Map '" + mapFileName
                    + "' does not exist!");
            }

            List<Vector2> wallLocations = new List<Vector2>();

            int numLines = 0;
            string curLine = "";
            StreamReader reader = File.OpenText(mapDirectory + mapFileName);
            while ((curLine = reader.ReadLine()) != null)
            {
                if (numLines == 0)
                {
                    TilesAcross = curLine.Length;
                }

                char[] chars = curLine.ToCharArray();
                for (int i = 0; i < chars.Length; i++)
                    if (chars[i] == 'W')
                        wallLocations.Add(new Vector2(i, numLines));

                numLines++;
            }

            TilesDown = numLines;

            // Map tiles are 28x28 in px.
            Rectangle mapTileDimensions = new Rectangle(0, 0, 28, 28);

            backgroundTiles = new Sprite<byte>[TilesAcross, TilesDown];

            Sprite<byte> grassTile = new Sprite<byte>(SensorsGame.GrassTile, Vector2.Zero, mapTileDimensions);
            grassTile.AddAnimationFrame(0, mapTileDimensions);
            grassTile.ActiveAnimation = 0;

            Sprite<byte> wallTile = new Sprite<byte>(SensorsGame.WallTile, Vector2.Zero, mapTileDimensions);
            wallTile.AddAnimationFrame(0, mapTileDimensions);
            wallTile.ActiveAnimation = 0;

            Vector2 curTilePos = Vector2.Zero;

            int xOffset = (SensorsGame.ScreenDimensions.Width - TilesAcross * mapTileDimensions.Width) / 2;
            int yOffset = (SensorsGame.ScreenDimensions.Height - TilesDown * mapTileDimensions.Height) / 2;
            Rectangle curRect = mapTileDimensions;
            curRect.X = xOffset;
            curRect.Y = yOffset;

            for (int y = 0; y < TilesDown; y++)
            {
                for (int x = 0; x < TilesAcross; x++)
                {
                    curTilePos.X = curRect.X;
                    curTilePos.Y = curRect.Y;

                    Sprite<byte> curTile;

                    Vector2 curLoc = new Vector2(x, y);
                    if (wallLocations.Contains(curLoc))
                    {
                        curTile = new Sprite<byte>(wallTile);

                        // Create a Wall and register it with the WallManager.
                        Wall curWall = new Wall();
                        curWall.TopLeftPixel = curTilePos;
                        curWall.BottomRightPixel = curTilePos + new Vector2(
                            mapTileDimensions.Width, mapTileDimensions.Height);
                        WallManager.Instance.AddWall(curWall);
                    }
                    else
                        curTile = new Sprite<byte>(grassTile);

                    curTile.Position = curTilePos;
                    backgroundTiles[x, y] = curTile;

                    curRect.X += curRect.Width;
                }

                curRect.X = xOffset;
                curRect.Y += mapTileDimensions.Height;
            }
        }
    }
}
