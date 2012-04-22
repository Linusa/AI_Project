namespace AIFGP_Game
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Farmer : BaseGameEntity
    {
        public enum AnimationIds
        {
            MoveForward,
            MoveLeft,
            MoveBack,
            MoveRight,
        }
        
        private const float aheadQuadrantStart = -45.0f;
        private const float rightQuadrantStart = 45.0f;
        private const float behindQuadrantStart = 135.0f;
        private const float leftQuadrantStart = -135.0f;

        // NOTE: Hard-coded.
        private static Rectangle dimensions = new Rectangle(0, 0, 36, 36);

        public Farmer(Texture2D texture, Vector2 position)
            : base(texture, position, dimensions)
        {
            Heading = new Vector2(1.0f, 0.0f);
        }

        public override Rectangle BoundingBox
        {
            get { return EntitySprite.BoundingBox; }
        }

        public override Nullable<float> BoundingRadius
        {
            get { return null; }
        }

        protected override void configureSprite()
        {
            EntitySprite.LayerDepth = AStarGame.DrawingOrder.Farmers;

            int numCols = 4;

            Rectangle curFrame = dimensions;

            for (int i = 0; i < numCols; i++)
            {
                EntitySprite.AddAnimationFrame((byte)AnimationIds.MoveBack, curFrame);
                curFrame.X += curFrame.Width;
            }
            
            curFrame.X = 0;
            curFrame.Y += curFrame.Height;
            
            for (int i = 0; i < numCols; i++)
            {
                EntitySprite.AddAnimationFrame((byte)AnimationIds.MoveForward, curFrame);
                curFrame.X += curFrame.Width;
            }
            
            curFrame.X = 0;
            curFrame.Y += curFrame.Height;
            
            for (int i = 0; i < numCols; i++)
            {
                EntitySprite.AddAnimationFrame((byte)AnimationIds.MoveLeft, curFrame);
                EntitySprite.AddAnimationFrame((byte)AnimationIds.MoveRight, curFrame);
                curFrame.X += curFrame.Width;
            }

            EntitySprite.ActiveAnimation = (byte)AnimationIds.MoveForward;
            EntitySprite.PlayAnimation();
        }
        
        protected void updateSpriteDirection()
        {
            float angle = MathHelper.ToDegrees((float)Angles.AngleFromUToV(Heading, Vector2.UnitY));

            if (angle > aheadQuadrantStart && angle <= rightQuadrantStart)
            {
                EntitySprite.ActiveAnimation = (byte)AnimationIds.MoveBack;
                EntitySprite.SpriteEffects = SpriteEffects.None;
            }
            else if (angle > rightQuadrantStart && angle <= behindQuadrantStart)
            {
                EntitySprite.ActiveAnimation = (byte)AnimationIds.MoveRight;
                EntitySprite.SpriteEffects = SpriteEffects.None;
            }
            else if (angle > leftQuadrantStart && angle <= aheadQuadrantStart)
            {
                EntitySprite.ActiveAnimation = (byte)AnimationIds.MoveLeft;
                EntitySprite.SpriteEffects = SpriteEffects.FlipHorizontally;
            }
            else if (angle > behindQuadrantStart || angle <= leftQuadrantStart)
            {
                EntitySprite.ActiveAnimation = (byte)AnimationIds.MoveForward;
                EntitySprite.SpriteEffects = SpriteEffects.None;
            }
        }
    }
}
