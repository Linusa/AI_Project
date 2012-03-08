namespace AIFGP_Game
{
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
