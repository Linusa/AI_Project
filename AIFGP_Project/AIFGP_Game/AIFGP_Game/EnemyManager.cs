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

        IGameEntity entityFollowingMouse = null;
        bool followingMouse = false;

        public EnemyManager()
        {
            int basePad = 15;
            int xPad = SimpleGameEntity.Dimensions.Width + basePad;
            int yPad = SimpleGameEntity.Dimensions.Height + basePad;

            Vector2 paddedTopLeft = new Vector2(xPad, yPad);
            Vector2 paddedTopRight = new Vector2(SensorsGame.ScreenDimensions.Width - xPad, yPad);
            Vector2 paddedBottomLeft = new Vector2(xPad, SensorsGame.ScreenDimensions.Height - yPad);

            Enemies.Add(new SimpleGameEntity(SensorsGame.NpcSpriteSheet, paddedTopLeft));
            Enemies.Add(new SimpleGameEntity(SensorsGame.NpcSpriteSheet, paddedTopRight));
            Enemies.Add(new SimpleGameEntity(SensorsGame.NpcSpriteSheet, paddedBottomLeft));
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
