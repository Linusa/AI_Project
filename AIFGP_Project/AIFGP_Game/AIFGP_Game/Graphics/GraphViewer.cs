namespace AIFGP_Game
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    using GraphType = Graph<PositionalNode, Edge>;

    /// <summary>
    /// GraphViewer visually displays a graph of PositionalNode's
    /// and Edges.
    /// </summary>
    public class GraphViewer : IDrawable, IUpdateable
    {
        public bool DisplayGraph = false;
        public bool DisplayNodeIndices = false;

        public Color NodeColor = Color.Transparent;
        public Color EdgeColor = Color.Gray;

        protected GraphType g;

        private Sprite<byte> nodeSprite;
        private Dictionary<int, Color> nodeColors = new Dictionary<int, Color>();
        protected float nodeRadius;

        private Dictionary<Edge, Line> edgeLines = new Dictionary<Edge,Line>();

        private Vector2 idxOffset = new Vector2(5.0f, 10.0f);

        private Timer inputTimer = new Timer(0.1f);

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

            inputTimer.Start();
        }

        public GraphType Graph
        {
            get { return g; }
            set
            {
                g = value;

                nodeColors.Clear();
                foreach (PositionalNode n in g.Nodes)
                    nodeColors.Add(n.Index, NodeColor);

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

                    edgeLines.Add(e, new Line(midPoint, (int)lengthBetween, EdgeColor));

                    float angleFromXToVec = (float)Angles.AngleFromUToV(Vector2.UnitX, vecBetween);
                    edgeLines[e].RotateInRadians(angleFromXToVec);
                }
            }
        }

        public void ChangeNodeColor(PositionalNode n, Color c)
        {
            nodeColors[n.Index] = c;
        }

        public void ChangeEdgeColor(Edge e, Color c)
        {
            edgeLines[e].LineColor = c;

            // Need to change reverse curEdge's color too in case it is drawn
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

        protected virtual void handleInput(KeyboardState keyboard, MouseState mouse)
        {
            if (keyboard.IsKeyDown(Keys.D4))
                DisplayGraph = !DisplayGraph;

            if (DisplayGraph)
            {
                if (keyboard.IsKeyDown(Keys.D5))
                    DisplayNodeIndices = !DisplayNodeIndices;
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            if (inputTimer.Expired(gameTime))
            {
                KeyboardState keyboardState = Keyboard.GetState();
                MouseState mouseState = Mouse.GetState();
                handleInput(keyboardState, mouseState);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (DisplayGraph)
            {
                foreach (Line l in edgeLines.Values)
                    l.Draw(spriteBatch);

                foreach (PositionalNode n in g.Nodes)
                {
                    nodeSprite.CenterPosition = n.Position;
                    nodeSprite.Color = nodeColors[n.Index];
                    nodeSprite.Draw(spriteBatch);

                    if (DisplayNodeIndices)
                    {
                        spriteBatch.DrawString(AStarGame.SmallDebugFont,
                            n.Index.ToString(), n.Position - idxOffset,
                            Color.Yellow);
                    }
                }
            }
        }
    }
}
