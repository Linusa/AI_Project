

namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;

    class ChaseState : State
    {
        public void Enter(SimpleSensingGameEntity i)
        {
            return;
        }

        public void Execute(SimpleSensingGameEntity i)
        {
            if (Vector2.Subtract(i.lastSpotted, i.Position).LengthSquared() < 100)
            {
                i.Velocity = Vector2.Zero;
                return;
            }

            Vector2 force = i.Seek(i.lastSpotted);
            i.Velocity += force;
        }

        public void Exit(SimpleSensingGameEntity i)
        {
            return;
        }
    }
}
