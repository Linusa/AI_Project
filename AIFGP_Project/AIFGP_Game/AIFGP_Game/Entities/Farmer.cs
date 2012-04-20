namespace AIFGP_Game
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System;

    /// <summary>
    /// SimpleSensingGameEntity is a SimpleGameEntity with sensors. :D
    /// </summary>
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
        private List<ISensor> sensors = new List<ISensor>();
        public State curState;
        public Vector2 lastSpotted;
        public LoS sight;


        public SmartFarmer(Texture2D texture, Vector2 position, List<Vector2> route)
            : base(texture, position)
        {
            // If these Adds are modified, make sure to update the PieSlice
            // one to get the correct index for the Radar instance.
            sensors.Add(new Rangefinder(this));
            sensors.Add(new Radar(this));
            sensors.Add(new PieSlice(sensors[1] as Radar));
            patrolRoute = route;
            curState = new PatrolState();
            stateEnum = (int)stateType.Patrolling;
            lastSpotted = new Vector2(500, 500);
            sight = new LoS(this);
        }

        public override void RotateInRadians(float radians)
        {
            //EntitySprite.RotateInRadians(radians);
            Heading = Vector2.Transform(Heading, Matrix.CreateRotationZ(radians));

            foreach (ISensor sensor in sensors)
                sensor.RotateInRadians(radians);
        }

        public override void RotateInDegrees(float degrees)
        {
            RotateInRadians(MathHelper.ToRadians(degrees));
        }
        
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (ISensor sensor in sensors)
            {
                sensor.Position = Position;
                sensor.Update(gameTime);
            }

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
            else if (stateEnum == (int)stateType.Chasing && (Position - lastSpotted).LengthSquared() < 100)
            {
                curState = new SearchState();
                stateEnum = (int)stateType.Searching;
                doneSearching = false;
                curState.Enter(this);
                curState.Execute(this);
            }
            else if (sight.canSee() || (stateEnum == (int)stateType.Chasing && rng.Next(1000) < 5))
            {
                lastSpotted = EntityManager.Instance.GetPlayer().Position;
            }
            else if (stateEnum == (int)stateType.Searching && doneSearching)
            {
                curState = new PatrolState();
                stateEnum = (int)stateType.Patrolling;
                curState.Execute(this);
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (ISensor sensor in sensors)
                sensor.Draw(spriteBatch);

            EntitySprite.Draw(spriteBatch);
        }

    }
}
