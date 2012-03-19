namespace AIFGP_Game
{
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Debugs Radar by showing graphical information about it.
    /// </summary>
    public class RadarDebugger : IUpdateable, IDrawable
    {
        private Radar radarDebugging;
        
        public Sprite<byte> RadarSprite;
        public static Rectangle SpriteDimensions = new Rectangle(0, 0, 250, 250);
        private Vector2 scaleUp = new Vector2(0.025f);
        private Vector2 scaleMax = new Vector2(1.0f);
        private Timer scaleUpTimer = new Timer(0.01f);

        private bool enabled = false;
        private Vector2 debugLoc = new Vector2(15.0f);
        private StringBuilder strBuilder = new StringBuilder();
        
        private List<RadarInfo> adjacentEntities = new List<RadarInfo>();

        public RadarDebugger(Radar radar)
        {
            radarDebugging = radar;

            RadarSprite = new Sprite<byte>(AStarGame.RadarCircle, Vector2.Zero, SpriteDimensions);
            RadarSprite.AddAnimationFrame(0, SpriteDimensions);
            RadarSprite.ActiveAnimation = 0;
            
            resetScaleUp();
        }

        public bool IsDebuggingEnabled
        {
            get { return enabled; }
            set
            {
                if (enabled)
                    resetEntityColors();
                else
                    resetScaleUp();

                enabled = value;
            }
        }

        public void Update(GameTime gameTime)
        {
            updateRadarScaleUp(gameTime);

            RadarSprite.CenterPosition = radarDebugging.Position;
            
            strBuilder.Clear();
            resetEntityColors();

            strBuilder.Append("RADAR\n================\n");

            radarDebugging.AdjacentEntities(out adjacentEntities);
            foreach (RadarInfo radarInfo in adjacentEntities)
            {
                BaseGameEntity entity = EntityManager.Instance.GetEntity(
                    radarInfo.EntityId) as BaseGameEntity;

                entity.EntitySprite.Color = Color.Magenta;

                strBuilder.AppendFormat("Entity: {0}...\nPos: {1}\nDir: {2}\nDist: {3}\nAngle: {4}\n----------------\n",
                    entity.ID.ToString().Substring(0, 8),
                    entity.Position,
                    entity.Heading,
                    radarInfo.Distance,
                    MathHelper.ToDegrees(radarInfo.RelativeAngle)
                );
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            RadarSprite.Draw(spriteBatch);
            spriteBatch.DrawString(AStarGame.DebugFont, strBuilder,
                debugLoc, Color.Yellow);
        }

        private void updateRadarScaleUp(GameTime gameTime)
        {
            if (scaleUp.X >= scaleMax.X && scaleUp.Y >= scaleMax.Y)
                scaleUp = scaleMax;
            else
            {
                if (scaleUpTimer.Expired(gameTime))
                    scaleUp += new Vector2(0.04f);
            }
            
            RadarSprite.Scale(scaleUp);
        }

        private void resetScaleUp()
        {
            scaleUp = Vector2.Zero;
            RadarSprite.Scale(scaleUp);
            scaleUpTimer.Restart();
        }

        private void resetEntityColors()
        {
            foreach (IGameEntity entity in EntityManager.Instance.Entities.Values)
            {
                BaseGameEntity nonAdjEntity = entity as BaseGameEntity;
                nonAdjEntity.EntitySprite.Color = Color.White;
            }
        }
    }
}
