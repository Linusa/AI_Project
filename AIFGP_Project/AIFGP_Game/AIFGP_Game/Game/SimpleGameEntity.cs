namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SimpleGameEntity : BaseGameEntity
    {
        public struct AnimationIds
        {
            public const byte NoAnimation = 0;
        }

        public static Rectangle Dimensions = new Rectangle(0, 0, 50, 50);

        public SimpleGameEntity(Texture2D texture, Vector2 position)
            : base(texture, position, Dimensions)
        {
            Heading = new Vector2(1, 0);
        }

        protected override void configureSprite()
        {
            EntitySprite.AddAnimationFrame(AnimationIds.NoAnimation, Dimensions);
            EntitySprite.ActiveAnimation = AnimationIds.NoAnimation;
        }
    }
}
