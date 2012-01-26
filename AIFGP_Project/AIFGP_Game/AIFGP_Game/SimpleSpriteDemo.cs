namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// This is a demonstration class and will not be used later. This
    /// just throws a few things together to be able to control a little
    /// 2D dude.
    /// </summary>
    public class SimpleSpriteDemo
    {
        private Sprite<string> playerSprite;
        private int playerSpriteSize = 30;
        private string[] animationIds = { "left", "right", "up", "down", "still" };

        private Vector2 playerPosition = Vector2.Zero;
        private float playerSpeed = 2.5f;

        public SimpleSpriteDemo(Texture2D tex, Vector2 pos)
        {
            playerPosition = pos;
            playerSprite = new Sprite<string>(tex, pos);

            Rectangle startFrame = new Rectangle(0, 0, playerSpriteSize, playerSpriteSize);

            for (int i = 0; i < animationIds.Length - 1; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    int curX = startFrame.X;
                    int curY = startFrame.Y + (j * playerSpriteSize);
                    Rectangle curFrame = new Rectangle(curX, curY, playerSpriteSize, playerSpriteSize);
                    playerSprite.AddAnimationFrame(animationIds[i], curFrame);

                    // Hack to be sure we add in the "still" 1-frame animation.
                    if (i == animationIds.Length - 2 && j == 1)
                    {
                        playerSprite.AddAnimationFrame(animationIds[i+1], curFrame);
                        break;
                    }
                }

                startFrame.X += startFrame.Width;
            }

            playerSprite.ActiveAnimation = "still";
            playerSprite.AnimationRate = 0.1f;
            playerSprite.Scale = 1.25f;
        }

        public void Update(GameTime gameTime)
        {
            handleInput();
            playerSprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            playerSprite.Draw(spriteBatch);
        }

        private void handleInput()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.GetPressedKeys().Length > 0)
            {
                if (keyboardState.IsKeyDown(Keys.Left))
                {
                    playerPosition += (new Vector2(-1, 0) * playerSpeed);
                    playerSprite.ActiveAnimation = "left";
                }

                if (keyboardState.IsKeyDown(Keys.Right))
                {
                    playerPosition += (new Vector2(1, 0) * playerSpeed);
                    playerSprite.ActiveAnimation = "right";
                }

                if (keyboardState.IsKeyDown(Keys.Up))
                {
                    playerPosition += (new Vector2(0, -1) * playerSpeed);
                    playerSprite.ActiveAnimation = "up";
                }

                if (keyboardState.IsKeyDown(Keys.Down))
                {
                    playerPosition += (new Vector2(0, 1) * playerSpeed);
                    playerSprite.ActiveAnimation = "down";
                }

                wrapPosition();
                playerSprite.Position = playerPosition;
            }
            else
            {
                playerSprite.ActiveAnimation = "still";
            }
        }

        void wrapPosition()
        {
            Rectangle bounds = new Rectangle(0, 0, 800, 600);

            if (playerPosition.X < bounds.X - playerSpriteSize)
            {
                playerPosition.X = bounds.Width;
            }
            else if (playerPosition.X > bounds.Width)
            {
                playerPosition.X = bounds.X - playerSpriteSize;
            }

            if (playerPosition.Y < bounds.Y - playerSpriteSize)
            {
                playerPosition.Y = bounds.Height;
            }
            else if (playerPosition.Y > bounds.Height)
            {
                playerPosition.Y = bounds.Y - playerSpriteSize;
            }
        }
    }
}
