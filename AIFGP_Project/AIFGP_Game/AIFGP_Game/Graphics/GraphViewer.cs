namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using GraphType = Graph<PositionalNode, Edge>;

    public class GraphViewer : IDrawable
    {
        private GraphType g;

        private Sprite<byte> nodeSprite;
        private Dictionary<PositionalNode, Color> nodeColors = new Dictionary<PositionalNode, Color>();

        private Dictionary<Edge, Line> edgeLines = new Dictionary<Edge,Line>();

        private Vector2 idxOffset = new Vector2(5.0f, 10.0f);

        public GraphViewer(GraphType graph)
        {
            Graph = graph;
            
            nodeSprite = new Sprite<byte>(AStarGame.RadarCircle, Vector2.Zero,
                RadarDebugger.SpriteDimensions);
            nodeSprite.AddAnimationFrame(0, RadarDebugger.SpriteDimensions);
            nodeSprite.ActiveAnimation = 0;
            nodeSprite.Scale(0.112f);
        }

        public GraphType Graph
        {
            get { return g; }
            set
            {
                g = value;

                nodeColors.Clear();
                foreach (PositionalNode n in g.Nodes)
                    nodeColors.Add(n, Color.White);

                edgeLines.Clear();
                foreach (Edge e in g.Edges)
                {
                    // Hauntingly similar to EdgeChanged(...)
                    Vector2 nodeToPos = g.GetNode(e.NodeTo).Position;
                    Vector2 nodeFromPos = g.GetNode(e.NodeFrom).Position;
                    
                    Vector2 vecBetween = nodeToPos - nodeFromPos;
                    float lengthBetween = vecBetween.Length();
        
                    Vector2 unitVecBetween;
                    Vector2.Normalize(ref vecBetween, out unitVecBetween);
        
                    Vector2 midPoint = nodeFromPos + (unitVecBetween * lengthBetween / 2);
        
                    edgeLines.Add(e, new Line(midPoint, (int)lengthBetween, Color.LightBlue));
                    
                    float angleFromXToVec = (float)Angles.AngleFromUToV(Vector2.UnitX, vecBetween);
                    edgeLines[e].RotateInRadians(angleFromXToVec);
                }
            }
        }

        public void EdgeChanged(Edge e)
        {
            Vector2 nodeToPos = g.GetNode(e.NodeTo).Position;
            Vector2 nodeFromPos = g.GetNode(e.NodeFrom).Position;
            
            Vector2 vecBetween = nodeToPos - nodeFromPos;
            float lengthBetween = vecBetween.Length();

            Vector2 unitVecBetween;
            Vector2.Normalize(ref vecBetween, out unitVecBetween);

            Vector2 midPoint = nodeFromPos + (unitVecBetween * lengthBetween / 2);

            edgeLines[e] = new Line(midPoint, (int)lengthBetween, Color.LightBlue);
            
            float angleFromXToVec = (float)Angles.AngleFromUToV(Vector2.UnitX, vecBetween);
            edgeLines[e].RotateInRadians(angleFromXToVec);
        }

        public void ChangeNodeColor(PositionalNode n, Color c)
        {
            nodeColors[n] = c;
        }

        public void ChangeEdgeColor(Edge e, Color c)
        {
            edgeLines[e].LineColor = c;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Line l in edgeLines.Values)
                l.Draw(spriteBatch);

            foreach (PositionalNode n in g.Nodes)
            {
                nodeSprite.CenterPosition = n.Position;
                nodeSprite.Color = nodeColors[n];
                nodeSprite.Draw(spriteBatch);

                spriteBatch.DrawString(AStarGame.DebugFont, n.Index.ToString(),
                    n.Position - idxOffset, Color.Yellow);
            }
        }
    }
}
