namespace AIFGP_Game
{
    using System;
    using Microsoft.Xna.Framework;

    public struct Wall : ICollidable
    {
        public Vector2 TopLeftPixel;
        public Vector2 BottomRightPixel;

        public Wall(Vector2 topLeftPixel, Vector2 bottomRightPixel)
        {
            TopLeftPixel = topLeftPixel;
            BottomRightPixel = bottomRightPixel;
        }

        public Rectangle BoundingBox
        {
            get
            {
                int width = (int)(BottomRightPixel.X - TopLeftPixel.X);
                int height = (int)(BottomRightPixel.Y - TopLeftPixel.Y);

                return new Rectangle((int)TopLeftPixel.X, (int)TopLeftPixel.Y,
                    width, height);
            }
        }

        // BoundingRadius does not apply to rectangular walls, return null.
        public Nullable<float> BoundingRadius
        {
            get { return null; }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Wall))
                return false;

            return Equals((Wall)obj);
        }

        public bool Equals(Wall wall)
        {
            if (TopLeftPixel != wall.TopLeftPixel)
                return false;

            return BottomRightPixel == wall.BottomRightPixel;
        }

        public override int GetHashCode()
        {
            return TopLeftPixel.GetHashCode() ^ BottomRightPixel.GetHashCode();
        }
    }
}
