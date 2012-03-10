namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    class EnemyManager : IUpdateable, IDrawable
    {
        public List<IGameEntity> Enemies = new List<IGameEntity>();

        private IGameEntity entityFollowingMouse = null;
        private bool followingMouse = false;

        private Random rng = new Random();

        public EnemyManager()
        {
            int xBasePad = 180;
            int xPad = SimpleGameEntity.Dimensions.Width;
            int yPad = SimpleGameEntity.Dimensions.Height;

            Vector2 startLocation = new Vector2(xBasePad + xPad, yPad);
            Vector2 curEnemyPosition = startLocation;

            int numEnemies = 0;
            for (int i = 0; i < numEnemies; i++)
            {
                Enemies.Add(new SimpleGameEntity(AStarGame.NpcSpriteSheet, curEnemyPosition));
                Enemies[i].RotateInDegrees(rng.Next(360));
                curEnemyPosition.X += xPad;
            }
        }

        public void Update(GameTime gameTime)
        {
            // Allows enemies to be positioned with the mouse.
            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Released)
            {
                followingMouse = false;
                entityFollowingMouse = null;
            }

            if (followingMouse)
            {
                entityFollowingMouse.Position = new Vector2(mouseState.X, mouseState.Y);
            }

            foreach (IGameEntity entity in Enemies)
            {
                if (!entity.Equals(entityFollowingMouse))
                {
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        Vector2 mouseVec = new Vector2(mouseState.X, mouseState.Y) - entity.Position;

                        if (mouseVec.LengthSquared() < entity.BoundingRadius * entity.BoundingRadius)
                        {
                            followingMouse = true;
                            entityFollowingMouse = entity;
                        }
                    }
                }

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
