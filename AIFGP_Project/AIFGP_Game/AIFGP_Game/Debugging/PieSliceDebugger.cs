namespace AIFGP_Game
{
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class PieSliceDebugger : IUpdateable, IDrawable, IRotatable
    {
        private PieSlice pieSliceDebugging;
        
        private Line pieSliceDivider1;
        private Line pieSliceDivider2;
        
        private StringBuilder debugStringBuilder = new StringBuilder();

        private Dictionary<PieSlice.PieSliceLocation, int> debugActivationLevels =
            new Dictionary<PieSlice.PieSliceLocation, int>();

        // These vectors are used as locations to print the activation levels
        // for each pie slice.
        private Vector2 aheadVec = Vector2.Zero;
        private Vector2 behindVec = Vector2.Zero;
        private Vector2 leftVec = Vector2.Zero;
        private Vector2 rightVec = Vector2.Zero;

        private bool enabled = false;

        public PieSliceDebugger(PieSlice pieSlice, float lineAngle1, float lineAngle2)
        {
            pieSliceDebugging = pieSlice;
           
            pieSliceDivider1 = new Line(pieSliceDebugging.Position,
                (int)pieSliceDebugging.radar.EntityRange * 2, Color.PowderBlue);
            pieSliceDivider1.RotateInDegrees(lineAngle1);

            pieSliceDivider2 = new Line(pieSliceDebugging.Position,
                (int)pieSliceDebugging.radar.EntityRange * 2, Color.PowderBlue);
            pieSliceDivider2.RotateInDegrees(lineAngle2);
        }

        public bool IsDebuggingEnabled
        {
            get { return enabled; }
            set { enabled = value; }
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

        public void Update(GameTime gameTime)
        {
            pieSliceDivider1.Position = pieSliceDebugging.Position;
            pieSliceDivider2.Position = pieSliceDebugging.Position;

            // Store locations for debug printing.
            float vecScale = 100.0f;
            aheadVec = pieSliceDebugging.radar.SensingEntity.Heading;
            behindVec = -pieSliceDebugging.radar.SensingEntity.Heading;
            leftVec = new Vector2(aheadVec.Y, -aheadVec.X);
            rightVec = new Vector2(-aheadVec.Y, aheadVec.X);

            // Scale/position locations for debug printing.
            aheadVec = pieSliceDebugging.radar.SensingEntity.Position + aheadVec * vecScale;
            behindVec = pieSliceDebugging.radar.SensingEntity.Position + behindVec * vecScale;
            leftVec = pieSliceDebugging.radar.SensingEntity.Position + leftVec * vecScale;
            rightVec = pieSliceDebugging.radar.SensingEntity.Position + rightVec * vecScale;
           
            // Store the activation levels from the PieSlice reference.
            pieSliceDebugging.ActivationLevels(out debugActivationLevels);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            pieSliceDivider1.Draw(spriteBatch);
            pieSliceDivider2.Draw(spriteBatch);

            spriteBatch.DrawString(AStarGame.DebugFont,
                debugActivationLevels[PieSlice.PieSliceLocation.Ahead].ToString(),
                aheadVec, Color.Yellow);
            spriteBatch.DrawString(AStarGame.DebugFont,
                debugActivationLevels[PieSlice.PieSliceLocation.Behind].ToString(),
                behindVec, Color.Yellow);
            spriteBatch.DrawString(AStarGame.DebugFont,
                debugActivationLevels[PieSlice.PieSliceLocation.Left].ToString(),
                leftVec, Color.Yellow);
            spriteBatch.DrawString(AStarGame.DebugFont,
                debugActivationLevels[PieSlice.PieSliceLocation.Right].ToString(),
                rightVec, Color.Yellow);
        }
    }
}
