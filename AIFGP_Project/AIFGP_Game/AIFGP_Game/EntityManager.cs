using System;
using System.Collections.Generic;

namespace AIFGP_Game
{
    // EntityManager is a singleton. Access it with Instance property.
    public class EntityManager
    {
        public Dictionary<Guid, IGameEntity> Entities = new Dictionary<Guid, IGameEntity>();
        public bool playerHidden = false;
        public Guid PlayerID;

        public void RegisterEntity(IGameEntity entity)
        {
            Entities.Add(entity.ID, entity);
        }

        public IGameEntity GetEntity(Guid id)
        {
            return Entities[id];
        }

        public IGameEntity GetPlayer()
        {
            return Entities[PlayerID];
        }

        public static EntityManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new EntityManager();

                return instance;
            }
        }

        // Single instance.
        private static EntityManager instance;

        // Private constructor.
        private EntityManager() { }
    }
}
