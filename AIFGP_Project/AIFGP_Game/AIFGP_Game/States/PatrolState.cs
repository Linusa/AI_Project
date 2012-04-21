﻿namespace AIFGP_Game
{
    class PatrolState : State
    {
        public void Enter(SmartFarmer i)
        {
            return;
        }

        public void Execute(SmartFarmer i)
        {
            if (i.patrolRoute.Count > 0)
                if (!i.FollowingPath)
                    i.FollowPath(i.patrolRoute, true);
            if (i.sight.canSee())
            {
                i.curState.Exit(i);
                i.curState = new ChaseState();
                i.curState.Enter(i);
            }
            
            return;
        }

        public void Exit(SmartFarmer i)
        {
            return;
        }
    }
}
