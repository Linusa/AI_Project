namespace AIFGP_Game
{
    using Microsoft.Xna.Framework;

    public interface IMovable
    {
        void Translate(Vector2 offset);
        void Translate(int x, int y);
    }
}
