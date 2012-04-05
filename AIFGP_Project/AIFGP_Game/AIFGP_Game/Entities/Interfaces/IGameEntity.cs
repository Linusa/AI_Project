namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;

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

        bool FollowingPath
        {
            get;
            set;
        }

        void FollowPath(List<Vector2> path, bool patrol);

        Vector2 Seek(Vector2 target);
    }
}
