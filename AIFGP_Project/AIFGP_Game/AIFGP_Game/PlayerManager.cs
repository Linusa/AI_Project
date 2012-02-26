namespace AIFGP_Game
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PlayerManager : IUpdateable, IDrawable
    {
        public IGameEntity Player;
        private float playerSpeed = 250.0f;

        BaseGameEntityDebugger playerDebugger;

        public PlayerManager()
        {
            Player = new SimpleSensingGameEntity(SensorsGame.PlayerSpriteSheet, SensorsGame.ScreenCenter);

            playerDebugger = new BaseGameEntityDebugger(Player as BaseGameEntity);
        }

        private void checkKeyboard(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Up))
                Player.Position += (Player.Heading * playerSpeed * dt);

            if (keyboardState.IsKeyDown(Keys.Down))
                Player.Position -= (Player.Heading * playerSpeed * dt);

            if (keyboardState.IsKeyDown(Keys.Left))
                Player.RotateInDegrees(-3.0f);

            if (keyboardState.IsKeyDown(Keys.Right))
                Player.RotateInDegrees(3.0f);

            // Press 'Left Alt' for player debugging.
            if (keyboardState.IsKeyDown(Keys.LeftAlt))
                playerDebugger.IsDebuggingEnabled = true;
            else if (keyboardState.IsKeyUp(Keys.LeftAlt))
                playerDebugger.IsDebuggingEnabled = false;
        }

        public void Update(GameTime gameTime)
        {
            checkKeyboard(gameTime);
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
