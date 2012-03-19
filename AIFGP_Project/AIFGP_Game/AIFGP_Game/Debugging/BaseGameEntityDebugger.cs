namespace AIFGP_Game
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Debugging class that can show some basic info about a
    /// game entity.
    /// </summary>
    public class BaseGameEntityDebugger : IUpdateable, IDrawable
    {
        private PropertyDebugger<BaseGameEntity> debugger;

        public BaseGameEntityDebugger(BaseGameEntity entity)
        {
            debugger = new PropertyDebugger<BaseGameEntity>(entity);

            debugger.RegisterProperty("ID");
            debugger.RegisterProperty("Position");
            debugger.RegisterProperty("Heading");
        }

        public bool IsDebuggingEnabled
        {
            get { return debugger.DebuggingEnabled; }
            set { debugger.DebuggingEnabled = value; }
        }

        public void Update(GameTime gameTime)
        {
            if (IsDebuggingEnabled)
            {
                updateDebugLocation();
                debugger.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsDebuggingEnabled)
            {
                debugger.Draw(spriteBatch);
            }
        }

        private void updateDebugLocation()
        {
            debugger.DebugLocation = debugger.ObjectDebugging.Position;

            Vector2 offset = new Vector2(debugger.ObjectDebugging.BoundingBox.Width + 10,
                -debugger.ObjectDebugging.BoundingBox.Height + 10) / 2.0f;

            if (debugger.ObjectDebugging.Position.X > AStarGame.ScreenCenter.X)
            {
                Vector2 stringSize = AStarGame.DebugFont.MeasureString(debugger.DebugText);
                offset.X -= stringSize.X + debugger.ObjectDebugging.BoundingBox.Width + 10;
            }

            debugger.DebugLocation = debugger.ObjectDebugging.Position + offset;
        }
    }
}
