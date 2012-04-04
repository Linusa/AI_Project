namespace AIFGP_Game
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    using AIFGP_Game_Data;

    public class PlayerManager : IUpdateable, IDrawable
    {
        public BaseGameEntity Player;

        private Vector2 oldVertical = Vector2.Zero;
        private Vector2 oldHorizontal = Vector2.Zero;

        public PlayerManager(PlayerDescription playerDescription)
        {
            Player = new Rabbit(TextureManager.RabbitSpriteSheet, Vector2.Zero);
            EntityManager.Instance.PlayerID = Player.ID;
            
            Vector2 playerPos = AStarGame.GameMap.TilePosToWorldPos(playerDescription.StartingTilePosition);
            Player.Position = playerPos;

            Player.MaxSpeed = playerDescription.MaxSpeed;
        }

        private void checkKeyboard(GameTime gameTime)
        {
            Vector2 vertical = Vector2.Zero;
            Vector2 horizontal = Vector2.Zero;

            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Up))
            {
                vertical.Y = -Player.MaxSpeed;
                Player.EntitySprite.ActiveAnimation = (byte)Rabbit.AnimationIds.HopBack;
            }
            
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                vertical.Y = Player.MaxSpeed;
                Player.EntitySprite.ActiveAnimation = (byte)Rabbit.AnimationIds.HopForward;
            }

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                horizontal.X = -Player.MaxSpeed;
                Player.EntitySprite.ActiveAnimation = (byte)Rabbit.AnimationIds.HopLeft;
            }
            
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                horizontal.X = Player.MaxSpeed;
                Player.EntitySprite.ActiveAnimation = (byte)Rabbit.AnimationIds.HopRight;
            }

            Player.Velocity = vertical + horizontal;

            if (Player.Velocity.Equals(Vector2.Zero))
            {
                bool wasMovingUp = oldVertical.Y < 0.0f;
                bool wasMovingDown = oldVertical.Y > 0.0f;
                bool wasMovingLeft = oldHorizontal.X < 0.0f;
                bool wasMovingRight = oldHorizontal.X > 0.0f;

                if (wasMovingUp)
                    Player.EntitySprite.ActiveAnimation = (byte)Rabbit.AnimationIds.LookBack;
                else if (wasMovingDown)
                    Player.EntitySprite.ActiveAnimation = (byte)Rabbit.AnimationIds.LookForward;
                
                if (wasMovingLeft)
                    Player.EntitySprite.ActiveAnimation = (byte)Rabbit.AnimationIds.LookLeft;
                else if (wasMovingRight)
                    Player.EntitySprite.ActiveAnimation = (byte)Rabbit.AnimationIds.LookRight;
            }
            
            oldVertical = vertical;
            oldHorizontal = horizontal;
        }

        public void Update(GameTime gameTime)
        {
            checkKeyboard(gameTime);
            Player.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Player.Draw(spriteBatch);
        }
    }
}
