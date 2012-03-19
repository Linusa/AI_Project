namespace AIFGP_Game
{
    using Microsoft.Xna.Framework;

    public interface ISpatialEntity : IMovable, IRotatable, IScalable
    {
        Vector2 Position
        {
            get;
            set;
        }
    }
}
