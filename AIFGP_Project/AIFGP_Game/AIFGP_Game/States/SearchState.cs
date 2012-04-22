namespace AIFGP_Game
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;

    class SearchState : State
    {
        List<Vector2> bushes = new List<Vector2>();
        int nextBush = 0;
        AStarSearch bushSearch;
        List<int> bushSearchNodes;
        List<Vector2> bushSearchPos;

        public void Enter(SmartFarmer i)
        {
            bushes = BushRadar.getBushes();

            if (bushes.Count == 0)
                i.doneSearching = true;
            else
            {
                bushSearch = new AStarSearch(
                    AStarGame.GameMap.NavigationGraph,
                    AStarGame.GameMap.ClosestNodeIndex(i.Position + (15.0f * i.Heading)),
                    AStarGame.GameMap.ClosestNodeIndex(bushes[0]),
                    AStarHeuristics.Distance);

                bushSearch.PathToTarget(out bushSearchNodes);
                bushSearchPos = AStarGame.GameMap.getWorldfromNodes(bushSearchNodes);

                i.FollowPath(bushSearchPos, false);
            }
        }

        public void Execute(SmartFarmer i)
        {
            if (!i.doneSearching)
            {
                bool reachedBush = AStarGame.ApproximatelyEqual(i.Position, bushes[nextBush]);
                if (reachedBush)
                {
                    nextBush++;
                    i.FollowingPath = false;
    
                    if (nextBush >= bushes.Count)
                    {
                        i.doneSearching = true;
                        return;
                    }
                }

                if (!i.FollowingPath)
                {
                    if (!AStarGame.GameMap.WallsBetween(i.Position, bushes[nextBush]))
                    {
                        Vector2 force = i.Seek(bushes[nextBush]);
                        i.Velocity += force;
                    }
                    else
                    {
                        bushSearch = new AStarSearch(
                            AStarGame.GameMap.NavigationGraph,
                            AStarGame.GameMap.ClosestNodeIndex(i.Position),
                            AStarGame.GameMap.ClosestNodeIndex(bushes[nextBush]),
                            AStarHeuristics.Distance);

                        bushSearch.PathToTarget(out bushSearchNodes);
                        bushSearchPos = AStarGame.GameMap.getWorldfromNodes(bushSearchNodes);

                        i.FollowPath(bushSearchPos, false);
                    }
                }
            }
        }

        public void Exit(SmartFarmer i) { }
    }
}
