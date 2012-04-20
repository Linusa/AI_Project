namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;

    // CollisionManager is a singleton. Access it with Instance property.
    public class CollisionManager
    {
        public bool CheckWalls(IGameEntity entity)
        {
            bool collision = false;
            Vector2 wallSideToEntity = Vector2.Zero;

            foreach (Wall wall in WallManager.Instance.Walls)
            {
                if (wall.BoundingBox.Intersects(entity.BoundingBox))
                {
                    collision = true;

                    Vector2 newVel = entity.Velocity;

                    Point center = wall.BoundingBox.Center;

                    wallSideToEntity = entity.Position - wall.LeftCenter;
                    wallSideToEntity.Normalize();
                    float leftDot = Vector2.Dot(wallSideToEntity, wall.LeftNormal);

                    wallSideToEntity = entity.Position - wall.RightCenter;
                    wallSideToEntity.Normalize();
                    float rightDot = Vector2.Dot(wallSideToEntity, wall.RightNormal);
                    
                    wallSideToEntity = entity.Position - wall.TopCenter;
                    wallSideToEntity.Normalize();
                    float topDot = Vector2.Dot(wallSideToEntity, wall.TopNormal);
                    
                    wallSideToEntity = entity.Position - wall.BottomCenter;
                    wallSideToEntity.Normalize();
                    float bottomDot = Vector2.Dot(wallSideToEntity, wall.BottomNormal);

                    if (leftDot > 0.0f && newVel.X > 0.0f)
                        newVel.X = 0.0f;
                    else if (rightDot > 0.0f && newVel.X < 0.0f)
                        newVel.X = 0.0f;
                    else if (topDot > 0.0f && newVel.Y > 0.0f)
                        newVel.Y = 0.0f;
                    else if (bottomDot > 0.0f && newVel.Y < 0.0f)
                        newVel.Y = 0.0f;

                    entity.Velocity = newVel;

                    //break;
                }
            }

            return collision;
        }

        public static CollisionManager Instance {
            get
            {
                if (instance == null)
                    instance = new CollisionManager();
    
                return instance;
            }
        }

        // Single instance.
        private static CollisionManager instance;
    
        // Private constructor.
        private CollisionManager() { }
    }
}
