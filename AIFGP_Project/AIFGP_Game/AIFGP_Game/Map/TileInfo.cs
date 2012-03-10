namespace AIFGP_Game
{
    using Microsoft.Xna.Framework;

    public struct TileInfo
    {
        public Map.TileType Type;
        public Vector2 Position;

        public TileInfo(Map.TileType type, Vector2 position)
        {
            Type = type;
            Position = position;
        }
    }
}
