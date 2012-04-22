namespace AIFGP_Game
{
    using System.Collections.Generic;
    using AIFGP_Game_Data;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    class EnemyManager : IUpdateable, IDrawable
    {
        public bool FarmerCaughtRabbit = false;

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
                    IGameEntity curEnemy = new SmartFarmer(TextureManager.FarmerSprite, curEnemyPosition, curPatrolRoute);
                    curEnemy.MaxSpeed = curEnemyInfo.MaxSpeed;

                    Enemies.Add(curEnemy);
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            bool caughtRabbit = false;

            foreach (IGameEntity entity in Enemies)
            {
                float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
                entity.Update(gameTime);

                if (Vector2.DistanceSquared(entity.Position, player.Position) < 324.0f)
                    caughtRabbit = true;
            }

            FarmerCaughtRabbit = caughtRabbit;
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
