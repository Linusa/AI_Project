namespace AIFGP_Game
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class Rangefinder : ISensor
    {
        public IGameEntity SensingEntity;

        public List<Ray> Feelers = new List<Ray>();
        public const float AngleBetweenFeelers = 35.0f;
        public const int MaxRayDistance = 200;

        private bool enabled = false;

        private RangefinderDebugger rangefinderDebugger;

        public Rangefinder(IGameEntity entity)
        {
            SensingEntity = entity;

            // Initialize feeler directions.
            Vector3 forwardDir = new Vector3(entity.Heading, 0.0f);
            forwardDir.Normalize();

            Vector3 leftForwardDir = Vector3.Transform(forwardDir,
                Matrix.CreateRotationZ(MathHelper.ToRadians(-AngleBetweenFeelers)));
            leftForwardDir.Normalize();

            Vector3 rightForwardDir = Vector3.Transform(forwardDir,
                Matrix.CreateRotationZ(MathHelper.ToRadians(AngleBetweenFeelers)));
            rightForwardDir.Normalize();

            // Add 3 feelers.
            Vector3 position = new Vector3(entity.Position, 0.0f);
            Feelers.Add(new Ray(position, leftForwardDir));
            Feelers.Add(new Ray(position, forwardDir));
            Feelers.Add(new Ray(position, rightForwardDir));

            rangefinderDebugger = new RangefinderDebugger(this);
        }

        // Returns the distance of the intersection (if any) from
        // the feeler ray to the closest wall. The intersection
        // distance must be less than the feeler's max ray distance for
        // it to be valid. -1.0f is returned if there is no intersection
        // or the intersection happens beyond the feeler's max ray distance.
        public float DistanceToWall(Ray feeler)
        {
            float minIntersectDist = float.MaxValue;

            foreach (Wall wall in WallManager.Instance.Walls)
            {
                BoundingBox wallBox = WallManager.Instance.WallExtentsIn3D(wall);
                float curIntersectDist = feeler.Intersects(wallBox) ?? float.MaxValue;

                if (curIntersectDist < minIntersectDist)
                    minIntersectDist = curIntersectDist;
            }

            if (minIntersectDist > Rangefinder.MaxRayDistance)
                minIntersectDist = -1.0f;

            return minIntersectDist;
        }
        
        public bool IsSensingEnabled
        {
            get { return enabled; }
            set
            {
                enabled = value;
                rangefinderDebugger.IsDebuggingEnabled = value;
            }
        }

        public Vector2 Position
        {
            get { return SensingEntity.Position; }
            set
            {
                SensingEntity.Position = value;

                Vector3 position3f = new Vector3(value, 0.0f);
                for (int i = 0; i < Feelers.Count; i++)
                {
                    Ray curFeeler = Feelers[i];
                    curFeeler.Position = position3f;
                    Feelers[i] = curFeeler;
                }
            }
        }

        public void RotateInRadians(float radians)
        {
            for (int i = 0; i < Feelers.Count; i++)
            {
                Ray curFeeler = Feelers[i];
                Vector3 curFeelerDir = Vector3.Transform(curFeeler.Direction,
                    Matrix.CreateRotationZ(radians));
                curFeelerDir.Normalize();

                curFeeler.Direction = curFeelerDir;
                Feelers[i] = curFeeler;
            }

            rangefinderDebugger.RotateInRadians(radians);
        }
        
        public void RotateInDegrees(float degrees)
        {
            RotateInRadians(MathHelper.ToRadians(degrees));
        }

        public void Update(GameTime gameTime)
        {
            // Press '1' to turn on debug display, '0' to disable.
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.D1))
                IsSensingEnabled = true;
            else if (keyboardState.IsKeyDown(Keys.D0))
                IsSensingEnabled = false;

            if (IsSensingEnabled)
                rangefinderDebugger.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsSensingEnabled)
            {
                rangefinderDebugger.Draw(spriteBatch);
            }
        }
    }
}
