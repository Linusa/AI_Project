namespace AIFGP_Game
{
    using System;
    using Microsoft.Xna.Framework;

    public static class Angles
    {
        /// <summary>
        /// Compute the angle in radians needed to rotate U so that U's
        /// </summary>
        /// <param name="U"></param>
        /// <param name="V"></param>
        /// <returns></returns>
        //public static double AngleFromUToV(Vector2 U, Vector2 V)
        //{
        //    Vector2 perpU = new Vector2(-U.Y, U.X);

        //    double dotResult1 = Vector2.Dot(perpU, V);
        //    double dotResult2 = Vector2.Dot(U, V);

        //    return Math.Atan2(dotResult1, dotResult2);
        //}

        public static double AngleFromUToV(Vector2 U, Vector2 V)
        {
            Vector2 perpV = new Vector2(-V.Y, V.X);

            double dotResult1 = Vector2.Dot(U, perpV);
            double dotResult2 = Vector2.Dot(U, V);

            return Math.Atan2(dotResult1, dotResult2);
        }
    }
}
