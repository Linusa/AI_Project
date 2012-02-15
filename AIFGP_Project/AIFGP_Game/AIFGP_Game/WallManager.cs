namespace AIFGP_Game
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;

    // WallManager is a singleton. Access it with Instance property.
    public class WallManager
    {
        // Note that this is public for reference access, but walls
        // should be added with the AddWall method.
        public List<Wall> Walls = new List<Wall>();

        // Adds a Wall. If possible, the Wall will be merged with an existing
        // wall.
        // TODO: THIS NEEDS TO BE FULLY TESTED!
        public void AddWall(Wall wallToAdd)
        {
            bool mergedWall = false;

            for (int i = 0; i < Walls.Count; i++)
            {
                Wall curWall = Walls[i];

                if (curWall.BoundingBox.Right == wallToAdd.BoundingBox.Left
                    && curWall.BoundingBox.Width >= curWall.BoundingBox.Height)
                {
                    curWall.BottomRightPixel = wallToAdd.BottomRightPixel;
                    mergedWall = true;
                }
                else if (curWall.BoundingBox.Left == wallToAdd.BoundingBox.Right
                    && curWall.BoundingBox.Width >= curWall.BoundingBox.Height)
                {
                    curWall.TopLeftPixel = wallToAdd.TopLeftPixel;
                    mergedWall = true;
                }
                else if (curWall.BoundingBox.Bottom == wallToAdd.BoundingBox.Top
                    && curWall.BoundingBox.Height >= curWall.BoundingBox.Width)
                {
                    curWall.BottomRightPixel = wallToAdd.BottomRightPixel;
                    mergedWall = true;
                }
                else if (curWall.BoundingBox.Top == wallToAdd.BoundingBox.Bottom
                    && curWall.BoundingBox.Height >= curWall.BoundingBox.Width)
                {
                    curWall.TopLeftPixel = wallToAdd.TopLeftPixel;
                    mergedWall = true;
                }

                Walls[i] = curWall;

                if (mergedWall)
                    break;
            }

            if (!mergedWall)
                Walls.Add(wallToAdd);
        }

        public BoundingBox WallExtentsIn3D(Wall wall)
        {
            Vector3 minPt = new Vector3();
            minPt.X = wall.BoundingBox.Left;
            minPt.Y = wall.BoundingBox.Top;
            minPt.Z = -10.0f;

            Vector3 maxPt = new Vector3();
            maxPt.X = wall.BoundingBox.Right;
            maxPt.Y = wall.BoundingBox.Bottom;
            maxPt.Z = 10.0f;

            return new BoundingBox(minPt, maxPt);
        }

        public static WallManager Instance {
            get
            {
                if (instance == null)
                    instance = new WallManager();
    
                return instance;
            }
        }
    
        // Single instance.
        private static WallManager instance;
    
        // Private constructor.
        private WallManager() { }
    }
}
