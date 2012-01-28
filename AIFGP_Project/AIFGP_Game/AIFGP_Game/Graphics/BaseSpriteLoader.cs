namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public abstract class BaseSpriteLoader<T> : ISpriteLoader<T>
    {
        public void Load(Texture2D texture, Vector2 position, out Sprite<T> sprite)
        {
            sprite = new Sprite<T>(texture, position);
            //sprite.Add
        }

        protected abstract void loadAnimations(out Dictionary<T, List<Rectangle>> anims);
    }
}
