namespace AIFGP_Game
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SimpleGameEntity : BaseGameEntity
    {
        public struct AnimationIds
        {
            public const byte Effects = 0;
        }

        // NOTE: Hard-coded based on 50x50 arrow pics.
        public static Rectangle Dimensions = new Rectangle(0, 0, 50, 50);

        public SimpleGameEntity(Texture2D texture, Vector2 position)
            : base(texture, position, Dimensions)
        {
            Heading = new Vector2(1.0f, 0.0f);
        }

        public override Rectangle BoundingBox
        {
            get {return EntitySprite.BoundingBox; }
        }

        // NOTE: Hard-coded based on 50x50 arrow pics.
        public override Nullable<float> BoundingRadius
        {
            get { return 24.0f; }
        }

        public override void Update(GameTime gameTime)
        {
            EntitySprite.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            EntitySprite.Draw(spriteBatch);
        }

        protected override void configureSprite()
        {
            Rectangle curFrame = Dimensions;
            for (int i = 0; i < 9; i++)
            {
                EntitySprite.AddAnimationFrame(AnimationIds.Effects, curFrame);
                curFrame.X += curFrame.Width;
            }

            EntitySprite.ActiveAnimation = AnimationIds.Effects;
            EntitySprite.PlayAnimation();
        }
    }
}
