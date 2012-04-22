namespace AIFGP_Game
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;

    class PatrolState : State
    {
        public void Enter(SmartFarmer i)
        {
            AStarSearch patrolStart = new AStarSearch(AStarGame.GameMap.NavigationGraph, AStarGame.GameMap.ClosestNodeIndex(i.Position),
                            AStarGame.GameMap.ClosestNodeIndex(i.patrolRoute[0]), AStarHeuristics.Distance);
            List<int> patrolSearchNodes = new List<int>();
            patrolStart.PathToTarget(out patrolSearchNodes);
            List<Vector2> patrolSearchPos = new List<Vector2>();
            patrolSearchPos = AStarGame.GameMap.getWorldfromNodes(patrolSearchNodes);
            i.FollowPath(patrolSearchPos, false);
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
