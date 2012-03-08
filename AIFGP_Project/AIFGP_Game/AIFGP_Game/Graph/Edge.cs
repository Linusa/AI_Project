namespace AIFGP_Game
{
    public class Edge
    {
        private int from = Node.InvalidIndex;
        private int to = Node.InvalidIndex;
        private double weight = 1.0;

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
            protected set { from = value >= 0 ? value : Node.InvalidIndex; }
        }

        public int NodeTo
        {
            get { return to; }
            protected set { to = value >= 0 ? value : Node.InvalidIndex; }
        }

        public double Weight
        {
            get { return weight; }
            set { weight = value; }
        }

        public virtual Edge Reversed
        {
            get { return new Edge(NodeTo, NodeFrom, Weight); }
        }
    }
}
