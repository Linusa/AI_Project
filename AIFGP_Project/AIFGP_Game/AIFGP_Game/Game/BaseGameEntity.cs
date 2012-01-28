namespace AIFGP_Game
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public abstract class BaseGameEntity : IGraphicalEntity
    {
        // Should never be adding more than 256 animations to a Sprite.
        protected Sprite<byte> EntitySprite;

        private Vector2 position = Vector2.Zero;
        private Vector2 heading = Vector2.Zero;

        public BaseGameEntity(Texture2D texture, Vector2 position)
        {
            EntitySprite = new Sprite<byte>(texture, position);
            Position = position;

            configureSprite();
        }

        public Vector2 Position
        {
            get { return position; }
            set
            {
                EntitySprite.CenterPosition = value;
                position = value;
            }
        }

        public Vector2 Heading
        {
            get { return heading; }
            set { heading = Vector2.Normalize(value); }
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

        public float RotationInRadians
        {
            get { return EntitySprite.RotationInRadians; }
            set
            {
                EntitySprite.RotationInRadians += value;

                Quaternion rotQuat = Quaternion.CreateFromAxisAngle(Vector3.Up, value);
                Heading = Vector2.Transform(heading, rotQuat);
            }
        }

        public float RotationInDegrees
        {
            get { return EntitySprite.RotationInDegrees; }
            set { RotationInRadians = MathHelper.ToRadians(value); }
        }

        public void Scale(float scale)
        {
            EntitySprite.Scale(scale);
        }

        public void Scale(Vector2 scale)
        {
            EntitySprite.Scale(scale);
        }

        public virtual void Update(GameTime gameTime)
        {
            EntitySprite.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            EntitySprite.Draw(spriteBatch);
        }

        protected abstract void configureSprite();
    }
}
