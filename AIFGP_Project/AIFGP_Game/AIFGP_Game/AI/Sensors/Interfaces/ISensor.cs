namespace AIFGP_Game
{
    using Microsoft.Xna.Framework;

    public interface ISensor : IUpdateable, IDrawable, IRotatable
    {
        Vector2 Position
        {
            get;
            set;
        }
    }
}
