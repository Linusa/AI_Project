namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class Radar : ISensor
    {
        public IGameEntity SensingEntity;
        public Sprite<byte> Sprite;

        public float EntityRange = 125.0f;

        private static Rectangle dimensions = new Rectangle(0, 0, 250, 250);

        private Vector2 scaleUp = new Vector2(0.025f);
        private Vector2 scaleMax = new Vector2(1.0f);
        private Timer scaleUpTimer = new Timer(0.01f);

        private bool enabled = false;

        RadarDebugger radarDebugger;

        public Radar(IGameEntity entity)
        {
            SensingEntity = entity;

            Sprite = new Sprite<byte>(SensorsGame.RadarCircle, Vector2.Zero, dimensions);
            Sprite.AddAnimationFrame(0, dimensions);
            Sprite.ActiveAnimation = 0;

            resetScaleUp();

            radarDebugger = new RadarDebugger(this);
            radarDebugger.IsDebuggingEnabled = true;
        }

        public void AdjacentEntities(out List<RadarInfo> adjacentEntities)
        {
            adjacentEntities = new List<RadarInfo>();

            foreach (IGameEntity curEntity in EntityManager.Instance.Entities.Values)
            {
                // Do not check if we are comparing the same entity.
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
            set { enabled = value; }
        }

        public Vector2 Position
        {
            get { return Sprite.CenterPosition; }
            set { Sprite.CenterPosition = value; }
        }

        public void Translate(Vector2 offset)
        {
            Position += offset;
        }

        public void Translate(int x, int y)
        {
            Vector2 offset = new Vector2(x, y);
            Position += offset;
        }

        public void RotateInRadians(float radians)
        {
            Sprite.RotateInRadians(radians);
        }

        public void RotateInDegrees(float degrees)
        {
            RotateInRadians(MathHelper.ToRadians(degrees));
        }

        public void Scale(Vector2 scale)
        {
            Sprite.Scale(scale);
        }

        public void Scale(float scale)
        {
            Sprite.Scale(scale);
        }

        public void Update(GameTime gameTime)
        {
            // Press '2' to turn on, '0' to disable.
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.D2))
            {
                IsSensingEnabled = true;
            }
            else if (keyboardState.IsKeyDown(Keys.D0))
            {
                IsSensingEnabled = false;
                resetScaleUp();
            }

            if (IsSensingEnabled)
            {
                updateRadarScaleUp(gameTime);

                if (radarDebugger.IsDebuggingEnabled)
                    radarDebugger.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsSensingEnabled)
            {
                Sprite.Draw(spriteBatch);

                if (radarDebugger.IsDebuggingEnabled)
                    radarDebugger.Draw(spriteBatch);
            }
        }

        private void updateRadarScaleUp(GameTime gameTime)
        {
            if (scaleUp.X >= scaleMax.X && scaleUp.Y >= scaleMax.Y)
            {
                scaleUp = scaleMax;
            }
            else
            {
                if (scaleUpTimer.Expired(gameTime))
                {
                    scaleUp += new Vector2(0.04f);
                }
            }
            
            Sprite.Scale(scaleUp);
        }

        private void resetScaleUp()
        {
            scaleUp = Vector2.Zero;
            Sprite.Scale(scaleUp);
            scaleUpTimer.Restart();
        }
    }
}
