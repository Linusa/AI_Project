namespace AIFGP_Game
{
    using Microsoft.Xna.Framework;

    public class PositionalNode : Node
    {
        public Vector2 Position;

        public PositionalNode(int index, Vector2 position)
            : base(index)
        {
            Position = position;
        }
    }
}
