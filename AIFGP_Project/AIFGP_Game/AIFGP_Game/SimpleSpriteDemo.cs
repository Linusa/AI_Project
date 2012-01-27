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
        private int spriteSize = 30;
        private string[] animationIds = { "left", "right", "up", "down" };

        private Sprite<byte> backgroundTile;

        private Vector2 playerPosition = Vector2.Zero;
        private float playerSpeed = 2.5f;

        public SimpleSpriteDemo(Texture2D tex, Vector2 pos)
        {
            playerPosition = pos;
            playerSprite = new Sprite<string>(tex, pos);

            Rectangle startFrame = new Rectangle(0, 0, spriteSize, spriteSize);

            foreach (string id in animationIds)
            {
                for (int j = 0; j < 2; j++)
                {
                    int curX = startFrame.X;
                    int curY = startFrame.Y + (j * spriteSize);
                    Rectangle curFrame = new Rectangle(curX, curY, spriteSize, spriteSize);
                    playerSprite.AddAnimationFrame(id, curFrame);
                }

                startFrame.X += startFrame.Width;
            }

            playerSprite.ActiveAnimation = "down";
            playerSprite.AnimationRate = 0.1f;
            playerSprite.Scale = 1.25f;

            backgroundTile = new Sprite<byte>(tex, Vector2.Zero);
            Rectangle grassRect = new Rectangle(121, 1, 29, 29);
            backgroundTile.AddAnimationFrame(0, grassRect);
            backgroundTile.ActiveAnimation = 0;
        }

        public void Update(GameTime gameTime)
        {
            handleInput();
            playerSprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 tempPos = Vector2.Zero;
            for (int i = 0; i < 600 / 29 + 2; i++)
            {
                for (int j = 0; j < 800 / 29 + 2; j++)
                {
                    backgroundTile.Draw(spriteBatch);
                    backgroundTile.Position += new Vector2(28, 0);
                }

                backgroundTile.Position = new Vector2(0, i * 28);
            }

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
                playerSprite.PlayAnimation();
            }
            else
            {
                playerSprite.PauseAnimation();
            }
        }

        void wrapPosition()
        {
            Rectangle bounds = new Rectangle(0, 0, 800, 600);

            if (playerPosition.X < bounds.X - spriteSize)
            {
                playerPosition.X = bounds.Width;
            }
            else if (playerPosition.X > bounds.Width)
            {
                playerPosition.X = bounds.X - spriteSize;
            }

            if (playerPosition.Y < bounds.Y - spriteSize)
            {
                playerPosition.Y = bounds.Height;
            }
            else if (playerPosition.Y > bounds.Height)
            {
                playerPosition.Y = bounds.Y - spriteSize;
            }
        }
    }
}
