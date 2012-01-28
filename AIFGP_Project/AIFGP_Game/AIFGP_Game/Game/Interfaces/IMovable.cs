namespace AIFGP_Game
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IMovable
    {
        Vector2 Position
        {
            get;
            set;
        }

        void Translate(Vector2 offset);
        void Translate(int x, int y);
    }
}
