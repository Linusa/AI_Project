namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SimplePlayer
    {
        private Sprite<string> sprite;

        string[] animationIds = { "left", "right", "up", "down" };
        int animationIdsIdx = 0;

        Timer animationCycleTime = new Timer(3.0f);

        public SimplePlayer(Texture2D texture, Vector2 position)
        {
            sprite = new Sprite<string>(texture, position);

            int spriteSize = 30;
            Rectangle startFrame = new Rectangle(0, 0, spriteSize, spriteSize);

            foreach (string id in animationIds)
            {
                for (int i = 0; i < 2; i++)
                {
                    int curX = startFrame.X;
                    int curY = startFrame.Y + (i * spriteSize);
                    Rectangle curFrame = new Rectangle(curX, curY, spriteSize, spriteSize);
                    sprite.AddAnimationFrame(id, curFrame);
                }

                startFrame.X += startFrame.Width;
            }

            sprite.ActiveAnimation = animationIds[0];
            sprite.AnimationRate = 0.15f;

            animationCycleTime.Start();
        }

        public void Update(GameTime gameTime)
        {
            if (animationCycleTime.Expired(gameTime))
            {
                animationIdsIdx = (animationIdsIdx + 1) % animationIds.Length;
                sprite.ActiveAnimation = animationIds[animationIdsIdx];
            }

            sprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
        }
    }
}
