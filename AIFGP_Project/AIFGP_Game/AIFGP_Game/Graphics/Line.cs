namespace AIFGP_Game
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Line : ISpatialEntity, IDrawable
    {
        public Sprite<byte> LineSprite;
        private int defaultLineHeight = 2;

        public Line(Vector2 position, int width, Color color)
        {
            Rectangle dimensions = new Rectangle(0, 0, width, defaultLineHeight);
            LineSprite = new Sprite<byte>(AStarGame.SingleWhitePixel,
                position, dimensions);
            
            LineSprite.Color = color;
            LineSprite.AddAnimationFrame(0, dimensions);
            LineSprite.ActiveAnimation = 0;
        }

        public Color LineColor
        {
            get { return LineSprite.Color; }
            set { LineSprite.Color = value; }
        }

        public Vector2 Position
        {
            get { return LineSprite.CenterPosition; }
            set { LineSprite.CenterPosition = value; }
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
            LineSprite.RotateInRadians(radians);
        }

        public void RotateInDegrees(float degrees)
        {
            RotateInRadians(MathHelper.ToRadians(degrees));
        }

        public void Scale(float scale)
        {
            LineSprite.Scale(scale);
        }

        public void Scale(Vector2 scale)
        {
            LineSprite.Scale(scale);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            LineSprite.Draw(spriteBatch);
        }
    }
}
