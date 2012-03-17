namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using VisualGraph = Graph<VisualNode, VisualEdge>;

    public class VisualGraphDebugger : IDrawable
    {
        private VisualGraph g;

        public VisualGraphDebugger(VisualGraph graph)
        {
            g = graph;
        }

        public VisualGraph Graph
        {
            get { return g; }
            set { g = value; }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (VisualEdge e in g.Edges)
                e.Draw(spriteBatch);

            foreach (VisualNode n in g.Nodes)
                n.Draw(spriteBatch);
        }
    }
}
