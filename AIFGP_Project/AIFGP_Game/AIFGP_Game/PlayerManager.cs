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
        private bool debugOutput = false;
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
                debugOutput = true;
                debugStringBuilder.Clear();
                debugStringBuilder.AppendFormat("Player: {0}...\nPos: {1}\nDir: {2}\n\n",
                    Player.ID.ToString().Substring(0, 8),
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
                debugOutput = false;
            }
            #endregion
        }

        public void Update(GameTime gameTime)
        {
            checkKeyboard();
            Player.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Player.Draw(spriteBatch);

            #region DEBUG OUTPUT
            if (debugOutput)
            {
                spriteBatch.DrawString(SensorsGame.DebugFont, debugStringBuilder,
                    debugLoc, Color.Yellow);
            }
            #endregion
        }
    }
}
