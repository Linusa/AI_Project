namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;

    using DFS = DepthFirstSearch;
    using SysDbg = System.Diagnostics.Debug;

    public class DFSDebugger
    {
        // This would be much more effective as a unit test.
        // TODO: Integrate nUnit or something similar.
        public void Run()
        {
            int testNum = 1;

            Graph<Node, Edge> graph = new Graph<Node, Edge>();
            graph.AddNode(new Node(graph.AvailableNodeIndex));
            graph.AddNode(new Node(graph.AvailableNodeIndex));
            graph.AddNode(new Node(graph.AvailableNodeIndex));
            graph.AddNode(new Node(graph.AvailableNodeIndex));
            graph.AddEdge(new Edge(0, 1));
            graph.AddEdge(new Edge(1, 2));
            graph.AddEdge(new Edge(2, 3));

            SysDbg.WriteLine("Test " + testNum++ + ":");
            outputResults(graph, 0, 3);

            graph.Clear();

            int numNodes = 7;
            for (int i = 0; i < numNodes; i++)
                graph.AddNode(new Node(graph.AvailableNodeIndex));

            graph.AddEdge(new Edge(0, 1));
            graph.AddEdge(new Edge(0, 2));
            graph.AddEdge(new Edge(0, 3));
            graph.AddEdge(new Edge(1, 4));
            graph.AddEdge(new Edge(1, 5));
            graph.AddEdge(new Edge(2, 6));
            graph.AddEdge(new Edge(3, 5));

            SysDbg.WriteLine("Test " + testNum++ + ":");
            outputResults(graph, 0, 0);
            outputResults(graph, 0, 1);
            outputResults(graph, 0, 2);
            outputResults(graph, 0, 3);
            outputResults(graph, 0, 4);
            outputResults(graph, 4, 6);
            outputResults(graph, 6, 4);
            outputResults(graph, 3, 1);
            outputResults(graph, 3, 0);
            outputResults(graph, 1, 4);
            outputResults(graph, 5, 5);
            outputResults(graph, 7, 7);
        }

        private void outputResults(Graph<Node, Edge> graph, int src, int tgt)
        {
            SysDbg.Write("\t");

            DFS dfs = new DFS(graph, src, tgt);

            if (dfs.TargetFound)
            {
                SysDbg.Write("DFS from " + src + " to " + tgt + ": ");
                List<int> path;
                dfs.PathToTarget(out path);
                foreach (int node in path)
                    SysDbg.Write(node + " ");

                SysDbg.WriteLine("");
            }
            else
                SysDbg.WriteLine("DFS search could not find " + tgt
                    + " from " + src + "!");
        }
    }
}
