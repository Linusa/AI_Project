﻿namespace AIFGP_Game
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;

    class SearchState : State
    {
        List<Vector2> bushes = new List<Vector2>();
        int nextBush = 0;
        bool done;

        public void Enter(SmartFarmer i)
        {
            bushes = BushRadar.getBushes();
            if (bushes.Count == 0)
                done = true;
            return;
        }

        public void Execute(SmartFarmer i)
        {
            if (done)
            {
                i.Velocity = Vector2.Zero;
                i.curState.Exit(i);
                i.curState = new PatrolState();
                i.curState.Enter(i);
                return;
            }
            if (i.sight.canSee())
            {
                i.Velocity = Vector2.Zero;
                i.curState.Exit(i);
                i.curState = new ChaseState();
                i.curState.Enter(i);
            }
            if (Vector2.Subtract(bushes[nextBush], i.Position).LengthSquared() < 100)
            {
                if (nextBush == bushes.Count - 1)
                {
                    done = true;
                   
                    i.doneSearching = true;
                }
                nextBush = (nextBush + 1) % bushes.Count;
            }
            Vector2 force = i.Seek(bushes[nextBush]);
            i.Velocity += force;
            return;
        }

        public void Exit(SmartFarmer i)
        {
            return;
        }
    }
}
