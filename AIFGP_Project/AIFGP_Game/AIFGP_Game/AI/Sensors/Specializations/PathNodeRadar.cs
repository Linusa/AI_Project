namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    
    using GraphType = Graph<PositionalNode, Edge>;

    public class PathNodeRadar : ISensor
    {
        public float Range = 50.0f;

        private GraphType g;

        public PathNodeRadar(Vector2 position, GraphType graph)
        {
            Position = position;
            g = graph;
        }
        
        public void AdjacentNodes(out List<PositionalNode> adjacentNodes)
        {
            adjacentNodes = new List<PositionalNode>();
            float rangeSquared = Range * Range;

            foreach (PositionalNode node in g.Nodes)
            {
                Vector2 vecToNode = node.Position - Position;
                float distToNodeSquared = vecToNode.LengthSquared();
                
                if (distToNodeSquared < rangeSquared)
                    adjacentNodes.Add(node);
            }
        }

        public Vector2 Position
        {
            get;
            set;
        }

        // A radar does not need to be rotated since it is a circle.
        public void RotateInRadians(float radians) { }
        public void RotateInDegrees(float degrees) { }

        public void Update(GameTime gameTime) { }
        public void Draw(SpriteBatch spriteBatch) { }
    }
}
