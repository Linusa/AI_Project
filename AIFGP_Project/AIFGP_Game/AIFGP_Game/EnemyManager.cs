namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    class EnemyManager : IUpdateable, IDrawable
    {
        public List<IGameEntity> Enemies = new List<IGameEntity>();

        private IGameEntity player;

        private Random rng = new Random();

        public EnemyManager()
        {
            player = EntityManager.Instance.GetPlayer();

            int xBasePad = 180;
            int xPad = SimpleGameEntity.Dimensions.Width;
            int yPad = SimpleGameEntity.Dimensions.Height;

            Vector2 startLocation = new Vector2(xBasePad + xPad, yPad);
            Vector2 curEnemyPosition = startLocation;
            List<Vector2> route = new List<Vector2>();
            route.Add(new Vector2(100, 100));
            route.Add(new Vector2(500, 500));
            int numEnemies = 1;
            for (int i = 0; i < numEnemies; i++)
            {
                Enemies.Add(new SimpleSensingGameEntity(AStarGame.NpcSpriteSheet, curEnemyPosition, route));
                Enemies[i].RotateInDegrees(rng.Next(360));
                curEnemyPosition.X += xPad;
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
