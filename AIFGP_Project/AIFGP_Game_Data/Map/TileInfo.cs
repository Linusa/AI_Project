namespace AIFGP_Game_Data
{
    using Microsoft.Xna.Framework;

    public struct TileInfo
    {
        public TileType Type;
        public Vector2 Position;

        public TileInfo(TileType type, Vector2 position)
        {
            Type = type;
            Position = position;
        }
    }
}
