namespace AIFGP_Game
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// The Radar sensor detects adjacent entities within a
    /// modifiable radius. It provides a List of RadarInfo's
    /// to uniquely identify each adjacent entity.
    /// </summary>
    public class Radar : ISensor
    {
        public IGameEntity SensingEntity;
        public float EntityRange = 125.0f;

        private bool enabled = false;

        private RadarDebugger radarDebugger;

        public Radar(IGameEntity entity)
        {
            SensingEntity = entity;
            radarDebugger = new RadarDebugger(this);
        }

        public void AdjacentEntities(out List<RadarInfo> adjacentEntities)
        {
            adjacentEntities = new List<RadarInfo>();

            foreach (IGameEntity curEntity in EntityManager.Instance.Entities.Values)
            {
                if (curEntity == SensingEntity)
                    continue;

                Vector2 vecToCurrentEntity = curEntity.Position - SensingEntity.Position;
                float distToCurrentEntity = vecToCurrentEntity.Length();
                
                if (distToCurrentEntity < EntityRange)
                {
                    vecToCurrentEntity.Normalize();
                    float relativeAngle = (float)Angles.AngleFromUToV(SensingEntity.Heading, vecToCurrentEntity);
                    adjacentEntities.Add(new RadarInfo(curEntity.ID, distToCurrentEntity, relativeAngle));
                }
            }
        }

        public bool IsSensingEnabled
        {
            get { return enabled; }
            set
            {
                enabled = value;
                radarDebugger.IsDebuggingEnabled = value;
            }
        }

        public Vector2 Position
        {
            get { return SensingEntity.Position; }
            set { SensingEntity.Position = value; }
        }

        // A radar does not need to be rotated since it is a circle.
        public void RotateInRadians(float radians) { }
        public void RotateInDegrees(float degrees) { }

        public void Update(GameTime gameTime)
        {
            // Press '2' to turn on debug display, '0' to disable.
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.D2))
                IsSensingEnabled = true;
            else if (keyboardState.IsKeyDown(Keys.D0))
                IsSensingEnabled = false;

            if (IsSensingEnabled)
                radarDebugger.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsSensingEnabled)
                radarDebugger.Draw(spriteBatch);
        }
    }
}
