namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using AIFGP_Game_Data;

    class EnemyManager : IUpdateable, IDrawable
    {
        public List<IGameEntity> Enemies = new List<IGameEntity>();

        private IGameEntity player;

        public EnemyManager(EnemiesDescription enemiesDescription)
        {
            player = EntityManager.Instance.GetPlayer();

            if (enemiesDescription.EnemiesInfo != null)
            {
                foreach (EnemiesDescription.EnemyInfo curEnemyInfo in enemiesDescription.EnemiesInfo)
                {
                    List<Vector2> curPatrolRoute = curEnemyInfo.PatrolTilePositions;
                    for (int i = 0; i < curPatrolRoute.Count; i++)
                        curPatrolRoute[i] = AStarGame.GameMap.TilePosToWorldPos(curPatrolRoute[i]);

                    Vector2 curEnemyPosition = AStarGame.GameMap.TilePosToWorldPos(curEnemyInfo.StartingTilePosition);
                    IGameEntity curEnemy = new SimpleSensingGameEntity(TextureManager.NpcSpriteSheet, curEnemyPosition, curPatrolRoute);
                    curEnemy.MaxSpeed = curEnemyInfo.MaxSpeed;

                    Enemies.Add(curEnemy);
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (IGameEntity entity in Enemies)
            {
                float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
                //entity.Velocity += entity.Seek(player.Position) * dt;

                entity.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (IGameEntity entity in Enemies)
            {
                entity.Draw(spriteBatch);
            }
        }
    }
}
