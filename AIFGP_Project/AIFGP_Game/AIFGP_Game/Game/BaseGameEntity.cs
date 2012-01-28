namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public abstract class BaseGameEntity<T>
    {
        public Sprite<T> EntitySprite;
        public Vector2 Heading;

        public BaseGameEntity(Texture2D texture, Vector2 position)
        {
            EntitySprite = new Sprite<T>(texture, position);
        }

        protected abstract void initializeSprite();

        public virtual void Update(GameTime gameTime)
        {
            EntitySprite.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            EntitySprite.Draw(spriteBatch);
        }
    }
}
