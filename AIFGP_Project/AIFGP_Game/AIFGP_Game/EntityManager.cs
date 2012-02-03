using System;
using System.Collections.Generic;
using System.Text;

namespace AIFGP_Game
{
    // EntityManager implemented as singleton.
    public class EntityManager
    {
        public Dictionary<Guid, IGameEntity> Entities = new Dictionary<Guid, IGameEntity>();

        public void RegisterEntity(IGameEntity entity)
        {
            Entities.Add(entity.ID, entity);
        }

        public IGameEntity GetEntity(Guid id)
        {
            return Entities[id];
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

        // Single instance.
        private static EntityManager instance;

        // Private constructor.
        private EntityManager() { }
    }
}
