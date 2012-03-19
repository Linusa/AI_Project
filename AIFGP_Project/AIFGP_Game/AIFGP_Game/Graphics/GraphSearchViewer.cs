namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    using GraphType = Graph<PositionalNode, Edge>;

    public class GraphSearchViewer : GraphViewer
    {
        public PositionalNode SourceNode;
        public PositionalNode TargetNode;

        private List<int> path = new List<int>();

        public GraphSearchViewer(GraphType graph)
            : base(graph)
        {
            // Do nothing! :D
        }

        protected override void handleInput(KeyboardState keyboard, MouseState mouse)
        {
            base.handleInput(keyboard, mouse);

            if (DisplayGraph)
            {
                if (keyboard.IsKeyDown(Keys.D0))
                    clearPathDisplay();

                bool leftAlt = keyboard.IsKeyDown(Keys.LeftAlt);
                bool rightAlt = keyboard.IsKeyDown(Keys.RightAlt);

                if (leftAlt || rightAlt)
                {
                    bool leftMouseButton = mouse.LeftButton == ButtonState.Pressed;

                    if (leftMouseButton)
                    {
                        Vector2 mouseVec = new Vector2(mouse.X, mouse.Y);
                        checkForClickedNode(mouseVec, leftAlt, rightAlt);

                        updatePathDisplay();
                    }
                }
            }
        }

        private void clearPathDisplay()
        {
            if (SourceNode != null && TargetNode != null)
            {
                for (int i = 0; i < path.Count - 1; i++)
                    ChangeEdgeColor(g.GetEdge(path[i], path[i + 1]), EdgeColor);

                ChangeNodeColor(SourceNode, NodeColor);
                ChangeNodeColor(TargetNode, NodeColor);

                SourceNode = null;
                TargetNode = null;
            }
            else if (SourceNode != null && TargetNode == null)
            {
                ChangeNodeColor(SourceNode, NodeColor);
                SourceNode = null;
            }
            else if (SourceNode == null && TargetNode != null)
            {
                for (int i = 0; i < path.Count - 1; i++)
                    ChangeEdgeColor(g.GetEdge(path[i], path[i + 1]), EdgeColor);

                ChangeNodeColor(TargetNode, NodeColor);
                TargetNode = null;
            }
        }

        private void checkForClickedNode(Vector2 clickPosition,
            bool leftAltPressed, bool rightAltPressed)
        {
            PositionalNode nodeClicked = null;
            PathNodeRadar nodeRadar = new PathNodeRadar(clickPosition, g);

            List<PositionalNode> adjacentNodes;
            nodeRadar.AdjacentNodes(out adjacentNodes);

            foreach (PositionalNode n in adjacentNodes)
            {
                Vector2 mouseToNode = n.Position - clickPosition;

                if (mouseToNode.LengthSquared() < nodeRadius * nodeRadius)
                {
                    nodeClicked = n;
                    break;
                }
            }
            
            if (nodeClicked != null)
            {
                if (leftAltPressed && !rightAltPressed)
                {
                    if (SourceNode != null)
                        ChangeNodeColor(SourceNode, NodeColor);

                    SourceNode = nodeClicked;
                    ChangeNodeColor(nodeClicked, Color.White);
                }
                else if (!leftAltPressed && rightAltPressed)
                {
                    if (TargetNode != null)
                        ChangeNodeColor(TargetNode, NodeColor);

                    TargetNode = nodeClicked;
                    ChangeNodeColor(nodeClicked, Color.Black);
                }
            }
        }

        private void updatePathDisplay()
        {
            IGameEntity player = EntityManager.Instance.GetPlayer();

            if (SourceNode == null && TargetNode != null && !player.FollowingPath)
            {
                PathNodeRadar nodeRadar = new PathNodeRadar(player.Position, g);
    
                List<PositionalNode> adjacentNodes;
                nodeRadar.AdjacentNodes(out adjacentNodes);

                PositionalNode nodeClosestToPlayer = null;
                float minDistSquared = float.MaxValue;
                foreach (PositionalNode n in adjacentNodes)
                {
                    float distSquared = (n.Position - player.Position).LengthSquared();

                    if (distSquared < minDistSquared)
                    {
                        minDistSquared = distSquared;
                        nodeClosestToPlayer = n;
                    }
                }

                AStarSearch search = new AStarSearch(g, nodeClosestToPlayer.Index,
                    TargetNode.Index, AStarHeuristics.Distance);

                if (search.TargetFound)
                {
                    // Change old path back to default curEdge color.
                    for (int i = 0; i < path.Count - 1; i++)
                        ChangeEdgeColor(g.GetEdge(path[i], path[i + 1]), EdgeColor);

                    search.PathToTarget(out path);
                    
                    // Change new path to contrasting curEdge color.
                    for (int i = 0; i < path.Count - 1; i++)
                        ChangeEdgeColor(g.GetEdge(path[i], path[i + 1]), Color.Red);

                    List<Vector2> nodePositions = new List<Vector2>();
                    for (int i = 0; i < path.Count; i++)
                        nodePositions.Add(g.GetNode(path[i]).Position);

                    player.FollowPath(nodePositions);
                }
            }
            else if (SourceNode != null && TargetNode != null)
            {
                AStarSearch search = new AStarSearch(g, SourceNode.Index,
                    TargetNode.Index, AStarHeuristics.Distance);

                if (search.TargetFound)
                {
                    // Change old path back to default curEdge color.
                    for (int i = 0; i < path.Count - 1; i++)
                        ChangeEdgeColor(g.GetEdge(path[i], path[i + 1]), EdgeColor);

                    search.PathToTarget(out path);
                    
                    // Change new path to contrasting curEdge color.
                    for (int i = 0; i < path.Count - 1; i++)
                        ChangeEdgeColor(g.GetEdge(path[i], path[i + 1]), Color.Red);
                }
            }
        }
    }
}
