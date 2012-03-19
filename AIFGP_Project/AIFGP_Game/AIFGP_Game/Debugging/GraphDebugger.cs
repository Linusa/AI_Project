namespace AIFGP_Game
{
    using System;
    using SysDbg = System.Diagnostics.Debug;

    public class GraphDebugger
    {
        // This would be much more effective as a unit test.
        // TODO: Integrate nUnit or something similar.
        public void Run(Graph<Node, Edge> graph)
        {
            // Add nodes 0 and 1, and an curEdge between them.
            graph.AddNode(new Node(graph.AvailableNodeIndex));
            graph.AddNode(new Node(graph.AvailableNodeIndex));
            graph.AddEdge(new Edge(0, 1));

            // Add nodes 2 and 3.
            graph.AddNode(new Node(graph.AvailableNodeIndex));
            graph.AddNode(new Node(graph.AvailableNodeIndex));

            // Connect 0-2, 0-3, 2-3, and change 2-3's actualWeight.
            graph.AddEdge(new Edge(0, 2));
            graph.AddEdge(new Edge(0, 3));
            graph.AddEdge(new Edge(2, 3));
            graph.ChangeEdgeWeight(3, 0, 32.0);

            // Should print all nodes (0, 1, 2, and 3)
            SysDbg.WriteLine("Nodes before first delete.");
            foreach (Node node in graph.Nodes)
                SysDbg.WriteLine("\tNode #" + node.Index);

            // Nodes 0 and 1 should be removed as well as corresponding edges.
            graph.RemoveNode(0);
            graph.RemoveNode(1);

            // Should print nodes 2 and 3.
            SysDbg.WriteLine("Nodes after first delete.");
            foreach (Node node in graph.Nodes)
                SysDbg.WriteLine("\tNode #" + node.Index);

            // Re-add node 0, connect 0-2. (Connect 0-1 and 1-0 not allowed)
            // Also, change 0-2 actualWeight.
            graph.AddNode(new Node(0));
            graph.AddEdge(new Edge(0, 1));
            graph.AddEdge(new Edge(2, 0));
            graph.AddEdge(new Edge(1, 0));
            graph.ChangeEdgeWeight(0, 2, 42.0);

            // Should print 0-2, 2-3, 2-0, and 3-2.
            SysDbg.WriteLine("Current edges.");
            foreach (Edge edge in graph.Edges)
                SysDbg.WriteLine("\tEdge " + edge.NodeFrom + " -> " + edge.NodeTo);

            // Remove 0-2 and 2-0.
            graph.RemoveEdge(2, 0);

            // Should print 2-3 and 3-2.
            SysDbg.WriteLine("Current edges.");
            foreach (Edge edge in graph.Edges)
                SysDbg.WriteLine("\tEdge " + edge.NodeFrom + " -> " + edge.NodeTo);

            SysDbg.WriteLine("Node 3 exists? " + graph.NodeExists(3));
            SysDbg.WriteLine("Node 1 exists? " + graph.NodeExists(1));
            SysDbg.WriteLine("Edge 2-3 exists? " + graph.EdgeExists(2, 3));
            SysDbg.WriteLine("Edge 3-2 exists? " + graph.EdgeExists(3, 2));
            SysDbg.WriteLine("Edge 0-1 exists? " + graph.EdgeExists(0, 1));
            SysDbg.WriteLine("Edge 1-0 exists? " + graph.EdgeExists(1, 0));

            graph.AddNode(new Node(1));
            graph.AddNode(new Node(4));
            graph.AddNode(new Node(5));
            graph.AddNode(new Node(6));
            graph.AddEdge(new Edge(2, 2, 3));
            for (int i = 2; i < 20; i++)
                graph.AddEdge(new Edge(i, 1, 9.999));

            // Should print nodes 0-6.
            SysDbg.WriteLine("Nodes");
            foreach (Node node in graph.Nodes)
                SysDbg.WriteLine("\tNode #" + node.Index);

            SysDbg.WriteLine("Current edges.");
            foreach (Edge edge in graph.Edges)
                SysDbg.WriteLine("\tEdge " + edge.NodeFrom + " -> " + edge.NodeTo);

            SysDbg.WriteLine("Current edges from node 1.");
            foreach (Edge edge in graph.EdgesFromNode(1))
                SysDbg.WriteLine("\tEdge " + edge.NodeFrom + " -> " + edge.NodeTo);

            graph.RemoveEdge(0, 5);
            graph.RemoveEdge(3, 2);

            // These all change the actualWeight.
            graph.ChangeEdgeWeight(1, 2, 9001);
            graph.GetEdge(6, 1).Weight = 33.2112;
            graph.GetEdge(1, 6).Weight = 2112.33;

            // Check for curEdge modification by iteration.
            foreach (Edge e in graph.Edges)
                e.Weight = 1.0;
            double weightMod = 10.0;
            foreach (Edge e in graph.Edges)
            {
                e.Weight += weightMod;
                weightMod += 10.0;
            }

            // Revert all weights back to 1.0.
            foreach (Edge e in graph.Edges)
                e.Weight = 1.0;

            // Check for curEdge modification by iteration over a node's edges.
            foreach (Edge e in graph.EdgesFromNode(1))
                e.Weight = 3.0;

            graph.Clear();

            int numNodes = 15;

            for (int i = 0; i < numNodes; i++)
                graph.AddNode(new Node(graph.AvailableNodeIndex));

            for (int i = 4; i < 7; i++)
                for (int j = 0; j < numNodes; j++)
                    graph.AddEdge(new Edge(i, j, 2.0));

            SysDbg.WriteLine("Current edges.");
            {
                int curEdgeNum = 0;
                foreach (Edge edge in graph.Edges)
                    SysDbg.WriteLine("\tEdge " + curEdgeNum++ + ": " + edge.NodeFrom + " -> " + edge.NodeTo);
            }

            graph.RemoveNode(4);
            
            SysDbg.WriteLine("Current edges.");
            {
                int curEdgeNum = 0;
                foreach (Edge edge in graph.Edges)
                    SysDbg.WriteLine("\tEdge " + curEdgeNum++ + ": " + edge.NodeFrom + " -> " + edge.NodeTo);
            }
        }
    }
}
