namespace AIFGP_Game
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public static class TextureManager
    {
        public static Texture2D GrassTile;
        public static Texture2D WallTile;
        public static Texture2D BushTile;
        public static Texture2D RadarCircle;
        public static Texture2D SingleWhitePixel;
        public static Texture2D RabbitSpriteSheet;
        public static Texture2D CarrotSprite;
        public static Texture2D FarmerSprite;

        public static void LoadTextures(Game game)
        {
            GrassTile = game.Content.Load<Texture2D>(@"Images\Grass");
            WallTile = game.Content.Load<Texture2D>(@"Images\Hedge");
            BushTile = game.Content.Load<Texture2D>(@"Images\Bush");
            RadarCircle = game.Content.Load<Texture2D>(@"Images\circle");
            RabbitSpriteSheet = game.Content.Load<Texture2D>(@"Images\rabbit_brown");
            CarrotSprite = game.Content.Load<Texture2D>(@"Images\carrot");
            FarmerSprite = game.Content.Load<Texture2D>(@"Images\farmerbrown_fitted");

            SingleWhitePixel = new Texture2D(game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            SingleWhitePixel.SetData<Color>(new Color[1] { Color.White }); 
        }
    }
}
