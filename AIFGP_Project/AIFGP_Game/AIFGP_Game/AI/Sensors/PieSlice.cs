namespace AIFGP_Game
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// The PieSlice sensor uses information gathered by a Radar
    /// instance and partitions the adjacent entities into
    /// pie slices based on location.
    /// </summary>
    public class PieSlice : ISensor
    {
        public enum PieSliceLocation
        {
            Ahead,
            Behind,
            Left,
            Right
        }

        // TODO: This should be private when debugging is removed from this class.
        public Radar radar;

        private List<RadarInfo> adjacentEntities = new List<RadarInfo>();

        // These starting positions adhere to a specific ordering so
        // be cautious if altering them.
        private const float aheadQuadrantStart = -50.0f;
        private const float rightQuadrantStart = 50.0f;
        private const float behindQuadrantStart = 130.0f;
        private const float leftQuadrantStart = -130.0f;

        private bool enabled = false;

        PieSliceDebugger pieSliceDebugger;

        public PieSlice(Radar radar)
        {
            this.radar = radar;
            Position = radar.SensingEntity.Position;

            pieSliceDebugger = new PieSliceDebugger(this, aheadQuadrantStart,
                rightQuadrantStart);
        }

        public void ActivationLevels(out Dictionary<PieSliceLocation, int> levels)
        {
            levels = new Dictionary<PieSliceLocation, int>(4);

            radar.AdjacentEntities(out adjacentEntities);

            levels.Add(PieSliceLocation.Ahead, 0);
            levels.Add(PieSliceLocation.Behind, 0);
            levels.Add(PieSliceLocation.Left, 0);
            levels.Add(PieSliceLocation.Right, 0);

            foreach (RadarInfo curRadarInfo in adjacentEntities)
            {
                float curAngle = MathHelper.ToDegrees(curRadarInfo.RelativeAngle);

                if (curAngle > aheadQuadrantStart && curAngle <= rightQuadrantStart)
                    levels[PieSliceLocation.Ahead]++;
                else if (curAngle > rightQuadrantStart && curAngle <= behindQuadrantStart)
                    levels[PieSliceLocation.Right]++;
                else if (curAngle > behindQuadrantStart || curAngle <= leftQuadrantStart)
                    levels[PieSliceLocation.Behind]++;
                else if (curAngle > leftQuadrantStart && curAngle <= aheadQuadrantStart)
                    levels[PieSliceLocation.Left]++;
            }
        }

        public bool IsSensingEnabled
        {
            get { return enabled; }
            set
            {
                enabled = value;
                pieSliceDebugger.IsDebuggingEnabled = value;
            }
        }

        public Vector2 Position
        {
            get { return radar.Position; }
            set { radar.Position = value; }
        }

        public void RotateInRadians(float radians)
        {
            pieSliceDebugger.RotateInRadians(radians);
        }

        public void RotateInDegrees(float degrees)
        {
            pieSliceDebugger.RotateInDegrees(degrees);
        }

        public void Update(GameTime gameTime)
        {
            // Press '3' to turn on debug display, '0' to disable.
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.D3))
            {
                IsSensingEnabled = true;
            }
            else if (keyboardState.IsKeyDown(Keys.D0))
            {
                IsSensingEnabled = false;
            }

            if (IsSensingEnabled)
            {
                pieSliceDebugger.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsSensingEnabled)
            {
                pieSliceDebugger.Draw(spriteBatch);
            }
        }
    }
}
