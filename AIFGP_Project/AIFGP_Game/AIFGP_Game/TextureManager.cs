namespace AIFGP_Game
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public static class TextureManager
    {
        public static Texture2D PlayerSpriteSheet;
        public static Texture2D NpcSpriteSheet;
        public static Texture2D GrassTile;
        public static Texture2D WallTile;
        public static Texture2D BushTile;
        public static Texture2D RadarCircle;
        public static Texture2D SingleWhitePixel;
        public static Texture2D RabbitSpriteSheet;
        public static Texture2D CarrotSprite;

        public static void LoadTextures(Game game)
        {
            PlayerSpriteSheet = game.Content.Load<Texture2D>(@"Images\player_arrow_with_effects");
            NpcSpriteSheet = game.Content.Load<Texture2D>(@"Images\npc_arrow_with_effects");
            GrassTile = game.Content.Load<Texture2D>(@"Images\grass_tile");
            WallTile = game.Content.Load<Texture2D>(@"Images\wall_tile");
            BushTile = game.Content.Load<Texture2D>(@"Images\bush_tile");
            RadarCircle = game.Content.Load<Texture2D>(@"Images\circle");
            RabbitSpriteSheet = game.Content.Load<Texture2D>(@"Images\rabbit_136x360_ea34x36");
            CarrotSprite = game.Content.Load<Texture2D>(@"Images\carrot");

            SingleWhitePixel = new Texture2D(game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            SingleWhitePixel.SetData<Color>(new Color[1] { Color.White }); 
        }
    }
}
