namespace AIFGP_Game
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISpatialEntity : IMovable, IRotatable, IScalable
    {
        Vector2 Position
        {
            get;
            set;
        }
    }
}
