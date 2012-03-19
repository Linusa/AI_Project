namespace AIFGP_Game
{
    /// <summary>
    /// Node represents a node/vertex in a graph and stores its index
    /// into the graph's nodes. This is to be used as the superclass
    /// for any specialized nodes.
    /// </summary>
    public class Node
    {
        private int idx = InvalidIndex;

        public Node(int index)
        {
            Index = index;
        }

        public int Index
        {
            get { return idx; }
            set { idx = value >= 0 ? value : InvalidIndex; }
        }

        public static int InvalidIndex
        {
            get { return int.MinValue; }
        }
    }
}
