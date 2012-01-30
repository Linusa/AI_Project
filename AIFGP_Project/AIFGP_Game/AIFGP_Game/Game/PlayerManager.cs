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
        public SimpleGameEntity Player;
        private float playerSpeed = 4.0f;

        private Timer inputTimer = new Timer(0.25f);

        public PlayerManager(Texture2D texture)
        {
            Player = new SimpleGameEntity(texture, SensorsGame.ScreenCenter);
            inputTimer.Start();
        }

        private void handleInput()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Up))
            {
                //Player.Translate(Player.Heading * playerSpeed);
                Player.Position += (Player.Heading * playerSpeed);
                //SensorsGame.WrapPosition(ref player);
            }

            if (keyboardState.IsKeyDown(Keys.Down))
            {
                //Player.Translate(-Player.Heading * playerSpeed);
                Player.Position -= (Player.Heading * playerSpeed);
                //SensorsGame.WrapPosition(ref player);
            }

            //if (inputTimer.Expired(gameTime))
            //{
                if (keyboardState.IsKeyDown(Keys.Left))
                {
                    Player.RotateInDegrees(-3.0f);
                }

                if (keyboardState.IsKeyDown(Keys.Right))
                {
                    Player.RotateInDegrees(3.0f);
                }
            //}
        }

        public void Update(GameTime gameTime)
        {
            handleInput();

            //player.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Player.Draw(spriteBatch);
        }
    }
}
