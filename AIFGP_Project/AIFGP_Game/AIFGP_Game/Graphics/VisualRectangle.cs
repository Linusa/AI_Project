namespace AIFGP_Game
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class VisualRectangle : IDrawable
    {
        private Rectangle rectangle;

        private Line topLine;
        private Line bottomLine;
        private Line leftLine;
        private Line rightLine;

        private Sprite<byte> backgroundRectSprite;

        public VisualRectangle(Rectangle rect)
        {
            Rectangle = rect;
        }

        public Rectangle Rectangle
        {
            get { return rectangle; }
            set
            {
                rectangle = value;

                Vector2 topCenterPt = new Vector2(rectangle.X + rectangle.Width / 2, rectangle.Y);
                Vector2 bottomCenterPt = topCenterPt + new Vector2(0.0f, rectangle.Height);
                int lengthTopAndBottom = rectangle.Width;
    
                Vector2 leftCenterPt = new Vector2(rectangle.X, rectangle.Y + rectangle.Height / 2);
                Vector2 rightCenterPt = leftCenterPt + new Vector2(rectangle.Width, 0.0f);
                int lengthLeftAndRight = rectangle.Height;
    
                // Not good for GC if dealing with lots of VisualRectangle instances.
                topLine = new Line(topCenterPt, lengthTopAndBottom, Color.Gray);
                bottomLine = new Line(bottomCenterPt, lengthTopAndBottom, Color.Gray);
                leftLine = new Line(leftCenterPt, lengthLeftAndRight, Color.Gray);
                rightLine = new Line(rightCenterPt, lengthLeftAndRight, Color.Gray);

                leftLine.RotateInDegrees(90.0f);
                rightLine.RotateInDegrees(90.0f);

                backgroundRectSprite = new Sprite<byte>(TextureManager.SingleWhitePixel,
                    Vector2.Zero, rectangle);
                backgroundRectSprite.CenterPosition = topCenterPt + new Vector2(0.0f, rectangle.Height / 2);
                backgroundRectSprite.AddAnimationFrame(0, rectangle);
                backgroundRectSprite.ActiveAnimation = 0;
            }
        }

        public Color EdgeColor
        {
            set
            {
                topLine.LineColor = value;
                bottomLine.LineColor = value;
                leftLine.LineColor = value;
                rightLine.LineColor = value;
            }
        }

        public Color FillColor
        {
            set { backgroundRectSprite.Color = value; }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            backgroundRectSprite.Draw(spriteBatch);

            topLine.Draw(spriteBatch);
            bottomLine.Draw(spriteBatch);
            leftLine.Draw(spriteBatch);
            rightLine.Draw(spriteBatch);
        }
    }
}
