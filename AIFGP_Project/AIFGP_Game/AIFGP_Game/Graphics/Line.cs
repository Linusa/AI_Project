namespace AIFGP_Game
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Line : ISpatialEntity, IDrawable
    {
        private Sprite<byte> lineSprite;
        private int defaultLineHeight = 3;

        public Line(Vector2 position, int width, Color color)
        {
            Rectangle dimensions = new Rectangle(0, 0, width, defaultLineHeight);

            lineSprite = new Sprite<byte>(SensorsGame.SingleWhitePixel,
                position, dimensions);

            lineSprite.Color = color;
            lineSprite.AddAnimationFrame(0, dimensions);
            lineSprite.ActiveAnimation = 0;
        }

        public Vector2 Position
        {
            get { return lineSprite.CenterPosition; }
            set { lineSprite.CenterPosition = value; }
        }

        public void Translate(Vector2 offset)
        {
            Position += offset;
        }

        public void Translate(int x, int y)
        {
            Vector2 offset = new Vector2(x, y);
            Position += offset;
        }

        public void RotateInRadians(float radians)
        {
            lineSprite.RotateInRadians(radians);
        }

        public void RotateInDegrees(float degrees)
        {
            RotateInRadians(MathHelper.ToRadians(degrees));
        }

        public void Scale(float scale)
        {
            lineSprite.Scale(scale);
        }

        public void Scale(Vector2 scale)
        {
            lineSprite.Scale(scale);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            lineSprite.Draw(spriteBatch);
        }
    }
}
