namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;

    public class DijkstraSearch
    {
        public readonly bool TargetFound = false;

        private readonly Graph<Node, Edge> g;

        private int src;
        private int tgt = -1;

        private List<Edge> edgeFrontier;
        private List<Edge> shortestPathTree;
        private List<double> accumulativeWeights;

        public DijkstraSearch(Graph<Node, Edge> graph, int source, int target)
        {
            g = graph;
            src = source;
            tgt = target;

            edgeFrontier = new List<Edge>(graph.NodeCount);
            shortestPathTree = new List<Edge>(graph.NodeCount);
            accumulativeWeights = new List<double>(graph.NodeCount);

            for (int i = 0; i < graph.NodeCount; i++)
            {
                edgeFrontier.Add(null);
                shortestPathTree.Add(null);
            }

            bool sourceExists = g.NodeExists(src);
            TargetFound = sourceExists ? search() : false;
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

        private bool search()
        {
            return false;
        }
    }
}
