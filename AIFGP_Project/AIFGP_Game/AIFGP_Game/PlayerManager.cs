namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PlayerManager : IUpdateable, IDrawable
    {
        public IGameEntity Player;
        private float playerSpeed = 4.0f;

        // DEBUG MEMBERS
        private bool debugOutputKB = false;
        private bool debugOutputGP = false;
        Vector2 debugLoc = Vector2.Zero;
        private StringBuilder debugStringBuilder = new StringBuilder();
        // END DEBUG MEMBERS

        public PlayerManager()
        {
            Player = new SimpleSensingGameEntity(SensorsGame.PlayerSpriteSheet, SensorsGame.ScreenCenter);
        }

        private void checkKeyboard()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Up))
            {
                Player.Position += (Player.Heading * playerSpeed);
            }

            if (keyboardState.IsKeyDown(Keys.Down))
            {
                Player.Position -= (Player.Heading * playerSpeed);
            }

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                Player.RotateInDegrees(-3.0f);
            }

            if (keyboardState.IsKeyDown(Keys.Right))
            {
                Player.RotateInDegrees(3.0f);
            }

            #region KEY CHECKS FOR DEBUG OUTPUT
            if (keyboardState.IsKeyDown(Keys.LeftAlt))
            {
                debugOutputKB = true;
                debugStringBuilder.Clear();
                debugStringBuilder.AppendFormat("Pos: {0}\nDir: {1}",
                    Player.Position,
                    Player.Heading
                );

                Vector2 offset = new Vector2(Player.BoundingBox.Width + 10, -Player.BoundingBox.Height + 10) / 2.0f;
                if (Player.Position.X > SensorsGame.ScreenCenter.X)
                {
                    Vector2 stringSize = SensorsGame.DebugFont.MeasureString(debugStringBuilder);
                    offset.X -= stringSize.X + Player.BoundingBox.Width + 10;
                }

                debugLoc = Player.Position + offset;
            }
            else if (keyboardState.IsKeyUp(Keys.LeftAlt))
            {
                debugOutputKB = false;
            }
            #endregion
        }

        private void checkController()
        {
            GamePadState controllerState = GamePad.GetState(PlayerIndex.One);

            float xVal = controllerState.ThumbSticks.Right.X;
            float yVal = controllerState.ThumbSticks.Left.Y;

            if (xVal < 0)
            {
                Player.RotateInDegrees(-3.0f);
            }
            else if (xVal > 0)
            {
                Player.RotateInDegrees(3.0f);
            }

            if (yVal > 0)
            {
                Player.Position += (Player.Heading * playerSpeed);
            }
            else if (yVal < 0)
            {
                Player.Position -= (Player.Heading * playerSpeed);
            }

            #region CONTROLLER CHECKS FOR DEBUG OUTPUT
            if (controllerState.IsButtonDown(Buttons.LeftStick))
            {
                debugOutputGP = true;
                debugStringBuilder.Clear();
                debugStringBuilder.AppendFormat("Pos: {0}\nDir: {1}",
                    Player.Position,
                    Player.Heading
                );

                Vector2 offset = new Vector2(Player.BoundingBox.Width + 10, -Player.BoundingBox.Height + 10) / 2.0f;
                if (Player.Position.X > SensorsGame.ScreenCenter.X)
                {
                    Vector2 stringSize = SensorsGame.DebugFont.MeasureString(debugStringBuilder);
                    offset.X -= stringSize.X + Player.BoundingBox.Width + 10;
                }

                debugLoc = Player.Position + offset;
            }
            else if (controllerState.IsButtonUp(Buttons.LeftStick))
            {
                debugOutputGP = false;
            }
            #endregion
        }

        public void Update(GameTime gameTime)
        {
            checkKeyboard();
            checkController();
            Player.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Player.Draw(spriteBatch);

            #region DEBUG OUTPUT
            if (debugOutputKB || debugOutputGP)
            {
                spriteBatch.DrawString(SensorsGame.DebugFont, debugStringBuilder,
                    debugLoc, Color.Yellow);
            }
            #endregion
        }
    }
}
