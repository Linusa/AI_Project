namespace AIFGP_Game
{
    using Microsoft.Xna.Framework;

    class ChaseState : State
    {
        public void Enter(SmartFarmer i) { }

        public void Execute(SmartFarmer i)
        {
            i.lastSpotted = EntityManager.Instance.GetPlayer().Position;
            i.FollowingPath = false;

            Vector2 force = i.Seek(i.lastSpotted);

            if (!AStarGame.ApproximatelyZero(force))
                i.Velocity += force;
            else
                i.Velocity = Vector2.Zero;
        }

        public void Exit(SmartFarmer i) { }
    }
}
