namespace AIFGP_Game
{
    public class Edge
    {
        private int from = Node.InvalidIndex;
        private int to = Node.InvalidIndex;
        private double weight = 1.0;

        public Edge() { }

        public Edge(int nodeFrom, int nodeTo)
        {
            NodeFrom = nodeFrom;
            NodeTo = nodeTo;
        }

        public Edge(int nodeFrom, int nodeTo, double edgeWeight)
        {
            NodeFrom = nodeFrom;
            NodeTo = nodeTo;
            Weight = edgeWeight;
        }

        public int NodeFrom
        {
            get { return from; }
            set { from = value >= 0 ? value : Node.InvalidIndex; }
        }

        public int NodeTo
        {
            get { return to; }
            set { to = value >= 0 ? value : Node.InvalidIndex; }
        }

        public double Weight
        {
            get { return weight; }
            set { weight = value; }
        }

        public virtual Edge ReverseEdge
        {
            get
            {
                Edge edge = new Edge();
                edge.NodeFrom = NodeTo;
                edge.NodeTo = NodeFrom;
                edge.Weight = Weight;

                return edge;
            }
        }
    }
}
