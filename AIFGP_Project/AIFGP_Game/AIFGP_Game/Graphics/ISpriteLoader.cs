namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISpriteLoader<T>
    {
        void Load(Texture2D texture, Vector2 position, out Sprite<T> sprite);
    }
}
