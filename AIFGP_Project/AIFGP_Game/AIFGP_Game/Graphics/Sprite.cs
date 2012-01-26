using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AIFGP_Game
{
    /// <summary>
    /// Class that abstracts away loading, displaying, and manipulating
    /// two-dimensional graphics as sprites. The standard usage of this
    /// class would be to set the position, add animation frames, set
    /// the active animation frame and call Update/Draw as needed.
    /// </summary>
    /// <typeparam name="T">Animation identifier type.</typeparam>
    public class Sprite<T>
    {
        /// <summary>
        /// The <c>Texture2D</c> the <c>Sprite</c> is drawn from.
        /// </summary>
        public Texture2D Texture;

        private Vector2 topLeftPixel = Vector2.Zero;
        private Vector2 localOrigin = Vector2.Zero;
        private float rotation = 0.0f;
        private int spriteWidth = 0;
        private int spriteHeight = 0;
        private float spriteScale = 1.0f;

        private Dictionary<T, List<Rectangle>> animationFrames = new Dictionary<T, List<Rectangle>>();
        private Timer animationTimer = new Timer(0.1f);
        private T curAnimationId;
        private int curAnimationFrame = 0;

        private Color tint = Color.White;

        public Sprite(Texture2D texture, Vector2 position)
        {
            Texture = texture;
            Position = position;
        }

        public Vector2 Position
        {
            get { return topLeftPixel; }
            set { topLeftPixel = value; }
        }

        public Vector2 CenterPosition
        {
            get { return topLeftPixel + localOrigin; }
        }

        public float RotationInDegrees
        {
            get { return MathHelper.ToDegrees(rotation); }
            set { rotation = MathHelper.ToRadians(value) % MathHelper.TwoPi; }
        }

        public float RotationInRadians
        {
            get { return rotation; }
            set { rotation = value % MathHelper.TwoPi; }
        }

        public float Scale
        {
            get { return spriteScale; }
            set { spriteScale = MathHelper.Max(0.0f, value); }
        }

        public float AnimationRate
        {
            get { return animationTimer.Timeout; }
            set { animationTimer.Timeout = MathHelper.Max(value, 0.025f); }
        }

        public T ActiveAnimation
        {
            get { return curAnimationId; }
            set
            {
                curAnimationId = value;
                if (curAnimationFrame >= animationFrames[curAnimationId].Count)
                {
                    curAnimationFrame = 0;
                }
            }
        }

        public Rectangle ActiveAnimationFrame
        {
            // TODO: Error-checking for out-of-bounds access.
            get { return animationFrames[curAnimationId][curAnimationFrame]; }
        }

        public Rectangle BoundingBox
        {
            // TODO: This is a weak bounding box as the actual sprite
            // could be much smaller than the image rectangle.
            get
            {
                int x = (int)topLeftPixel.X, y = (int)topLeftPixel.Y;
                return new Rectangle(x, y, spriteWidth, spriteHeight);
            }
        }

        public void AddAnimationFrame(T animationId, Rectangle frame)
        {
            if (!animationFrames.ContainsKey(animationId))
            {
                animationFrames.Add(animationId, new List<Rectangle>());
            }

            animationFrames[animationId].Add(frame);

            if (spriteWidth == 0 || spriteHeight == 0)
            {
                spriteWidth = frame.Width;
                spriteHeight = frame.Height;
                
                localOrigin.X = spriteWidth / 2;
                localOrigin.Y = spriteHeight / 2;
            }
        }

        public void PauseAnimation()
        {
            animationTimer.Stop();
        }

        public void PlayAnimation()
        {
            animationTimer.Start();
        }

        public void Update(GameTime gameTime)
        {
            if (animationTimer.Expired(gameTime))
            {
                int numFrames = animationFrames[curAnimationId].Count;
                curAnimationFrame = (curAnimationFrame + 1) % numFrames;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                Texture,
                CenterPosition,
                ActiveAnimationFrame,
                tint,
                RotationInRadians,
                localOrigin,
                Scale,
                SpriteEffects.None,
                0.0f
            );
        }
    }
}
