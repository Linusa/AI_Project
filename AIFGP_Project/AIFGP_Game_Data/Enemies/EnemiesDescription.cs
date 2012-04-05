namespace AIFGP_Game_Data
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;

    public class EnemiesDescription
    {
        public struct EnemyInfo
        {
            public Vector2 StartingTilePosition;
            public float MaxSpeed;

            public List<Vector2> PatrolTilePositions;
        }

        public List<EnemyInfo> EnemiesInfo;
    }
}
