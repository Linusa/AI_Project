
namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    using GraphType = Graph<PositionalNode, Edge>;

    public class GraphSearchViewer : GraphViewer
    {
        public GraphSearchViewer(GraphType graph)
            : base(graph)
        {
            // Do nothing! :D
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            bool leftAlt = keyboardState.IsKeyDown(Keys.LeftAlt);
            bool rightAlt = keyboardState.IsKeyDown(Keys.RightAlt);

            if (leftAlt && !rightAlt)
            {
            }
            else if (!leftAlt && rightAlt)
            {
            }
        }
    }
}
