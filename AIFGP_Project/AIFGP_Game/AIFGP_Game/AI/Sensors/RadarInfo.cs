namespace AIFGP_Game
{
    using System;

    /// <summary>
    /// Simple struct that contains valuable spatial information
    /// for an entity relative to the entity whose Radar returned
    /// this info.
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
