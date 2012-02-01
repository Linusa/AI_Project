namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    class EnemyManager : IUpdateable, IDrawable
    {
        public List<IGameEntity> Enemies = new List<IGameEntity>();

        public EnemyManager(Texture2D texture)
        {
            int basePad = 15;
            int xPad = SimpleGameEntity.Dimensions.Width + basePad;
            int yPad = SimpleGameEntity.Dimensions.Height + basePad;

            Vector2 paddedTopLeft = new Vector2(xPad, yPad);
            Vector2 paddedTopRight = new Vector2(SensorsGame.ScreenDimensions.Width - xPad, yPad);
            Vector2 paddedBottomLeft = new Vector2(xPad, SensorsGame.ScreenDimensions.Height - yPad);

            Enemies.Add(new SimpleGameEntity(texture, paddedTopLeft));
            Enemies.Add(new SimpleGameEntity(texture, paddedTopRight));
            Enemies.Add(new SimpleGameEntity(texture, paddedBottomLeft));
        }

        public void Update(GameTime gameTime)
        {
            foreach (IGameEntity entity in Enemies)
            {
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
