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
            sensors.Add(new PieSlice((Radar)sensors[0]));
        }

        public override void RotateInRadians(float radians)
        {
            EntitySprite.RotateInRadians(radians);
            Heading = Vector2.Transform(Heading, Matrix.CreateRotationZ(radians));

            foreach (ISensor sensor in sensors)
            {
                // Sprite used for radar is not perfect circle so it
                // looks funky when rotated.
                if (sensor is Radar)
                {
                    continue;
                }

                sensor.RotateInRadians(radians);
            }
        }

        public override void RotateInDegrees(float degrees)
        {
            RotateInRadians(MathHelper.ToRadians(degrees));
        }

        public override void Update(GameTime gameTime)
        {
            foreach (ISensor sensor in sensors)
            {
                sensor.Position = Position;
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
