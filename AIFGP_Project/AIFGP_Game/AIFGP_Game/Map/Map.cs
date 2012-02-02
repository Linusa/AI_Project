namespace AIFGP_Game
{
    using System;
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

        public Map()
        {
            // Grass tile is 28x28 in px.
            Rectangle frameRect = new Rectangle(0, 0, 28, 28);

            int TilesAcross = (int)(SensorsGame.ScreenDimensions.Width / frameRect.Width) + 1;
            int TilesDown = (int)(SensorsGame.ScreenDimensions.Height / frameRect.Height) + 1;

            backgroundTiles = new Sprite<byte>[TilesAcross, TilesDown];

            Vector2 curTilePos = Vector2.Zero;
            Rectangle curRect = frameRect;
            for (int i = 1; i <= TilesDown; i++)
            {
                for (int j = 1; j <= TilesAcross; j++)
                {
                    curTilePos.X = curRect.X;
                    curTilePos.Y = curRect.Y;
                    Sprite<byte> curTile = new Sprite<byte>(SensorsGame.GrassTile, curTilePos, frameRect);
                    curTile.AddAnimationFrame(0, frameRect);
                    curTile.ActiveAnimation = 0;
                    backgroundTiles[j - 1, i - 1] = curTile;

                    curRect.X += curRect.Width;
                }

                curRect.X = 0;
                curRect.Y = i * frameRect.Height;
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
