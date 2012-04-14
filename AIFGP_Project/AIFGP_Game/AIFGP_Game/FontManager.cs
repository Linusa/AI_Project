namespace AIFGP_Game
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public static class FontManager
    {
        public static SpriteFont BigFont;
        public static SpriteFont DebugFont;
        public static SpriteFont SmallDebugFont;

        public static void LoadFonts(Game game)
        {
            BigFont = game.Content.Load<SpriteFont>(@"Fonts\LindseyBig");
            DebugFont = game.Content.Load<SpriteFont>(@"Fonts\Debug");
            SmallDebugFont = game.Content.Load<SpriteFont>(@"Fonts\Debug_Small");
        }
    }
}
