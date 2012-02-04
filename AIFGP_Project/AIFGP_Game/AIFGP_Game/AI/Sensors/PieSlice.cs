namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class PieSlice : ISensor
    {
        public enum PieSliceLocation
        {
            Ahead,
            Behind,
            Left,
            Right
        }

        private Radar radar;

        private List<RadarInfo> adjacentEntities = new List<RadarInfo>();

        private Line pieSliceDivider1;
        private Line pieSliceDivider2;

        // These starting positions adhere to a specific ordering so
        // be cautious if altering them.
        private const float aheadQuadrantStart = -50.0f;
        private const float rightQuadrantStart = 50.0f;
        private const float behindQuadrantStart = 130.0f;
        private const float leftQuadrantStart = -130.0f;

        private bool enabled = false;

        // DEBUG MEMBERS
        private StringBuilder debugStringBuilder = new StringBuilder();
        Dictionary<PieSliceLocation, int> debugActivationLevels = new Dictionary<PieSliceLocation, int>();
        Vector2 aheadVec = Vector2.Zero;
        Vector2 behindVec = Vector2.Zero;
        Vector2 leftVec = Vector2.Zero;
        Vector2 rightVec = Vector2.Zero;
        // END DEBUG MEMBERS

        public PieSlice(Radar radar)
        {
            this.radar = radar;

            pieSliceDivider1 = new Line(this.radar.Position,
                (int)this.radar.EntityRange * 2, Color.PowderBlue);
            pieSliceDivider1.RotateInDegrees(aheadQuadrantStart);

            pieSliceDivider2 = new Line(this.radar.Position,
                (int)this.radar.EntityRange * 2, Color.PowderBlue);
            pieSliceDivider2.RotateInDegrees(rightQuadrantStart);

            Position = radar.SensingEntity.Position;
        }

        public void ActivationLevels(out Dictionary<PieSliceLocation, int> levels)
        {
            levels = new Dictionary<PieSliceLocation, int>(4);

            radar.AdjacentEntities(out adjacentEntities);

            // Store locations for debug printing.
            float vecScale = 100.0f;
            aheadVec = radar.SensingEntity.Heading;
            behindVec = -radar.SensingEntity.Heading;
            leftVec = new Vector2(aheadVec.Y, -aheadVec.X);
            rightVec = new Vector2(-aheadVec.Y, aheadVec.X);

            // Scale/position locations for debug printing.
            aheadVec = radar.SensingEntity.Position + aheadVec * vecScale;
            behindVec = radar.SensingEntity.Position + behindVec * vecScale;
            leftVec = radar.SensingEntity.Position + leftVec * vecScale;
            rightVec = radar.SensingEntity.Position + rightVec * vecScale;

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
            set { enabled = value; }
        }

        public Vector2 Position
        {
            get { return radar.Position; }
            set
            {
                radar.Position = value;
                pieSliceDivider1.Position = radar.SensingEntity.Position;
                pieSliceDivider2.Position = radar.SensingEntity.Position;
            }
        }

        public void Translate(Vector2 offset)
        {
            Position += offset;
        }

        public void Translate(int x, int y)
        {
            Vector2 offset = new Vector2(x, y);
            Position += offset;
        }

        public void RotateInRadians(float radians)
        {
            pieSliceDivider1.RotateInRadians(radians);
            pieSliceDivider2.RotateInRadians(radians);
        }

        public void RotateInDegrees(float degrees)
        {
            RotateInRadians(MathHelper.ToRadians(degrees));
        }

        public void Scale(Vector2 scale)
        {
            pieSliceDivider1.Scale(scale);
            pieSliceDivider2.Scale(scale);
        }

        public void Scale(float scale)
        {
            pieSliceDivider1.Scale(scale);
            pieSliceDivider2.Scale(scale);
        }

        public void Update(GameTime gameTime)
        {
            // Press '3' to turn on, '0' to disable.
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
                ActivationLevels(out debugActivationLevels);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsSensingEnabled)
            {
                pieSliceDivider1.Draw(spriteBatch);
                pieSliceDivider2.Draw(spriteBatch);

                // DEBUGGING TEXT DISPLAY
                spriteBatch.DrawString(SensorsGame.DebugFont,
                    debugActivationLevels[PieSliceLocation.Ahead].ToString(),
                    aheadVec, Color.Yellow);
                spriteBatch.DrawString(SensorsGame.DebugFont,
                    debugActivationLevels[PieSliceLocation.Behind].ToString(),
                    behindVec, Color.Yellow);
                spriteBatch.DrawString(SensorsGame.DebugFont,
                    debugActivationLevels[PieSliceLocation.Left].ToString(),
                    leftVec, Color.Yellow);
                spriteBatch.DrawString(SensorsGame.DebugFont,
                    debugActivationLevels[PieSliceLocation.Right].ToString(),
                    rightVec, Color.Yellow);
                // END DEBUGGING TEXT DISPLAY
            }
        }
    }
}
