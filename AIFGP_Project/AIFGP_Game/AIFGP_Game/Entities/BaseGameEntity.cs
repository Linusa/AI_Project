﻿namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// BaseGameEntity is an abstract class that provides most of the
    /// implementation to get an entity up and running.
    /// </summary>
    public abstract class BaseGameEntity : IGameEntity
    {
        // Should never be adding more than 256 animations to a Sprite.
        public Sprite<byte> EntitySprite;

        private Guid id;

        private Vector2 heading = Vector2.Zero;
        private Vector2 velocity = Vector2.Zero;
        private float maxSpeed = 100.0f;

        private List<Vector2> path = new List<Vector2>();
        private int pathIdx = 0;
        private bool followingPath = false;
        private bool patrollingPath = false;

        private Vector2 oldVelocity = Vector2.Zero;

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

        public bool FollowingPath
        {
            get { return followingPath; }
            set
            {
                followingPath = value;

                if (!followingPath)
                    Velocity = Vector2.Zero;
            }
        }

        public bool PatrollingPath
        {
            get { return patrollingPath; }
        }

        public int NextPathIndex
        {
            get { return pathIdx; }
            set { pathIdx = value % path.Count; }
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
            //EntitySprite.RotateInRadians(radians);
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

        public void FollowPath(List<Vector2> p, bool patrol)
        {
            path = p;
            FollowingPath = true;
            patrollingPath = patrol;
            NextPathIndex = 0;
        }

        // Force needed to move towards a target.
        public Vector2 Seek(Vector2 target)
        {
            Vector2 toTarget = Vector2.Normalize(target - Position);

            if (Single.IsNaN(toTarget.X) || Single.IsNaN(toTarget.Y))
                return Vector2.Zero;

            toTarget *= MaxSpeed;
            return toTarget - Velocity;
        }

        public virtual void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (FollowingPath)
            {
                Velocity += Seek(path[NextPathIndex]);

                if (nextNodeReached())
                {
                    NextPathIndex++;

                    if (patrollingPath && NextPathIndex == 0)
                        path.Reverse();

                    if (!patrollingPath && NextPathIndex == 0)
                        FollowingPath = false;
                }
            }
            
            CollisionManager.Instance.CheckWalls(this);
            
            RotateInRadians((float)Angles.AngleFromUToV(Heading, Velocity));

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

        // Checks if the entity is within 10px of current path position.
        private bool nextNodeReached()
        {
            float epsilon = 10.0f;

            bool xEqual = Math.Abs(path[NextPathIndex].X - Position.X) < epsilon;
            bool yEqual = Math.Abs(path[NextPathIndex].Y - Position.Y) < epsilon;

            return xEqual && yEqual;
        }
    }
}
