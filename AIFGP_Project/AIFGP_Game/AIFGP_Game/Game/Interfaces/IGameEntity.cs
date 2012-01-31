namespace AIFGP_Game
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IGameEntity : ISpatialEntity, ICollidable, IUpdateable, IDrawable
    {
        Vector2 Heading
        {
            get;
            set;
        }
    }
}
