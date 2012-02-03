namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public struct RadarInfo
    {
        public Guid EntityId;
        public float RelativeAngle;

        public RadarInfo(Guid id, float angle)
        {
            EntityId = id;
            RelativeAngle = angle;
        }
    }
}
