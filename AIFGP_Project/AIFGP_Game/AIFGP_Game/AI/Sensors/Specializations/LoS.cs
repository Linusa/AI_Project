

namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;

    public class LoS
    {
        SmartFarmer agent;
        private const int rangeSqrd = 40000;
        private const int range = 200;
        private const float angleWidth = (float)Math.PI / 4;

        public LoS(SmartFarmer i)
        {
            agent = i;
        }

        public bool canSee()
        {
            if ((EntityManager.Instance.GetPlayer() as Rabbit).IsHidden)
                return false;
            Vector2 vecToRabbit = EntityManager.Instance.GetPlayer().Position - agent.Position;
            if (vecToRabbit.LengthSquared() < rangeSqrd)
            {
                vecToRabbit.Normalize();
                float relativeAngle = (float)Angles.AngleFromUToV(agent.Heading, vecToRabbit);
                if (relativeAngle < angleWidth && relativeAngle > angleWidth * -1)
                {
                    foreach (Wall wall in WallManager.Instance.Walls)
                    {
                        float? inter = new Ray(new Vector3(agent.Position, 0),
                            new Vector3(vecToRabbit, 0)).Intersects(
                                new BoundingBox(new Vector3(wall.BoundingBox.Left,wall.BoundingBox.Top,-10),
                                                new Vector3(wall.BoundingBox.Right,wall.BoundingBox.Bottom,10)
                                                ));
                        if (inter != null && (float)inter
                            < range)
                            return false;
                    }
                    return true;
                }
            }

            return false;
        }
    }
}
