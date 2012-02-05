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

        BaseGameEntityDebugger playerDebugger;

        public PlayerManager()
        {
            Player = new SimpleSensingGameEntity(SensorsGame.PlayerSpriteSheet, SensorsGame.ScreenCenter);

            playerDebugger = new BaseGameEntityDebugger(Player as BaseGameEntity);
        }

        private void checkKeyboard()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Up))
                Player.Position += (Player.Heading * playerSpeed);

            if (keyboardState.IsKeyDown(Keys.Down))
                Player.Position -= (Player.Heading * playerSpeed);

            if (keyboardState.IsKeyDown(Keys.Left))
                Player.RotateInDegrees(-3.0f);

            if (keyboardState.IsKeyDown(Keys.Right))
                Player.RotateInDegrees(3.0f);

            #region KEY CHECKS FOR DEBUG OUTPUT
            if (keyboardState.IsKeyDown(Keys.LeftAlt))
            {
                playerDebugger.IsDebuggingEnabled = true;
            }
            else if (keyboardState.IsKeyUp(Keys.LeftAlt))
            {
                playerDebugger.IsDebuggingEnabled = false;
            }
            #endregion
        }

        public void Update(GameTime gameTime)
        {
            checkKeyboard();
            Player.Update(gameTime);
            SensorsGame.WrapPosition(ref Player);

            if (playerDebugger.IsDebuggingEnabled)
                playerDebugger.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Player.Draw(spriteBatch);

            if (playerDebugger.IsDebuggingEnabled)
                playerDebugger.Draw(spriteBatch);
        }
    }
}
