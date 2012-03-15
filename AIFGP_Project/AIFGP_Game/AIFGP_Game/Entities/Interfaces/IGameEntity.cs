namespace AIFGP_Game
{
    using System;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IGameEntity : ISpatialEntity, ICollidable, IUpdateable, IDrawable
    {
        Guid ID
        {
            get;
        }

        Vector2 Heading
        {
            get;
            set;
        }

        Vector2 Velocity
        {
            get;
            set;
        }

        float MaxSpeed
        {
            get;
            set;
        }

        Vector2 Seek(Vector2 target);
    }
}
