using System;
using System.Collections.Generic;
using System.Text;

namespace AIFGP_Game
{
    // EntityManager implemented as singleton.
    public class EntityManager
    {
        public List<IGameEntity> Entities = new List<IGameEntity>();

        public void RegisterEntity(IGameEntity entity)
        {
            Entities.Add(entity);
        }

        public static EntityManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EntityManager();
                }

                return instance;
            }
        }

        // Private constructor.
        private EntityManager() { }

        // Single instance.
        private static EntityManager instance;
    }
}
