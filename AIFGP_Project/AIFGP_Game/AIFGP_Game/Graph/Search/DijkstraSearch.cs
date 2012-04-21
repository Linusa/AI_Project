namespace AIFGP_Game
{
    using System.Collections.Generic;

    /// <summary>
    /// DijkstraSearch searches a graph using Dijkstra's algorithm. It
    /// creates a shortest path tree from the source to all nodes
    /// encountered until the target is reached.  If a target is not
    /// specified, the shortest path tree will have the shortest path
    /// from the source to all nodes in the graph, provided the graph
    /// is connected.
    /// </summary>
    public class DijkstraSearch
    {
        public readonly bool TargetFound = false;

        private readonly Graph<Node, Edge> g;
        private readonly int numNodes;

        private int src;
        private int tgt;

        private List<Edge> edgeFrontier;
        private List<Edge> shortestPathTree;
        private List<double> accumulativeWeights;

        public DijkstraSearch(Graph<Node, Edge> graph, int source, int target = -1)
        {
            g = graph;
            src = source;
            tgt = target;

            numNodes = graph.NodeCount;
            edgeFrontier = new List<Edge>(numNodes);
            shortestPathTree = new List<Edge>(numNodes);
            accumulativeWeights = new List<double>(numNodes);

            for (int i = 0; i < numNodes; i++)
            {
                edgeFrontier.Add(null);
                shortestPathTree.Add(null);
                accumulativeWeights.Add(0.0);
            }

            bool sourceExists = g.NodeExists(src);
            TargetFound = sourceExists ? search() : false;
        }

        // Reconstruct the path to the target.
        public void PathToTarget(out List<int> path)
        {
            path = new List<int>();

            if (TargetFound)
            {
                int curNode = tgt;
                path.Add(curNode);

                while (curNode != src && shortestPathTree[curNode] != null)
                {
                    curNode = shortestPathTree[curNode].NodeFrom;
                    path.Add(curNode);
                }

                path.Reverse();
            }
        }

        public void ShortestPathTree(out List<Edge> tree)
        {
            tree = shortestPathTree;
        }

        public double CostToTarget
        {
            get { return accumulativeWeights[tgt]; }
        }

        public double CostToNode(int nodeIndex)
        {
            return accumulativeWeights[nodeIndex];
        }

        // Dijkstra's algorithm.
        private bool search()
        {
            // KeyHeap is used as a heap that sorts indices
            // based on the elements at their locations in
            // accumulativeWeights.
            KeyHeap<double> cheapestNodesHeap =
                new KeyHeap<double>(
                    HeapSorting.Min,
                    accumulativeWeights,
                    numNodes
                );
            
            cheapestNodesHeap.Insert(src);
            while (!cheapestNodesHeap.IsEmpty)
            {
                int cheapestNode = cheapestNodesHeap.Remove();
                shortestPathTree[cheapestNode] = edgeFrontier[cheapestNode];

                if (cheapestNode == tgt)
                    return true;

                foreach (Edge e in g.EdgesFromNode(cheapestNode))
                {
                    double weight = accumulativeWeights[cheapestNode] + e.Weight;

                    if (edgeFrontier[e.NodeTo] == null)
                    {
                        accumulativeWeights[e.NodeTo] = weight;
                        cheapestNodesHeap.Insert(e.NodeTo);
                        edgeFrontier[e.NodeTo] = e;
                    }
                    else if (weight < accumulativeWeights[e.NodeTo] && shortestPathTree[e.NodeTo] == null)
                    {
                        accumulativeWeights[e.NodeTo] = weight;
                        cheapestNodesHeap.Reorder(e.NodeTo);
                        edgeFrontier[e.NodeTo] = e;
                    }
                }
            }

            // If no target was specified (or target did not exist) search
            // returns false. The instance still holds valuable info though
            // in the shortest path tree and the accumulative weights.
            return false;
        }
    }
}
