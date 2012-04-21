namespace AIFGP_Game
{
    using System;
    using Microsoft.Xna.Framework;

    class ChaseState : State
    {
        Random rng = new Random();
        private int chanceUpdate = 16;
        public void Enter(SmartFarmer i)
        {
            return;
        }

        public void Execute(SmartFarmer i)
        {
            if (Vector2.Subtract(i.lastSpotted, i.Position).LengthSquared() < 100)
            {
                i.Velocity = Vector2.Zero;
                i.curState.Exit(i);
                i.curState = new SearchState();
                i.curState.Enter(i);
                return;
            }
            /*if (rng.Next(1000) < chanceUpdate)
            {
                i.lastSpotted = EntityManager.Instance.GetPlayer().Position;
            }*/

            Vector2 force = i.Seek(i.lastSpotted);
            i.Velocity += force;
        }

        public void Exit(SmartFarmer i)
        {
            return;
        }
    }
}
