using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AIFGP_Game;

namespace AIFGP_Game_MapCreation
{
    public class MapCreator : DrawableGameComponent
    {
        public readonly int TilesAcross;
        public readonly int TilesDown;
        public readonly Vector2 TileSize;

        private SpriteBatch spriteBatch;

        public MapCreator(Game game, int numRows, int numCols, Vector2 tileSize)
            : base(game)
        {
            TilesAcross = numRows;
            TilesDown = numCols;
            TileSize = tileSize;

            spriteBatch = new SpriteBatch(game.GraphicsDevice);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
        }

        public override void Draw(GameTime gameTime)
        {
        }
    }
}
