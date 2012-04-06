namespace AIFGP_Game_MapCreation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using AIFGP_Game;

    public class MapCreatorGui : AIFGP_Game.IUpdateable, AIFGP_Game.IDrawable
    {
        private enum Button
        {
            Grass,
            Wall,
            Bush,
            Save
        }

        private class ButtonInfo
        {
            public string Name;
            public Rectangle Rect;
            public VisualRectangle VisRect;
        }

        private Dictionary<Button, ButtonInfo> buttonInfoDict =
            new Dictionary<Button, ButtonInfo>();

        private Rectangle lastBtnRectAdded = new Rectangle();

        private const int buttonPadX = 10;
        private const int buttonPadY = 10;

        private const int buttonTextSpacingX = 3;
        private const int buttonTextSpacingY = 3;

        private SpriteFont guiFont = FontManager.DebugFont;

        public MapCreatorGui()
        {
            addButton(Button.Grass, "Grass");
            addButton(Button.Wall, "Wall");
            addButton(Button.Bush, "Bush");
            addButton(Button.Save, "Save");
        }

        public void Update(GameTime gameTime)
        {
            // handle mouse stuff
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Button button in buttonInfoDict.Keys)
            {
                string name = buttonInfoDict[button].Name;
                
                int posX = buttonInfoDict[button].Rect.X;
                int posY = buttonInfoDict[button].Rect.Y;
                Vector2 pos = new Vector2(posX, posY);

                buttonInfoDict[button].VisRect.Draw(spriteBatch);
                spriteBatch.DrawString(guiFont, name, pos, Color.Coral);
            }
        }

        private void addButton(Button button, string buttonName)
        {
            buttonInfoDict.Add(button, new ButtonInfo());
            buttonInfoDict[button].Name = buttonName;
            buttonInfoDict[button].Rect = computeButtonRectangle(buttonName);
            buttonInfoDict[button].VisRect = new VisualRectangle(buttonInfoDict[button].Rect);
            buttonInfoDict[button].VisRect.FillColor = Color.PaleGoldenrod;
            buttonInfoDict[button].VisRect.EdgeColor = Color.LightGray;
        }

        private Rectangle computeButtonRectangle(string buttonName)
        {
            Vector2 strDimensions = guiFont.MeasureString(buttonName);

            lastBtnRectAdded.X += lastBtnRectAdded.Width + buttonPadX;
            lastBtnRectAdded.Y = buttonPadY;

            Rectangle rect = new Rectangle(lastBtnRectAdded.X, lastBtnRectAdded.Y,
                (int)strDimensions.X + buttonTextSpacingX, (int)strDimensions.Y + buttonTextSpacingY);

            lastBtnRectAdded = rect;
            return rect;
        }
    }
}
