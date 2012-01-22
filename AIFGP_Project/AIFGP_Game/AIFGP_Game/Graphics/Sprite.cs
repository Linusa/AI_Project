using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AIFGP_Game
{
    /// <summary>
    /// Class that abstracts away loading, displaying, and manipulating
    /// two-dimensional graphics as sprites.
    /// </summary>
    public class Sprite<T> where T : new()
    {
        /// <summary>
        /// The <c>Texture2D</c> the <c>Sprite</c> is drawn from.
        /// </summary>
        private Texture2D Texture;

        private Vector2 topLeftPixel = Vector2.Zero;
        private float rotation = 0.0f;
        private UInt16 spriteWidth = 0;
        private UInt16 spriteHeight = 0;

        private Dictionary<T, List<Rectangle>> animationFrames = new Dictionary<T, List<Rectangle>>();
        private T curAnimationId = new T();
        private int curAnimationFrame = 0;
        private Timer animationTimer = new Timer(0.1f);

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
            get
            {
                Vector2 scaleToCenter = new Vector2(spriteWidth / 2, spriteHeight / 2);
                return topLeftPixel + scaleToCenter;
            }
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

        public float AnimationRate
        {
            get { return animationTimer.Timeout; }
            set { animationTimer.Timeout = MathHelper.Max(value, 0.01f); }
        }

        public T ActiveAnimation
        {
            get { return curAnimationId; }
            set { curAnimationId = value; }
        }

        public Rectangle ActiveAnimationFrame
        {
            get { return animationFrames[curAnimationId][curAnimationFrame]; }
        }

        public Rectangle BoundingBox
        {
            get
            {
                int x = (int)topLeftPixel.X, y = (int)topLeftPixel.Y;
                return new Rectangle(x, y, spriteWidth, spriteHeight);
            }
        }

        public void AddAnimationFrame(T animationId, Rectangle frame)
        {
            animationFrames[animationId].Add(frame);
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, CenterPosition, ActiveAnimationFrame,
                tint, rotation, new Vector2(spriteWidth / 2, spriteHeight / 2),
                1.0f, SpriteEffects.None, 0.0f);
        }
    }
}
