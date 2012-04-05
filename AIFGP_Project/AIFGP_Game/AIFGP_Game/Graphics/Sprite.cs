namespace AIFGP_Game
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Class that abstracts away loading, displaying, and manipulating
    /// two-dimensional graphics as sprites. The standard usage of this
    /// class would be to set the position, add animation frames, set
    /// the active animation frame and call Update/Draw as needed.
    /// </summary>
    /// <typeparam name="T">Animation identifier type.</typeparam>
    public class Sprite<T> : ISpatialEntity, IUpdateable, IDrawable
    {
        public Texture2D Texture;

        private Vector2 topLeftPixel = Vector2.Zero;
        private Vector2 localOrigin = Vector2.Zero;
        private float rotation = 0.0f;
        private int spriteWidth = 0;
        private int spriteHeight = 0;
        private Vector2 spriteScale = Vector2.One;
        private float layerDepth = 0.0f;

        private Dictionary<T, List<Rectangle>> animationFrames = new Dictionary<T, List<Rectangle>>();
        private Timer animationTimer = new Timer(0.1f);
        private T curAnimationId;
        private int curAnimationFrame = 0;

        private Color tint = Color.White;

        public Sprite(Texture2D texture, Vector2 position, Rectangle dimensions)
        {
            Dimensions = dimensions;
            Texture = texture;
            CenterPosition = position;
        }

        // Copy constructor.
        public Sprite(Sprite<T> sprite)
        {
            Dimensions = sprite.Dimensions;
            Texture = sprite.Texture;
            Position = sprite.Position;
            Color = sprite.Color;

            rotation = sprite.rotation;
            spriteScale = sprite.spriteScale;
            animationFrames = new Dictionary<T,List<Rectangle>>(sprite.animationFrames);
            animationTimer = new Timer(sprite.AnimationRate);
            
            ActiveAnimation = sprite.ActiveAnimation;
        }

        public Vector2 Position
        {
            get { return topLeftPixel; }
            set { topLeftPixel = value; }
        }

        public Vector2 CenterPosition
        {
            get { return topLeftPixel + localOrigin; }
            set { topLeftPixel = value - localOrigin; }
        }

        public Vector2 LocalOrigin
        {
            get { return localOrigin; }
            set { localOrigin = value; }
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

        public void RotateInDegrees(float degrees)
        {
            RotateInRadians(MathHelper.ToRadians(degrees));
        }

        public void RotateInRadians(float radians)
        {
            while (radians < 0.0f)
            {
                radians += MathHelper.TwoPi;
            }

            rotation = (rotation + radians) % MathHelper.TwoPi;
        }

        public void Scale(Vector2 scale)
        {
            spriteScale.X = MathHelper.Max(0.0f, scale.X);
            spriteScale.Y = MathHelper.Max(0.0f, scale.Y);
        }

        public void Scale(float scale)
        {
            Scale(new Vector2(scale));
        }

        public float LayerDepth
        {
            get { return layerDepth; }
            set { layerDepth = value; }
        }

        public Color Color
        {
            get { return tint; }
            set { tint = value; }
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

        public Rectangle Dimensions
        {
            get { return new Rectangle(0, 0, spriteWidth, spriteHeight); }
            set
            {
                spriteWidth = value.Width;
                spriteHeight = value.Height;
                computeLocalOrigin();
            }
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
        }

        public void PauseAnimation()
        {
            animationTimer.Stop();
        }

        public void PlayAnimation()
        {
            animationTimer.Start();
        }

        private void computeLocalOrigin()
        {
            localOrigin.X = spriteWidth / 2;
            localOrigin.Y = spriteHeight / 2;
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
                rotation,
                localOrigin,
                spriteScale,
                SpriteEffects.None,
                LayerDepth
            );
        }
    }
}
