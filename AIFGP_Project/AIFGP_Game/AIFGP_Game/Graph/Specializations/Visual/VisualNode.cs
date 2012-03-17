namespace AIFGP_Game
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class VisualNode : PositionalNode, IDrawable
    {
        public Sprite<byte> NodeSprite;

        private string nodeIdxStr;
        private Vector2 nodeIdxLoc;

        public VisualNode(int index, Vector2 position)
            : base(index, position)
        {
            nodeIdxLoc = position - new Vector2(5.0f, 10.0f);
            nodeIdxStr = index.ToString();

            NodeSprite = new Sprite<byte>(AStarGame.RadarCircle, position, RadarDebugger.SpriteDimensions);
            NodeSprite.AddAnimationFrame(0, RadarDebugger.SpriteDimensions);
            NodeSprite.ActiveAnimation = 0;
            NodeSprite.Scale(0.112f);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            NodeSprite.Draw(spriteBatch);
            spriteBatch.DrawString(AStarGame.DebugFont, nodeIdxStr,
                nodeIdxLoc, Color.Yellow);
        }
    }
}
