namespace AIFGP_Game
{
    using System.Collections.Generic;

    /// <summary>
    /// BreadthFirstSearch searches a graph by levels of depth. From the
    /// source, it searches all adjacent nodes, then from each adjacent node,
    /// it examines nodes adjacent to it and so on.
    /// </summary>
    public class BreadthFirstSearch
    {
        public readonly bool TargetFound = false;

        private readonly Graph<Node, Edge> g;

        private int src;
        private int tgt;

        private List<bool> visited;
        private List<int> parents;

        public BreadthFirstSearch(Graph<Node, Edge> graph, int source, int target)
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

        // The breadth-first search.
        private bool search()
        {
            bool found = false;

            Queue<Edge> curEdges = new Queue<Edge>();
            
            Edge startEdge = new Edge(src, src, 0.0);
            curEdges.Enqueue(startEdge);
            visited[src] = true;

            while (curEdges.Count > 0)
            {
                Edge curEdge = curEdges.Dequeue();
                
                parents[curEdge.NodeTo] = curEdge.NodeFrom;
                
                if (curEdge.NodeTo == tgt)
                {
                    found = true;
                    break;
                }

                foreach (Edge e in g.EdgesFromNode(curEdge.NodeTo))
                {
                    if (!visited[e.NodeTo])
                    {
                        curEdges.Enqueue(e);
                        visited[e.NodeTo] = true;
                    }
                }
            }

            return found;
        }
    }
}
