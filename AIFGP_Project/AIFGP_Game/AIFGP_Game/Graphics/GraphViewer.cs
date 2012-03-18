namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    using GraphType = Graph<PositionalNode, Edge>;

    public class GraphViewer : IDrawable, IUpdateable
    {
        public bool DisplayNodeIndices = false;

        private GraphType g;

        private Sprite<byte> nodeSprite;
        private Dictionary<PositionalNode, Color> nodeColors = new Dictionary<PositionalNode, Color>();
        protected float nodeRadius;

        private Dictionary<Edge, Line> edgeLines = new Dictionary<Edge,Line>();

        private Vector2 idxOffset = new Vector2(5.0f, 10.0f);

        public GraphViewer(GraphType graph)
        {
            Graph = graph;
            
            nodeSprite = new Sprite<byte>(AStarGame.RadarCircle, Vector2.Zero,
                RadarDebugger.SpriteDimensions);
            nodeSprite.AddAnimationFrame(0, RadarDebugger.SpriteDimensions);
            nodeSprite.ActiveAnimation = 0;

            float scale = 0.06f;
            nodeSprite.Scale(scale);
            nodeRadius = nodeSprite.Dimensions.Width / 2 * scale;
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

        public void ChangeNodeColor(PositionalNode n, Color c)
        {
            nodeColors[n] = c;
        }

        public void ChangeEdgeColor(Edge e, Color c)
        {
            edgeLines[e].LineColor = c;

            // Need to change reverse edge's color too in case it is drawn
            // over top of the line for e.
            foreach (Edge curEdge in g.EdgesFromNode(e.NodeTo))
            {
                if (e.NodeFrom == curEdge.NodeTo && e.NodeTo == curEdge.NodeFrom)
                {
                    edgeLines[curEdge].LineColor = c;
                    break;
                }
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.D4))
                DisplayNodeIndices = true;
            else
                DisplayNodeIndices = false;

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (Line l in edgeLines.Values)
                l.Draw(spriteBatch);

            foreach (PositionalNode n in g.Nodes)
            {
                nodeSprite.CenterPosition = n.Position;
                nodeSprite.Color = nodeColors[n];
                nodeSprite.Draw(spriteBatch);

                if (DisplayNodeIndices)
                    spriteBatch.DrawString(AStarGame.SmallDebugFont, n.Index.ToString(),
                        n.Position - idxOffset, Color.Yellow);
            }
        }
    }
}
