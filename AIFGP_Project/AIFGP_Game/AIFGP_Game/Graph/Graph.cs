namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;

    using SysDbg = System.Diagnostics.Debug;

    public class Graph<NodeType, EdgeType>
        where NodeType : Node
        where EdgeType : Edge
    {
        private List<NodeType> nodes = new List<NodeType>();
        private List<List<EdgeType>> edges = new List<List<EdgeType>>();

        private int availableNodeIndex = 0;

        public int NodeCount
        {
            get
            {
                int numNodes = 0;
                for (int i = 0; i < nodes.Count; i++)
                    if (nodes[i].Index != Node.InvalidIndex)
                        numNodes++;

                return numNodes;
            }
        }

        public int EdgeCount
        {
            get
            {
                int numEdges = 0;
                for (int i = 0; i < edges.Count; i++)
                    numEdges += edges[i].Count;

                return numEdges;
            }
        }

        public int AvailableNodeIndex
        {
            get { return availableNodeIndex; }
        }

        public bool NodeExists(int index)
        {
            return index >= 0
                && index < nodes.Count
                && nodes[index].Index != Node.InvalidIndex;
        }

        public bool NodeInactive(int index)
        {
            return index >= 0
                && index < nodes.Count
                && nodes[index].Index == Node.InvalidIndex;
        }

        public bool EdgeExists(int nodeFrom, int nodeTo)
        {
            bool exists = false;
            if (NodeExists(nodeFrom) && NodeExists(nodeTo))
            {
                foreach (EdgeType edge in EdgesFromNode(nodeFrom))
                {
                    if (edge.NodeTo == nodeTo)
                    {
                        exists = true;
                        break;
                    }
                }
            }

            return exists;
        }

        public NodeType GetNode(int index)
        {
            if (NodeExists(index))
                return nodes[index];
            
            SysDbg.WriteLine("Graph.GetNode(" + index
                + ") received invalid index!");

            return null;
        }

        public EdgeType GetEdge(int nodeFrom, int nodeTo)
        {
            if (NodeExists(nodeFrom) && NodeExists(nodeTo))
            {
                foreach (EdgeType edge in EdgesFromNode(nodeFrom))
                    if (edge.NodeTo == nodeTo)
                        return edge;
            }

            SysDbg.WriteLine("Graph.GetEdge(" + nodeFrom
                + ", " + nodeTo + ") returning null!");

            return null;
        }

        public void AddNode(NodeType node)
        {
            if (node.Index == AvailableNodeIndex)
            {
                nodes.Add(node);
                edges.Add(new List<EdgeType>());

                availableNodeIndex++;
            }
            else if (NodeInactive(node.Index))
            {
                nodes[node.Index] = node;
            }
            else
            {
                SysDbg.WriteLine("Graph.AddNode: Could not add node!");
            }
        }

        public void AddEdge(EdgeType edge)
        {
            if (edgeCanBeAdded(edge.NodeFrom, edge.NodeTo))
            {
                edges[edge.NodeFrom].Add(edge);

                EdgeType edgeReversed = (EdgeType)edge.Reversed;
                edges[edgeReversed.NodeFrom].Add(edgeReversed);
            }
        }

        public void RemoveNode(int index)
        {
            if (NodeExists(index))
            {
                // Remove all edges that go to removed node.
                foreach (EdgeType edge in EdgesFromNode(index))
                    edges[edge.NodeTo].RemoveAll(e => e.NodeTo == index);
                
                // Mark node as invalid (don't actually delete it)
                nodes[index].Index = Node.InvalidIndex;

                // Remove the removed node's edges.
                edges[index].Clear();
            }
        }

        public void RemoveEdge(int nodeFrom, int nodeTo)
        {
            if (EdgeExists(nodeFrom, nodeTo))
            {
                edges[nodeFrom].RemoveAt(edges[nodeFrom].FindIndex(e => e.NodeTo == nodeTo));
                edges[nodeTo].RemoveAt(edges[nodeTo].FindIndex(e => e.NodeTo == nodeFrom));
            }
        }

        public void ChangeEdgeWeight(int nodeFrom, int nodeTo, double weight)
        {
            if (EdgeExists(nodeFrom, nodeTo))
            {
                for (int i = 0; i < edges[nodeFrom].Count; i++)
                {
                    if (edges[nodeFrom][i].NodeTo == nodeTo)
                    {
                        edges[nodeFrom][i].Weight = weight;
                        break;
                    }
                }

                for (int i = 0; i < edges[nodeTo].Count; i++)
                {
                    if (edges[nodeTo][i].NodeTo == nodeFrom)
                    {
                        edges[nodeTo][i].Weight = weight;
                        break;
                    }
                }
            }
        }

        public bool IsEmpty
        {
            get
            {
                bool empty = true;
                foreach (NodeType node in nodes)
                {
                    if (node.Index != Node.InvalidIndex)
                    {
                        empty = false;
                        break;
                    }
                }

                return empty;
            }
        }

        public IEnumerable<NodeType> Nodes
        {
            get
            {
                for (int i = 0; i < nodes.Count; i++)
                    if (nodes[i].Index != Node.InvalidIndex)
                        yield return nodes[i];
            }
        }
        
        public IEnumerable<EdgeType> Edges
        {
            get
            {
                for (int i = 0; i < edges.Count; i++)
                {
                    for (int j = 0; j < edges[i].Count; j++)
                    {
                        int from = edges[i][j].NodeFrom;
                        int to = edges[i][j].NodeTo;

                        // Explicit NodeExists inlining here to make
                        // iteration as fast as possible.
                        bool fromExists = from >= 0
                            && from < nodes.Count
                            && nodes[from].Index != Node.InvalidIndex;

                        bool toExists = to >= 0
                            && to < nodes.Count
                            && nodes[to].Index != Node.InvalidIndex;

                        if (fromExists && toExists)
                            yield return edges[i][j];
                    }
                }
            }
        }

        public IEnumerable<EdgeType> EdgesFromNode(int nodeIndex)
        {
            bool fromNodeExists = NodeExists(nodeIndex);
            for (int i = 0; i < edges[nodeIndex].Count; i++)
                if (fromNodeExists && NodeExists(edges[nodeIndex][i].NodeTo))
                    yield return edges[nodeIndex][i];
        }

        public void Clear()
        {
            nodes.Clear();
            edges.Clear();
            availableNodeIndex = 0;
        }

        public void ClearAllEdges()
        {
            for (int i = 0; i < edges.Count; i++)
                edges[i].Clear();
        }

        private bool edgeCanBeAdded(int nodeFrom, int nodeTo)
        {
            bool canBeAdded = NodeExists(nodeFrom) && NodeExists(nodeTo)
                && nodeFrom != nodeTo;

            if (canBeAdded)
            {
                foreach (EdgeType edge in EdgesFromNode(nodeFrom))
                {
                    if (edge.NodeTo == nodeTo)
                    {
                        canBeAdded = false;
                        break;
                    }
                }
            }

            return canBeAdded;
        }
    }
}
