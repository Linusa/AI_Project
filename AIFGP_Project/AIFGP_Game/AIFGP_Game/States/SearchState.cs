namespace AIFGP_Game
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;

    class SearchState : State
    {
        List<Vector2> bushes = new List<Vector2>();
        int nextBush = 0;
        bool done;
        AStarSearch bushSearch;
        List<int> bushSearchNodes;
        List<Vector2> bushSearchPos;

        public void Enter(SmartFarmer i)
        {
            bushes = BushRadar.getBushes();
            if (bushes.Count == 0)
                done = true;
            bushSearch = new AStarSearch(AStarGame.GameMap.NavigationGraph, AStarGame.GameMap.ClosestNodeIndex(i.Position),
                            AStarGame.GameMap.ClosestNodeIndex(bushes[0]), AStarHeuristics.Distance);
            bushSearchNodes = new List<int>();
            bushSearch.PathToTarget(out bushSearchNodes);
            bushSearchPos = new List<Vector2>();
            bushSearchPos = AStarGame.GameMap.getWorldfromNodes(bushSearchNodes);
            i.FollowPath(bushSearchPos, false);
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
            if (AStarGame.GameMap.ClosestNodeIndex(i.Position) == AStarGame.GameMap.ClosestNodeIndex(bushes[nextBush]))
            {
                if (nextBush == bushes.Count - 1)
                {
                    done = true;
                    i.FollowingPath = false;
                    i.doneSearching = true;
                }
                else
                {

                    nextBush++;

                    bushSearch = new AStarSearch(AStarGame.GameMap.NavigationGraph, AStarGame.GameMap.ClosestNodeIndex(i.Position),
                                AStarGame.GameMap.ClosestNodeIndex(bushes[nextBush]), AStarHeuristics.Distance);
                    bushSearchNodes.Clear();
                    bushSearch.PathToTarget(out bushSearchNodes);
                    bushSearchPos.Clear();
                    bushSearchPos = AStarGame.GameMap.getWorldfromNodes(bushSearchNodes);
                    i.FollowPath(bushSearchPos, false);
                }
            }
           /* Vector2 force = i.Seek(bushes[nextBush]);
            i.Velocity += force;*/
            return;
        }

        public void Exit(SmartFarmer i)
        {
            return;
        }
    }
}
