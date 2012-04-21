namespace AIFGP_Game_MapCreation
{
    using AIFGP_Game;
    using AIFGP_Game_Data;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Map subclass used in MapCreator. Basically, this is the same
    /// as Map it just does not generate a navigation graph.
    /// </summary>
    public class EditorMap : Map
    {
        public bool AreHeadersVisible = true;
        public bool AreGridlinesVisible = true;

        private MapGridlines mapGridlines;

        public EditorMap(MapDescription mapDescription)
            : base(mapDescription)
        {
            mapGridlines = new MapGridlines(this);
        }

        public bool TilePosWithinMapBounds(int row, int col)
        {
            return col >= 0 && col < TilesAcross &&
                row >= 0 && row < TilesDown;
        }

        public bool WorldPosWithinMapBounds(Vector2 worldPos)
        {
            Vector2 tilePos = WorldPosToTilePos(worldPos);
            return TilePosWithinMapBounds((int)tilePos.Y, (int)tilePos.X);
        }

        public override void Update(GameTime gameTime)
        {

        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            drawMap(spriteBatch);

            if (AreHeadersVisible)
                drawHeaders(spriteBatch);

            if (AreGridlinesVisible)
                mapGridlines.Draw(spriteBatch);
        }

        protected override void initializeSprites()
        {
            Rectangle tileFrame = new Rectangle(0, 0, (int)TileSize.X, (int)TileSize.Y);

            base.initializeSprites();
        }

        private void drawMap(SpriteBatch spriteBatch)
        {
            foreach (TileInfo tileInfo in mapTiles)
            {
                Sprite<byte> tile;

                if (tileInfo.Type == TileType.Wall)
                    tile = wallTile;
                else if (tileInfo.Type == TileType.Bush)
                {
                    // Hack for now so that the transparent parts
                    // of bush do not show bg color.
                    tile = grassTile;
                    tile.Position = tileInfo.Position;
                    tile.Draw(spriteBatch);

                    tile = bushTile;
                }
                else
                    tile = grassTile;

                tile.Position = tileInfo.Position;
                tile.Draw(spriteBatch);
            }
        }

        private void drawHeaders(SpriteBatch spriteBatch)
        {
            Vector2 headerPos = new Vector2(0.0f, -FontManager.SmallDebugFont.MeasureString("0").Y);
            for (int i = 0; i < TilesAcross; i++)
            {
                spriteBatch.DrawString(FontManager.SmallDebugFont, i.ToString(),
                    headerPos, Color.White);

                headerPos.X += TileSize.X;
            }

            headerPos = Vector2.Zero;
            for (int i = 0; i < TilesDown; i++)
            {
                string iAsStr = i.ToString();

                Vector2 offset = Vector2.Zero;
                offset.X = FontManager.SmallDebugFont.MeasureString(iAsStr).X + 2.0f;

                spriteBatch.DrawString(FontManager.SmallDebugFont, i.ToString(),
                    headerPos - offset, Color.White);

                headerPos.Y += TileSize.Y;
            }
        }

        // Deliberately empty so no navgraph is created!
        protected override void createNavigationGraph() { }
    }
}
