namespace AIFGP_Game
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;

    // WallManager is a singleton. Access it with Instance property.
    public class WallManager
    {
        // Note that this is public for easy access, but walls
        // should be added with the AddWall method.
        public List<Wall> Walls = new List<Wall>();

        // TODO: Merging code not working 100% correctly, so each small
        // Wall is just added for now. This is not efficient at all,
        // and must be changed later.
        public void AddWall(Wall wall)
        {
            
            bool mergedWall = false;

            for (int i = 0; i < Walls.Count; i++)
            {
                Wall otherWall = Walls[i];

                if (otherWall.BoundingBox.Right == wall.BoundingBox.Left
                    && otherWall.BoundingBox.Width >= otherWall.BoundingBox.Height
                    && otherWall.BoundingBox.Top == wall.BoundingBox.Top)
                {
                    otherWall.BottomRightPixel = wall.BottomRightPixel;
                    mergedWall = true;
                }
                else if (otherWall.BoundingBox.Left == wall.BoundingBox.Right
                    && otherWall.BoundingBox.Width >= otherWall.BoundingBox.Height
                    && otherWall.BoundingBox.Top == wall.BoundingBox.Top)
                {
                    otherWall.TopLeftPixel = wall.TopLeftPixel;
                    mergedWall = true;
                }
                else if (otherWall.BoundingBox.Bottom == wall.BoundingBox.Top
                    && otherWall.BoundingBox.Height >= otherWall.BoundingBox.Width
                    && otherWall.BoundingBox.Left == wall.BoundingBox.Left)
                {
                    otherWall.BottomRightPixel = wall.BottomRightPixel;
                    mergedWall = true;
                }
                else if (otherWall.BoundingBox.Top == wall.BoundingBox.Bottom
                    && otherWall.BoundingBox.Height >= otherWall.BoundingBox.Width
                    && otherWall.BoundingBox.Left == wall.BoundingBox.Left)
                {
                    otherWall.TopLeftPixel = wall.TopLeftPixel;
                    mergedWall = true;
                }

                //Walls[i] = wall;

                if (mergedWall)
                {
                    Walls[i] = otherWall;
                    break;
                }
            }

            if (!mergedWall)
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
