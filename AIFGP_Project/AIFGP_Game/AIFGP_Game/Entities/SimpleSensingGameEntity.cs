namespace AIFGP_Game
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// SimpleSensingGameEntity is a SimpleGameEntity with sensors. :D
    /// </summary>
    public class SimpleSensingGameEntity : SimpleGameEntity
    {
        private List<ISensor> sensors = new List<ISensor>();

        public SimpleSensingGameEntity(Texture2D texture, Vector2 position)
            : base(texture, position)
        {
            // If these Add's are modified, make sure to update the PieSlice
            // one to get the correct index for the Radar instance.
            sensors.Add(new Rangefinder(this));
            sensors.Add(new Radar(this));
            sensors.Add(new PieSlice(sensors[1] as Radar));
        }

        public override void RotateInRadians(float radians)
        {
            EntitySprite.RotateInRadians(radians);
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
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (ISensor sensor in sensors)
                sensor.Draw(spriteBatch);

            EntitySprite.Draw(spriteBatch);
        }
    }
}
