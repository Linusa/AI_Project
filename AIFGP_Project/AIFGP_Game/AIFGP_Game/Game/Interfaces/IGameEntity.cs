namespace AIFGP_Game
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IGameEntity : ISpatialEntity, IUpdateable, IDrawable
    {
        Vector2 Heading
        {
            get;
            set;
        }
    }
}
