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

        public SimpleGameEntity(Texture2D texture, Vector2 position)
            : base(texture, position)
        {
            // do nothing
        }

        protected override void configureSprite()
        {
            const int spriteSize = 50;
            Rectangle frame = new Rectangle(0, 0, spriteSize, spriteSize);

            EntitySprite.AddAnimationFrame(AnimationIds.NoAnimation, frame);
            EntitySprite.ActiveAnimation = AnimationIds.NoAnimation;
            //EntitySprite.AnimationRate = 0.1f;
            //EntitySprite.Scale(1.25f);
        }
    }
}
