

namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;

    class LoS
    {
        SimpleSensingGameEntity agent;
        private const int rangeSqrd = 40000;
        private const float angleWidth = (float)Math.PI/6;

        public LoS(SimpleSensingGameEntity i)
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
                    return true;
            }

            return false;
        }
    }
}
