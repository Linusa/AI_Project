namespace AIFGP_Game
{
    using System;
    using Microsoft.Xna.Framework;

    public static class Angles
    {
        /// <summary>
        /// Compute the angle in radians needed to rotate U in the direction
        /// of V.
        /// </summary>
        /// <param name="U">Must be normalized.</param>
        /// <param name="V">Must be normalized.</param>
        /// <returns>
        /// Angle from U to V. Result will be from 0 to pi if the closest
        /// rotation of U to V is clockwise. Result will be from 0 to -pi
        /// if the closest rotation of U to V is counter-clockwise.
        /// </returns>
        public static double AngleFromUToV(Vector2 U, Vector2 V)
        {
            Vector2 perpV = new Vector2(-V.Y, V.X);

            double dotResult1 = Vector2.Dot(U, perpV);
            double dotResult2 = Vector2.Dot(U, V);

            return -Math.Atan2(dotResult1, dotResult2);
        }
    }
}
