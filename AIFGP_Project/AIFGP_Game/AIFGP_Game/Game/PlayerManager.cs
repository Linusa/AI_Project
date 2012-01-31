namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
        private StringBuilder debugStringBuilder = new StringBuilder();
        // END DEBUG MEMBERS

        public PlayerManager(Texture2D texture)
        {
            Player = new SimpleGameEntity(texture, SensorsGame.ScreenCenter);
        }

        private void handleInput()
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

            // DEBUG KEY CHECKS
            if (keyboardState.IsKeyDown(Keys.LeftAlt))
            {
                debugOutput = true;
                debugStringBuilder.Clear();
                debugStringBuilder.AppendFormat("Pos: {0}\nDir: {1}",
                    Player.Position,
                    Player.Heading
                );
            }
            else if (keyboardState.IsKeyUp(Keys.LeftAlt))
            {
                if (debugOutput)
                {
                    debugOutput = false;
                }
            }
            // END DEBUG KEY CHECKS
        }

        public void Update(GameTime gameTime)
        {
            handleInput();
            Player.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Player.Draw(spriteBatch);

            // DEBUG OUTPUT
            if (debugOutput)
            {
                Vector2 offset = new Vector2(Player.BoundingBox.Width + 10, -Player.BoundingBox.Height + 10) / 2.0f;
                if (Player.Position.X > SensorsGame.ScreenCenter.X)
                {
                    Vector2 stringSize = SensorsGame.DebugFont.MeasureString(debugStringBuilder);
                    offset.X -= stringSize.X + Player.BoundingBox.Width + 10;
                }

                spriteBatch.DrawString(SensorsGame.DebugFont, debugStringBuilder.ToString(),
                    Player.Position + offset, Color.Yellow);
            }
            // END DEBUG OUTPUT
        }
    }
}
