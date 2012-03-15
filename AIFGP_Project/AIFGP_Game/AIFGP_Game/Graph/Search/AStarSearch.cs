namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;

    using Heuristic = System.Func<Graph<PositionalNode, Edge>, int, int, double>;

    public class AStarSearch
    {
        public readonly bool TargetFound = false;

        private readonly Graph<PositionalNode, Edge> g;
        private readonly int numNodes;

        private int src;
        private int tgt;

        private Heuristic heuristic;

        private List<Edge> edgeFrontier;
        private List<Edge> shortestPathTree;
        private List<double> weights;
        private List<double> weightsPlusHeuristic;

        public AStarSearch(Graph<PositionalNode, Edge> graph, int source, int target, Heuristic h)
        {
            g = graph;
            src = source;
            tgt = target;

            heuristic = h;

            numNodes = graph.NodeCount;
            edgeFrontier = new List<Edge>(numNodes);
            shortestPathTree = new List<Edge>(numNodes);
            weights = new List<double>(numNodes);
            weightsPlusHeuristic = new List<double>(numNodes);

            for (int i = 0; i < numNodes; i++)
            {
                edgeFrontier.Add(null);
                shortestPathTree.Add(null);
                weights.Add(0.0);
                weightsPlusHeuristic.Add(0.0);
            }

            bool nodesExist = g.NodeExists(src) && g.NodeExists(tgt);
            TargetFound = nodesExist ? search() : false;
        }

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

        public double CostToTarget
        {
            get { return weights[tgt]; }
        }

        private bool search()
        {
            KeyHeap<double> cheapestNodesHeap =
                new KeyHeap<double>(
                    HeapSorting.Min,
                    weightsPlusHeuristic,
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
                    double actualWeight = weights[cheapestNode] + e.Weight;
                    double heuristicWeight = actualWeight + heuristic(g, e.NodeTo, tgt);

                    if (edgeFrontier[e.NodeTo] == null)
                    {
                        weights[e.NodeTo] = actualWeight;
                        weightsPlusHeuristic[e.NodeTo] = heuristicWeight;
                        cheapestNodesHeap.Insert(e.NodeTo);
                        edgeFrontier[e.NodeTo] = e;
                    }
                    else if (actualWeight < weights[e.NodeTo] && shortestPathTree[e.NodeTo] == null)
                    {
                        weights[e.NodeTo] = actualWeight;
                        weightsPlusHeuristic[e.NodeTo] = heuristicWeight;
                        cheapestNodesHeap.Reorder(e.NodeTo);
                        edgeFrontier[e.NodeTo] = e;
                    }
                }
            }

            return false;
        }
    }
}
