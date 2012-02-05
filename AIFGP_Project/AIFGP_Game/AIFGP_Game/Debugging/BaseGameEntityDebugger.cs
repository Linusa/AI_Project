namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class BaseGameEntityDebugger : IUpdateable, IDrawable
    {
        private TextDebugger<BaseGameEntity> debugger;

        public BaseGameEntityDebugger(BaseGameEntity entity)
        {
            debugger = new TextDebugger<BaseGameEntity>(entity);

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

            if (debugger.ObjectDebugging.Position.X > SensorsGame.ScreenCenter.X)
            {
                Vector2 stringSize = SensorsGame.DebugFont.MeasureString(debugger.DebugText);
                offset.X -= stringSize.X + debugger.ObjectDebugging.BoundingBox.Width + 10;
            }

            debugger.DebugLocation = debugger.ObjectDebugging.Position + offset;
        }
    }
}
