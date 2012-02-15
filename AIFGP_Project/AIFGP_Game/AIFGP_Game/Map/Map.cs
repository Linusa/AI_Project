namespace AIFGP_Game
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Super basic map implementation, this will be changed
    /// in a big way later. Currently just makes a bunch of
    /// grass tiles to fill the background.
    /// </summary>
    public class Map : IDrawable
    {
        private Sprite<byte>[,] backgroundTiles;

        public int TilesAcross;
        public int TilesDown;

        // Pretty big constructor, could be re-organized at some point.
        public Map()
        {
            // Map tiles are 28x28 in px.
            Rectangle mapTileDimensions = new Rectangle(0, 0, 28, 28);

            int TilesAcross = (int)(SensorsGame.ScreenDimensions.Width / mapTileDimensions.Width) + 1;
            int TilesDown = (int)(SensorsGame.ScreenDimensions.Height / mapTileDimensions.Height) + 1;

            backgroundTiles = new Sprite<byte>[TilesAcross, TilesDown];

            Sprite<byte> grassTile = new Sprite<byte>(SensorsGame.GrassTile, Vector2.Zero, mapTileDimensions);
            grassTile.AddAnimationFrame(0, mapTileDimensions);
            grassTile.ActiveAnimation = 0;

            Sprite<byte> wallTile = new Sprite<byte>(SensorsGame.WallTile, Vector2.Zero, mapTileDimensions);
            wallTile.AddAnimationFrame(0, mapTileDimensions);
            wallTile.ActiveAnimation = 0;

            // Hard-coded wall locations.
            List<Vector2> wallLocations = new List<Vector2>();
            Vector2 curWallPos = new Vector2(TilesAcross / 2, TilesDown / 5);
            for (int x = 0; x < 10; x++)
            {
                wallLocations.Add(curWallPos);
                curWallPos.X++;
            }
            for (int y = 0; y < 10; y++)
            {
                wallLocations.Add(curWallPos);
                curWallPos.Y++;
            }

            Vector2 curTilePos = Vector2.Zero;
            Rectangle curRect = mapTileDimensions;
            for (int x = 1; x <= TilesAcross; x++)
            {
                for (int y = 1; y <= TilesDown; y++)
                {
                    curTilePos.X = curRect.X;
                    curTilePos.Y = curRect.Y;

                    Sprite<byte> curTile;

                    Vector2 curLoc = new Vector2(x-1, y-1);
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
                    backgroundTiles[x-1, y-1] = curTile;

                    curRect.Y += curRect.Height;
                }

                curRect.X = x * mapTileDimensions.Height;
                curRect.Y = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Sprite<byte> tile in backgroundTiles)
            {
                tile.Draw(spriteBatch);
            }
        }
    }
}
