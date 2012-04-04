namespace AIFGP_Game
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    using GraphType = Graph<PositionalNode, Edge>;

    /// <summary>
    /// GraphSearchViewer displays a graph of PositionalNode's
    /// Edge's just like GraphViewer, only it also allows the
    /// user to select source and target nodes for A* searches.
    /// If no source is selected, selecting a target will
    /// instruct the player's entity to follow the A* path
    /// to the target.
    /// </summary>
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
                        Vector2 mouseVec = AStarGame.MousePositionInWorld();
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

        // Depending on whether or not source and target nodes have
        // been selected, this method will display the A* path between
        // source and target, or instruct the player's entity to move
        // along the A* path to the target.
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

                    //if (nodePositions[0] == player.Position)
                    //    nodePositions.RemoveAt(0);

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
