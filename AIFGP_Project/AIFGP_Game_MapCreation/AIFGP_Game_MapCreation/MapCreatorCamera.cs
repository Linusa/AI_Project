namespace AIFGP_Game_MapCreation
{
    using AIFGP_Game;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class MapCreatorCamera : Camera, AIFGP_Game.IUpdateable
    {
        private MouseState prevMouseState;

        public MapCreatorCamera(GraphicsDevice graphicsDevice, Vector2 position)
            : base(graphicsDevice, position)
        {
        }

        public void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            if (mouseState.MiddleButton == ButtonState.Pressed)
                Position += new Vector2(prevMouseState.X - mouseState.X, prevMouseState.Y - mouseState.Y);
            
            if (mouseState.ScrollWheelValue != prevMouseState.ScrollWheelValue)
            {
                // magFactor assigned either 1.1 or 0.9 depending on direction of scroll.
                float magFactor = (mouseState.ScrollWheelValue - prevMouseState.ScrollWheelValue) / 1200.0f + 1;
                Magnification *= magFactor;
            }
 
            prevMouseState = mouseState;
        }
    }
}
