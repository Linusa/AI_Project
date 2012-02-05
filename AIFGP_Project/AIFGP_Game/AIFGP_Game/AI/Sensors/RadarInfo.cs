namespace AIFGP_Game
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public struct RadarInfo
    {
        public Guid EntityId;
        public float Distance;
        public float RelativeAngle;

        public RadarInfo(Guid id, float distance, float angle)
        {
            EntityId = id;
            Distance = distance;
            RelativeAngle = angle;
        }
    }
}
