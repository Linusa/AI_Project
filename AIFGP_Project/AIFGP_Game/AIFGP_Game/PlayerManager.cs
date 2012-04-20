namespace AIFGP_Game
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    using AIFGP_Game_Data;

    public class PlayerManager : IUpdateable, IDrawable
    {
        public Rabbit Player;
        private Carrot carrot;

        private Vector2 playerStartPos;

        private Vector2 oldVertical = Vector2.Zero;
        private Vector2 oldHorizontal = Vector2.Zero;

        public PlayerManager(PlayerDescription playerDescription)
        {
            Player = new Rabbit(TextureManager.RabbitSpriteSheet, Vector2.Zero);
            EntityManager.Instance.PlayerID = Player.ID;
            
            playerStartPos = AStarGame.GameMap.TilePosToWorldPos(playerDescription.StartingTilePosition);
            ResetPlayerPosition();

            Player.MaxSpeed = playerDescription.MaxSpeed;

            carrot = new Carrot(TextureManager.CarrotSprite, Vector2.Zero);
            Vector2 carrotPos = AStarGame.GameMap.TilePosToWorldPos(playerDescription.CarrotTilePosition);
            carrot.Position = carrotPos;
        }

        public bool PlayerReachedCarrot
        {
            get { return Vector2.DistanceSquared(Player.Position, carrot.Position) < 200.0f; }
        }

        public void ResetPlayerPosition()
        {
            Player.Position = playerStartPos;
        }

        private void checkKeyboard(GameTime gameTime)
        {
            Vector2 vertical = Vector2.Zero;
            Vector2 horizontal = Vector2.Zero;

            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Up))
            {
                vertical = -Vector2.UnitY;
                Player.EntitySprite.ActiveAnimation = (byte)Rabbit.AnimationIds.HopBack;
                Player.EntitySprite.SpriteEffects = SpriteEffects.None;
            }
            
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                vertical = Vector2.UnitY;
                Player.EntitySprite.ActiveAnimation = (byte)Rabbit.AnimationIds.HopForward;
                Player.EntitySprite.SpriteEffects = SpriteEffects.None;
            }

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                horizontal = -Vector2.UnitX;
                Player.EntitySprite.ActiveAnimation = (byte)Rabbit.AnimationIds.HopLeft;
                Player.EntitySprite.SpriteEffects = SpriteEffects.None;
            }
            
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                horizontal = Vector2.UnitX;
                Player.EntitySprite.ActiveAnimation = (byte)Rabbit.AnimationIds.HopRight;
                Player.EntitySprite.SpriteEffects = SpriteEffects.FlipHorizontally;
            }

            Vector2 unitVel = vertical + horizontal;
            if (unitVel.X != 0.0f && unitVel.Y != 0.0f)
                unitVel.Normalize();

            Player.Velocity = Player.MaxSpeed * unitVel;

            if (Player.Velocity.Equals(Vector2.Zero))
            {
                Player.EntitySprite.SpriteEffects = SpriteEffects.None;

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

            if (AStarGame.GameMap.IsWorldPosBush(Player.Position))
                Player.IsHidden = true;
            else
                Player.IsHidden = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Player.Draw(spriteBatch);
            carrot.Draw(spriteBatch);
        }
    }
}
