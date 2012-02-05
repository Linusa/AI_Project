namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class RadarDebugger : IUpdateable, IDrawable
    {
        private Radar radarDebugging;

        private bool enabled = false;
        private Vector2 debugLoc = new Vector2(15.0f);
        private StringBuilder strBuilder = new StringBuilder();
        
        private List<RadarInfo> adjacentEntities = new List<RadarInfo>();

        public RadarDebugger(Radar radar)
        {
            radarDebugging = radar;
        }

        public bool IsDebuggingEnabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public void Update(GameTime gameTime)
        {
            strBuilder.Clear();

            if (IsDebuggingEnabled)
            {
                foreach (IGameEntity entity in EntityManager.Instance.Entities.Values)
                {
                    BaseGameEntity nonAdjEntity = entity as BaseGameEntity;
                    nonAdjEntity.EntitySprite.Color = Color.White;
                }

                radarDebugging.AdjacentEntities(out adjacentEntities);
                foreach (RadarInfo radarInfo in adjacentEntities)
                {
                    BaseGameEntity entity = EntityManager.Instance.GetEntity(
                        radarInfo.EntityId) as BaseGameEntity;

                    entity.EntitySprite.Color = Color.Magenta;

                    strBuilder.AppendFormat("RADAR\nEntity: {0}...\nPos: {1}\nDir: {2}\nDist: {3}\nAngle: {4}\n================\n",
                        entity.ID.ToString().Substring(0, 8),
                        entity.Position,
                        entity.Heading,
                        radarInfo.Distance,
                        MathHelper.ToDegrees(radarInfo.RelativeAngle)
                    );
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(SensorsGame.DebugFont, strBuilder,
                debugLoc, Color.Yellow);
        }
    }
}
