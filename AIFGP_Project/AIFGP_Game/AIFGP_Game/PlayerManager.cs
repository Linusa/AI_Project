﻿namespace AIFGP_Game
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PlayerManager : IUpdateable, IDrawable
    {
        public IGameEntity Player;

        private BaseGameEntityDebugger playerDebugger;

        public PlayerManager()
        {
            Player = new SimpleSensingGameEntity(AStarGame.PlayerSpriteSheet, AStarGame.ScreenCenter);
            EntityManager.Instance.PlayerID = Player.ID;

            playerDebugger = new BaseGameEntityDebugger(Player as BaseGameEntity);
        }

        private void checkKeyboard(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            bool noVelocityChange = false;

            if (keyboardState.IsKeyDown(Keys.Up))
                Player.Velocity = Player.Heading * Player.MaxSpeed;
            else if (keyboardState.IsKeyDown(Keys.Down))
                Player.Velocity = -Player.Heading * Player.MaxSpeed;
            else
                noVelocityChange = true;
            
            if (keyboardState.IsKeyDown(Keys.Left))
                Player.RotateInDegrees(-3.0f);
            else if (keyboardState.IsKeyDown(Keys.Right))
                Player.RotateInDegrees(3.0f);

            if (noVelocityChange)
                Player.Velocity = Vector2.Zero;

            /*
            // Press 'Left Alt' for player debugging.
            if (keyboardState.IsKeyDown(Keys.LeftAlt))
                playerDebugger.IsDebuggingEnabled = true;
            else if (keyboardState.IsKeyUp(Keys.LeftAlt))
                playerDebugger.IsDebuggingEnabled = false;
            */
        }

        public void Update(GameTime gameTime)
        {
            checkKeyboard(gameTime);

            Player.Update(gameTime);
            AStarGame.WrapPosition(ref Player);

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
