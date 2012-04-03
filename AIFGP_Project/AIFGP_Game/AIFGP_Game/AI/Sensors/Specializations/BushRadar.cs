

namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    
    public class BushRadar
    {
        const int radiusSqrd = 250000;

        public static List<Vector2> getBushes()
        {
            List<Vector2> bushes = new List<Vector2>();
            Vector2 position = EntityManager.Instance.GetPlayer().Position;


            foreach (Vector2 bush in Map.bushes)
            {
                if (Vector2.Subtract(position, bush).LengthSquared() < radiusSqrd)
                    bushes.Add(bush);
            }

            return bushes;
        }
    }
}
