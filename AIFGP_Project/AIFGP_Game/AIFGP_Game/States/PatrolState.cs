namespace AIFGP_Game
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;

    class PatrolState : State
    {
        public void Enter(SmartFarmer i)
        {
            if (i.patrolRoute.Count > 0)
            {
                AStarSearch patrolStart = new AStarSearch(
                    AStarGame.GameMap.NavigationGraph,
                    AStarGame.GameMap.ClosestNodeIndex(i.Position),
                    AStarGame.GameMap.ClosestNodeIndex(i.patrolRoute[0]),
                    AStarHeuristics.Distance);

                List<int> patrolSearchNodes;
                patrolStart.PathToTarget(out patrolSearchNodes);

                List<Vector2> patrolSearchPos;
                patrolSearchPos = AStarGame.GameMap.getWorldfromNodes(patrolSearchNodes);

                i.FollowPath(patrolSearchPos, false);
            }
        }

        public void Execute(SmartFarmer i)
        {
            if (i.patrolRoute.Count > 0)
                if (!i.FollowingPath)
                    i.FollowPath(i.patrolRoute, true);
        }

        public void Exit(SmartFarmer i) { }
    }
}
