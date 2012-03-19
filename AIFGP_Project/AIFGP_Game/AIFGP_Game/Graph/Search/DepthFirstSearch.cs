namespace AIFGP_Game
{
    using System.Collections.Generic;

    /// <summary>
    /// DepthFirstSearch searches a graph by traveling as deep as possible,
    /// and once it gets stuck, backtracks until it can take another path
    /// as deep as possible, and so on.
    /// </summary>
    public class DepthFirstSearch
    {
        public readonly bool TargetFound = false;

        private readonly Graph<Node, Edge> g;

        private int src;
        private int tgt;

        private List<bool> visited;
        private List<int> parents;

        public DepthFirstSearch(Graph<Node, Edge> graph, int source, int target)
        {
            g = graph;
            src = source;
            tgt = target;

            visited = new List<bool>(graph.NodeCount);
            parents = new List<int>(graph.NodeCount);

            for (int i = 0; i < graph.NodeCount; i++)
            {
                visited.Add(false);
                parents.Add(-1);
            }

            bool nodesExist = g.NodeExists(src) && g.NodeExists(tgt);
            TargetFound = nodesExist ? search() : false;
        }
        
        // Reconstruct the path to the target.
        public void PathToTarget(out List<int> path)
        {
            path = new List<int>();

            if (TargetFound)
            {
                int curNode = tgt;
                path.Add(curNode);

                while (curNode != src)
                {
                    curNode = parents[curNode];
                    path.Add(curNode);
                }

                path.Reverse();
            }
        }

        // The depth-first search.
        private bool search()
        {
            bool found = false;

            Stack<Edge> curEdges = new Stack<Edge>();

            Edge startEdge = new Edge(src, src, 0.0);
            curEdges.Push(startEdge);

            while (curEdges.Count > 0)
            {
                Edge curEdge = curEdges.Pop();

                visited[curEdge.NodeTo] = true;
                parents[curEdge.NodeTo] = curEdge.NodeFrom;

                if (curEdge.NodeTo == tgt)
                {
                    found = true;
                    break;
                }

                foreach (Edge e in g.EdgesFromNode(curEdge.NodeTo))
                    if (!visited[e.NodeTo])
                        curEdges.Push(e);
            }

            return found;
        }
    }
}
