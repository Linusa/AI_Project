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
        public void AddWall(Wall wall)
        {
            /*
            bool mergedWall = false;

            for (int i = 0; i < Walls.Count; i++)
            {
                Wall wall = Walls[i];

                if (wall.BoundingBox.Right == wall.BoundingBox.Left
                    && wall.BoundingBox.Width >= wall.BoundingBox.Height)
                {
                    wall.BottomRightPixel = wall.BottomRightPixel;
                    mergedWall = true;
                }
                else if (wall.BoundingBox.Left == wall.BoundingBox.Right
                    && wall.BoundingBox.Width >= wall.BoundingBox.Height)
                {
                    wall.TopLeftPixel = wall.TopLeftPixel;
                    mergedWall = true;
                }
                else if (wall.BoundingBox.Bottom == wall.BoundingBox.Top
                    && wall.BoundingBox.Height >= wall.BoundingBox.Width)
                {
                    wall.BottomRightPixel = wall.BottomRightPixel;
                    mergedWall = true;
                }
                else if (wall.BoundingBox.Top == wall.BoundingBox.Bottom
                    && wall.BoundingBox.Height >= wall.BoundingBox.Width)
                {
                    wall.TopLeftPixel = wall.TopLeftPixel;
                    mergedWall = true;
                }

                Walls[i] = wall;

                if (mergedWall)
                    break;
            }

            if (!mergedWall)
                Walls.Add(wall);

            Walls.Add(wall);
            */

            if (!Walls.Contains(wall))
                Walls.Add(wall);
        }

        public void RemoveWall(Wall wall)
        {
            if (Walls.Contains(wall))
                Walls.RemoveAt(Walls.FindIndex(w => w.Equals(wall)));
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
