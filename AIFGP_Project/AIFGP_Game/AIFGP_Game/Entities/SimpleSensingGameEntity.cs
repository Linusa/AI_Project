namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SimpleSensingGameEntity : SimpleGameEntity
    {
        private List<ISensor> sensors = new List<ISensor>();

        public SimpleSensingGameEntity(Texture2D texture, Vector2 position)
            : base(texture, position)
        {
            sensors.Add(new Radar(this));
        }

        public override void Update(GameTime gameTime)
        {
            foreach (ISensor sensor in sensors)
            {
                sensor.Update(gameTime);
            }

            EntitySprite.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (ISensor sensor in sensors)
            {
                sensor.Draw(spriteBatch);
            }

            EntitySprite.Draw(spriteBatch);
        }
    }
}
