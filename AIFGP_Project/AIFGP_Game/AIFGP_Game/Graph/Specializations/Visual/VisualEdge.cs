namespace AIFGP_Game
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    
    using VisualGraph = Graph<VisualNode, VisualEdge>;

    public class VisualEdge : Edge, IDrawable
    {
        public Line EdgeLine;

        private readonly VisualGraph g;
        
        public VisualEdge(int nodeFrom, int nodeTo, VisualGraph graph)
            : base(nodeFrom, nodeTo)
        {
            g = graph;

            Vector2 nodeToPos = g.GetNode(NodeTo).Position;
            Vector2 nodeFromPos = g.GetNode(NodeFrom).Position;
            
            Vector2 vecBetween = nodeToPos - nodeFromPos;
            float lengthBetween = vecBetween.Length();

            Vector2 unitVecBetween;
            Vector2.Normalize(ref vecBetween, out unitVecBetween);

            Vector2 midPoint = nodeFromPos + (unitVecBetween * lengthBetween / 2);

            EdgeLine = new Line(midPoint, (int)lengthBetween, Color.LightBlue);
            
            float angleFromXToVec = (float)Angles.AngleFromUToV(Vector2.UnitX, vecBetween);
            EdgeLine.RotateInRadians(angleFromXToVec);
        }

        public Color LineColor
        {
            get { return EdgeLine.LineColor; }
            set { EdgeLine.LineColor = value; }
        }
        
        public override Edge Reversed
        {
            get { return new VisualEdge(NodeTo, NodeFrom, g); }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            EdgeLine.Draw(spriteBatch);
        }
    }
}
