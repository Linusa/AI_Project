namespace AIFGP_Game_MapCreation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using AIFGP_Game;

    public class MapCreatorGui : AIFGP_Game.IUpdateable, AIFGP_Game.IDrawable
    {
        public enum Button
        {
            Grass,
            Wall,
            Bush,
            Player_Position,
            Save
        }

        public bool ButtonJustPressed = false;

        private class ButtonInfo
        {
            public string Name;
            public Rectangle Rect;
            public VisualRectangle VisRect;
        }

        private Button toggledButton = Button.Grass;

        private Dictionary<Button, ButtonInfo> buttonInfoDict =
            new Dictionary<Button, ButtonInfo>();

        private Rectangle lastBtnRectAdded = new Rectangle();

        private const int buttonPadX = 10;
        private const int buttonPadY = 10;

        private const int buttonTextSpacingX = 3;
        private const int buttonTextSpacingY = 3;

        private SpriteFont guiFont = FontManager.DebugFont;

        private float timeSinceLastClick = 0.0f;
        private float minTimeBetweenClicks = 0.25f;

        public MapCreatorGui()
        {
            addButton(Button.Grass, "Grass");
            addButton(Button.Wall, "Wall");
            addButton(Button.Bush, "Bush");
            addButton(Button.Player_Position, "Player Position");
            addButton(Button.Save, "Save");

            toggleButton(toggledButton);
        }

        public Button ToggledButton
        {
            get { return toggledButton; }
        }

        public void Update(GameTime gameTime)
        {
            timeSinceLastClick += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (minTimeBetweenClicks < timeSinceLastClick)
            {
                foreach (Button button in buttonInfoDict.Keys)
                {
                    if (buttonClicked(button))
                    {
                        ButtonJustPressed = true;
                        timeSinceLastClick = 0.0f;

                        if (button != toggledButton)
                        {
                            System.Diagnostics.Debug.WriteLine("Button" + button + " clicked!");
                            toggleButton(button);
                            break;
                        }
                        else
                            break;
                    }
                    else
                        ButtonJustPressed = false;
                }
            }
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

        private bool buttonClicked(Button button)
        {
            bool clicked = false;

            MouseState mouse = Mouse.GetState();
            if (mouse.LeftButton == ButtonState.Pressed)
                clicked = buttonInfoDict[button].Rect.Contains(mouse.X, mouse.Y);

            return clicked;
        }

        private void toggleButton(Button button)
        {
            buttonInfoDict[toggledButton].VisRect.FillColor = Color.PaleGoldenrod;
            buttonInfoDict[toggledButton].VisRect.EdgeColor = Color.LightGray;

            buttonInfoDict[button].VisRect.FillColor = Color.SteelBlue;
            buttonInfoDict[button].VisRect.EdgeColor = Color.LightBlue;

            toggledButton = button;
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
