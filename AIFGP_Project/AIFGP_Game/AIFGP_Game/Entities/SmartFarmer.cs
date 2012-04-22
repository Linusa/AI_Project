namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class SmartFarmer : Farmer
    {
        public enum stateType
        {
            Searching,
            Patrolling,
            Chasing
        }
        private Random rng = new Random();
        public bool doneSearching = false;
        private int stateEnum;
        public List<Vector2> patrolRoute;
        public int nextPatrol;
        public State curState;
        public Vector2 lastSpotted;
        public LoS sight;

        public SmartFarmer(Texture2D texture, Vector2 position, List<Vector2> route)
            : base(texture, position)
        {
            patrolRoute = route;
            curState = new PatrolState();
            stateEnum = (int)stateType.Patrolling;
            lastSpotted = new Vector2(500, 500);
            sight = new LoS(this);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            updateSpriteDirection();

            curState.Execute(this);
            if (sight.canSee() && stateEnum != (int)stateType.Chasing)
            {
                curState = new ChaseState();
                stateEnum = (int)stateType.Chasing;
                lastSpotted = EntityManager.Instance.GetPlayer().Position;

                if (FollowingPath)
                    FollowingPath = false;

                curState.Enter(this);
                curState.Execute(this);
            }
            else if (stateEnum == (int)stateType.Chasing && 
                        AStarGame.GameMap.ClosestNodeIndex(Position) == AStarGame.GameMap.ClosestNodeIndex(lastSpotted))
            {
                curState = new SearchState();
                stateEnum = (int)stateType.Searching;
                doneSearching = false;
                curState.Enter(this);
                curState.Execute(this);
            }
            /*else if (sight.canSee() || (stateEnum == (int)stateType.Chasing && rng.Next(1000) < 5))
            {
                lastSpotted = EntityManager.Instance.GetPlayer().Position;
            }*/
            else if (stateEnum == (int)stateType.Searching && doneSearching)
            {
                FollowingPath = false;
                curState = new PatrolState();
                stateEnum = (int)stateType.Patrolling;
                curState.Execute(this);
                
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            EntitySprite.Draw(spriteBatch);
        }
    }
}
