namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Rabbit : BaseGameEntity
    {
        public enum AnimationIds
        {
            LookForward,
            LookLeft,
            LookBack,
            LookRight,
            ExcitedLookForward,
            ExcitedLookLeft,
            ExcitedLookBack,
            ExcitedLookRight,
            HopForward,
            HopLeft,
            HopBack,
            HopRight,
            ExcitedHopForward,
            ExcitedHopLeft,
            ExcitedHopBack,
            ExcitedHopRight
        }

        private static Rectangle dimensions = new Rectangle(0, 0, 34, 36);

        public Rabbit(Texture2D texture, Vector2 position)
            : base(texture, position, dimensions)
        {
            Heading = new Vector2(1.0f, 0.0f);
        }
        
        public override Rectangle BoundingBox
        {
            get {return EntitySprite.BoundingBox; }
        }

        public override Nullable<float> BoundingRadius
        {
            get { return EntitySprite.BoundingBox.Width / 2; }
        }

        protected override void configureSprite()
        {
            int numCols = 4;

            Rectangle curFrame = dimensions;

            EntitySprite.AddAnimationFrame((byte)AnimationIds.LookForward, curFrame);
            curFrame.X += curFrame.Width;
            EntitySprite.AddAnimationFrame((byte)AnimationIds.LookLeft, curFrame);
            curFrame.X += curFrame.Width;
            EntitySprite.AddAnimationFrame((byte)AnimationIds.LookBack, curFrame);
            curFrame.X += curFrame.Width;
            EntitySprite.AddAnimationFrame((byte)AnimationIds.LookRight, curFrame);

            curFrame.X = 0;
            curFrame.Y += curFrame.Height;

            EntitySprite.AddAnimationFrame((byte)AnimationIds.ExcitedLookForward, curFrame);
            curFrame.X += curFrame.Width;
            EntitySprite.AddAnimationFrame((byte)AnimationIds.ExcitedLookLeft, curFrame);
            curFrame.X += curFrame.Width;
            EntitySprite.AddAnimationFrame((byte)AnimationIds.ExcitedLookBack, curFrame);
            curFrame.X += curFrame.Width;
            EntitySprite.AddAnimationFrame((byte)AnimationIds.ExcitedLookRight, curFrame);

            curFrame.X = 0;
            curFrame.Y += curFrame.Height;

            for (int i = 0; i < numCols; i++)
            {
                EntitySprite.AddAnimationFrame((byte)AnimationIds.HopForward, curFrame);
                curFrame.X += curFrame.Width;
            }
            
            curFrame.X = 0;
            curFrame.Y += curFrame.Height;

            for (int i = 0; i < numCols; i++)
            {
                EntitySprite.AddAnimationFrame((byte)AnimationIds.HopLeft, curFrame);
                curFrame.X += curFrame.Width;
            }
            
            curFrame.X = 0;
            curFrame.Y += curFrame.Height;

            for (int i = 0; i < numCols; i++)
            {
                EntitySprite.AddAnimationFrame((byte)AnimationIds.HopBack, curFrame);
                curFrame.X += curFrame.Width;
            }
            
            curFrame.X = 0;
            curFrame.Y += curFrame.Height;

            for (int i = 0; i < numCols; i++)
            {
                EntitySprite.AddAnimationFrame((byte)AnimationIds.HopRight, curFrame);
                curFrame.X += curFrame.Width;
            }
            
            curFrame.X = 0;
            curFrame.Y += curFrame.Height;

            for (int i = 0; i < numCols; i++)
            {
                EntitySprite.AddAnimationFrame((byte)AnimationIds.ExcitedHopForward, curFrame);
                curFrame.X += curFrame.Width;
            }
            
            curFrame.X = 0;
            curFrame.Y += curFrame.Height;

            for (int i = 0; i < numCols; i++)
            {
                EntitySprite.AddAnimationFrame((byte)AnimationIds.ExcitedHopLeft, curFrame);
                curFrame.X += curFrame.Width;
            }
            
            curFrame.X = 0;
            curFrame.Y += curFrame.Height;

            for (int i = 0; i < numCols; i++)
            {
                EntitySprite.AddAnimationFrame((byte)AnimationIds.ExcitedHopBack, curFrame);
                curFrame.X += curFrame.Width;
            }
            
            curFrame.X = 0;
            curFrame.Y += curFrame.Height;

            for (int i = 0; i < numCols; i++)
            {
                EntitySprite.AddAnimationFrame((byte)AnimationIds.ExcitedHopRight, curFrame);
                curFrame.X += curFrame.Width;
            }

            EntitySprite.ActiveAnimation = (byte)AnimationIds.ExcitedLookForward;
            EntitySprite.PlayAnimation();
        }
    }
}
