namespace AIFGP_Game
{
    using Microsoft.Xna.Framework;

    public interface ICollidable
    {
        Rectangle BoundingBox
        {
            get;
        }

        float BoundingRadius
        {
            get;
        }
    }
}
