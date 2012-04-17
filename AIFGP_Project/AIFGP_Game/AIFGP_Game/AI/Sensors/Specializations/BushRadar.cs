

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
            List<float> distances = new List<float>();

            foreach (Vector2 bush in AStarGame.GameMap.BushLocations)
            {
                if (Vector2.Subtract(position, bush).LengthSquared() < radiusSqrd)
                {
                    if (distances.Count == 0)
                    {
                        bushes.Add(bush);
                        distances.Add((bush - position).LengthSquared());
                    }
                    else if ((bush - position).LengthSquared() < distances[0])
                    {
                        bushes.Insert(0, bush);
                        distances.Insert(0, (bush - position).LengthSquared());
                    }
                    else
                    {
                        for (int i = 0; i < bushes.Count - 1; i++)
                        {
                            if ((bush - bushes[i]).LengthSquared() < distances[i + 1])
                            {
                                distances.Insert(i + 1, (bush - bushes[i]).LengthSquared());
                                bushes.Insert(i + 1, bush);
                                break;
                            }
                            else if (i == bushes.Count - 2)
                            {
                                distances.Add((bush - bushes[i+1]).LengthSquared());
                                bushes.Add(bush);
                                break;
                            }
                        }
                    }
                }
            }

            return bushes;
        }
    }
}
