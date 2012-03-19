namespace AIFGP_Game
{
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Debugs Radar by showing graphical information about it.
    /// </summary>
    public class RangefinderDebugger : IUpdateable, IDrawable, IRotatable
    {
        private class FeelerLine
        {
            public int FeelerIndex;
            public Line VisualLine;

            public FeelerLine(int feelerIdx, Line line)
            {
                FeelerIndex = feelerIdx;
                VisualLine = line;
            }
        }

        private Rangefinder rangefinderDebugging;

        private List<FeelerLine> feelerLines = new List<FeelerLine>();

        private bool enabled = false;
        private Vector2 debugLoc = new Vector2(15.0f);
        private StringBuilder strBuilder = new StringBuilder();

        public RangefinderDebugger(Rangefinder rangefinder)
        {
            rangefinderDebugging = rangefinder;

            for (int i = 0; i < rangefinderDebugging.Feelers.Count; i++)
            {
                Ray curFeeler = rangefinderDebugging.Feelers[i];

                Vector2 position2f = new Vector2(curFeeler.Position.X, curFeeler.Position.Y);
                Vector2 entityDir2f = rangefinderDebugging.SensingEntity.Heading;
                Vector2 feelerDir2f = new Vector2(curFeeler.Direction.X, curFeeler.Direction.Y);

                Line curLine = new Line(position2f, Rangefinder.MaxRayDistance, Color.PowderBlue);

                float radians = (float)Angles.AngleFromUToV(entityDir2f, feelerDir2f);
                curLine.RotateInRadians(radians);

                curLine.LineSprite.LocalOrigin = new Vector2(0.0f, curLine.LineSprite.BoundingBox.Height / 2);

                feelerLines.Add(new FeelerLine(i, curLine));
            }
        }

        public bool IsDebuggingEnabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public void RotateInRadians(float radians)
        {
            foreach (FeelerLine line in feelerLines)
            {
                line.VisualLine.RotateInRadians(radians);
            }
        }

        public void RotateInDegrees(float degrees)
        {
            RotateInRadians(MathHelper.ToRadians(degrees));
        }

        public void Update(GameTime gameTime)
        {
            strBuilder.Clear();
            strBuilder.Append("RANGE_FINDER\n================\n");

            int curLine = 1;
            foreach (FeelerLine line in feelerLines)
            {
                line.VisualLine.Position = rangefinderDebugging.Position;

                Ray curRay = rangefinderDebugging.Feelers[line.FeelerIndex];
                float curIntersectDist = rangefinderDebugging.DistanceToWall(curRay);
                if (curIntersectDist < Rangefinder.MaxRayDistance && curIntersectDist != -1.0f)
                {
                    Vector3 offset3f = curRay.Direction * (Rangefinder.MaxRayDistance - curIntersectDist);
                    Vector2 offset = new Vector2(offset3f.X, offset3f.Y);
                    line.VisualLine.Position -= offset;
                }

                strBuilder.AppendFormat("Dist {0}: {1}\n", curLine, curIntersectDist);
                curLine++;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (FeelerLine line in feelerLines)
            {
                line.VisualLine.Draw(spriteBatch);
                spriteBatch.DrawString(AStarGame.DebugFont, strBuilder,
                    debugLoc, Color.Yellow);
            }
        }
    }
}
