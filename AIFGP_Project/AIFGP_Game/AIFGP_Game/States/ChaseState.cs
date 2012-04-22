namespace AIFGP_Game
{
    using System;
    using Microsoft.Xna.Framework;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;

    class ChaseState : State
    {
        Random rng = new Random();
        private int chanceUpdate = 5;
        public void Enter(SmartFarmer i)
        {
            return;
        }

        public void Execute(SmartFarmer i)
        {
            bool los = i.sight.canSee();
            /*if (AStarGame.GameMap.ClosestNodeIndex(i.Position) == AStarGame.GameMap.ClosestNodeIndex(i.lastSpotted))
            {
                i.Velocity = Vector2.Zero;
                return;
            }*/
            if ((los || rng.Next(1000) < chanceUpdate) &&
                            AStarGame.GameMap.ClosestNodeIndex(EntityManager.Instance.GetPlayer().Position) != AStarGame.GameMap.ClosestNodeIndex(i.lastSpotted))
            {

                i.lastSpotted = EntityManager.Instance.GetPlayer().Position;
                i.FollowingPath = false;
               
            }
            if (!i.FollowingPath)
            {
                AStarSearch rabbitSearch = new AStarSearch(AStarGame.GameMap.NavigationGraph, AStarGame.GameMap.ClosestNodeIndex(i.Position),
                            AStarGame.GameMap.ClosestNodeIndex(i.lastSpotted), AStarHeuristics.Distance);
                List<int> rabbitSearchNodes = new List<int>();
                rabbitSearch.PathToTarget(out rabbitSearchNodes);
                List<Vector2> rabbitSearchPos = new List<Vector2>();
                rabbitSearchPos = AStarGame.GameMap.getWorldfromNodes(rabbitSearchNodes);
                i.FollowPath(rabbitSearchPos, false);
                return;
            }
            /*else if(!i.FollowingPath)
            {
                i.FollowingPath = false;
                Vector2 force = i.Seek(i.lastSpotted);
                i.Velocity += force;
            }*/
        }

        public void Exit(SmartFarmer i)
        {
            return;
        }
    }
}
