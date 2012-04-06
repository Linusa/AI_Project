
namespace AIFGP_Game_MapCreation
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using AIFGP_Game;

    public class MapGridlines : AIFGP_Game.IDrawable
    {
        private List<Line> gridlines;

        public MapGridlines(Map map)
        {
            gridlines = new List<Line>(map.TilesAcross + map.TilesDown);

            storeVerticalLines(map);
            storeHorizontalLines(map);
        }

        private void storeVerticalLines(Map map)
        {
            Vector2 centerPos = map.TileInfoAtTilePos(map.TilesDown / 2, 0).Position;
            centerPos.X += map.TileSize.X;

            int verticalLineLength = map.TilesDown * (int)map.TileSize.Y;

            for (int i = 1; i < map.TilesAcross; i++)
            {
                gridlines.Add(new Line(centerPos, verticalLineLength, Color.White));
                gridlines[i-1].RotateInDegrees(90.0f);

                centerPos.X += map.TileSize.X;
            }
        }

        private void storeHorizontalLines(Map map)
        {
            Vector2 centerPos = map.TileInfoAtTilePos(0, map.TilesAcross / 2).Position;
            centerPos.Y += map.TileSize.Y;

            int horizontalLineLength = map.TilesAcross * (int)map.TileSize.X;

            for (int i = 1; i < map.TilesDown; i++)
            {
                gridlines.Add(new Line(centerPos, horizontalLineLength, Color.White));
                centerPos.Y += map.TileSize.Y;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Line line in gridlines)
                line.Draw(spriteBatch);
        }
    }
}
