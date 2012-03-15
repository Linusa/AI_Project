namespace AIFGP_Game
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public abstract class BaseGameEntity : IGameEntity
    {
        // Should never be adding more than 256 animations to a Sprite.
        public Sprite<byte> EntitySprite;

        // GUID for the entity.
        private Guid id;

        private Vector2 heading = Vector2.Zero;
        private Vector2 velocity = Vector2.Zero;

        private float maxSpeed = 250.0f;

        public BaseGameEntity(Texture2D texture, Vector2 position, Rectangle dimensions)
        {
            EntitySprite = new Sprite<byte>(texture, position, dimensions);
            Position = position;

            configureSprite();

            // Store a new GUID for the entity and make sure it is not empty.
            id = Guid.NewGuid();
            while (id == Guid.Empty)
                id = Guid.NewGuid();

            // Make sure to register this entity with the EntityManager.
            EntityManager.Instance.RegisterEntity(this);
        }

        public Guid ID
        {
            get { return id; }
        }

        public Vector2 Position
        {
            get { return EntitySprite.CenterPosition; }
            set { EntitySprite.CenterPosition = value; }
        }

        public Vector2 Heading
        {
            get { return heading; }
            set { heading = Vector2.Normalize(value); }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public float MaxSpeed
        {
            get { return maxSpeed; }
            set { maxSpeed = MathHelper.Max(0.0f, value); }
        }

        public virtual void Translate(Vector2 offset)
        {
            Position += offset;
        }

        public virtual void Translate(int x, int y)
        {
            Vector2 offset = new Vector2(x, y);
            Position += offset;
        }

        public virtual void RotateInRadians(float radians)
        {
            EntitySprite.RotateInRadians(radians);
            Heading = Vector2.Transform(heading, Matrix.CreateRotationZ(radians));
        }

        public virtual void RotateInDegrees(float degrees)
        {
            RotateInRadians(MathHelper.ToRadians(degrees));
        }

        public virtual void Scale(float scale)
        {
            EntitySprite.Scale(scale);
        }

        public virtual void Scale(Vector2 scale)
        {
            EntitySprite.Scale(scale);
        }

        public Vector2 Seek(Vector2 target)
        {
            Vector2 toTarget = Vector2.Normalize(target - Position) * MaxSpeed;
            return toTarget - Velocity;
        }

        public virtual void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position += Velocity * dt;

            EntitySprite.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            EntitySprite.Draw(spriteBatch);
        }

        public abstract Rectangle BoundingBox
        {
            get;
        }

        public abstract Nullable<float> BoundingRadius
        {
            get;
        }

        protected abstract void configureSprite();
    }
}
