namespace AIFGP_Game
{
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
        public override float BoundingRadius
        {
            get { return 24.0f; }
        }

        protected override void configureSprite()
        {
            /*
            EntitySprite.AddAnimationFrame(AnimationIds.NoAnimation, Dimensions);
            EntitySprite.ActiveAnimation = AnimationIds.NoAnimation;
            */
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
