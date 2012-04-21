namespace AIFGP_Game
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Carrot
    {
        private Sprite<byte> sprite;

        public Carrot(Texture2D texture, Vector2 position)
        {
            sprite = new Sprite<byte>(texture, Vector2.Zero,
                new Rectangle(0, 0, 36, 36));

            configureSprite();

            Position = position;
        }

        public Vector2 Position
        {
            get { return sprite.CenterPosition; }
            set { sprite.CenterPosition = value; }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
        }

        private void configureSprite()
        {
            sprite.LayerDepth = AStarGame.DrawingOrder.Entities;
            sprite.AddAnimationFrame(0, sprite.Dimensions);
        }
    }
}
