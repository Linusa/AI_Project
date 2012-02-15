namespace AIFGP_Game
{
    using System;
    using Microsoft.Xna.Framework;

    public interface ICollidable
    {
        Rectangle BoundingBox
        {
            get;
        }

        Nullable<float> BoundingRadius
        {
            get;
        }
    }
}
