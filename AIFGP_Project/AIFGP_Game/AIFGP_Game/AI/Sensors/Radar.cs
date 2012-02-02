namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class Radar : ISensor
    {
        private Sprite<byte> radarSprite;
        private static Rectangle dimensions = new Rectangle(0, 0, 250, 250);

        private IGameEntity sensingEntity;

        private Vector2 scaleUp = new Vector2(0.025f);
        private Vector2 scaleMax = new Vector2(1.0f);
        private Timer scaleUpTimer = new Timer(0.01f);

        private bool enabled = false;

        public Radar(IGameEntity entity)
        {
            sensingEntity = entity;

            radarSprite = new Sprite<byte>(SensorsGame.RadarCircle, Vector2.Zero, dimensions);
            radarSprite.AddAnimationFrame(0, dimensions);
            radarSprite.ActiveAnimation = 0;

            resetScaleUp();
        }

        public bool IsSensingEnabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public Vector2 Position
        {
            get { return radarSprite.CenterPosition; }
            set { radarSprite.CenterPosition = value; }
        }

        public void Translate(Vector2 offset)
        {
            Position += offset;
        }

        public void Translate(int x, int y)
        {
            Vector2 offset = new Vector2(x, y);
            Position += offset;
        }

        public void RotateInRadians(float radians)
        {
            radarSprite.RotateInRadians(radians);
        }

        public void RotateInDegrees(float degrees)
        {
            RotateInRadians(MathHelper.ToRadians(degrees));
        }

        public void Scale(Vector2 scale)
        {
            radarSprite.Scale(scale);
        }

        public void Scale(float scale)
        {
            radarSprite.Scale(scale);
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.D2))
            {
                IsSensingEnabled = true;
            }
            else if (keyboardState.IsKeyDown(Keys.D0))
            {
                IsSensingEnabled = false;
                resetScaleUp();
            }

            if (IsSensingEnabled)
            {
                Position = sensingEntity.Position;
                updateRadarScaleUp(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsSensingEnabled)
            {
                radarSprite.Draw(spriteBatch);
            }
        }

        private void updateRadarScaleUp(GameTime gameTime)
        {
            if (scaleUp.X >= scaleMax.X && scaleUp.Y >= scaleMax.Y)
            {
                scaleUp = scaleMax;
            }

            if (scaleUpTimer.Expired(gameTime))
            {
                scaleUp += new Vector2(0.04f);
                radarSprite.Scale(scaleUp);
            }
        }

        private void resetScaleUp()
        {
            scaleUp = Vector2.Zero;
            radarSprite.Scale(scaleUp);
            scaleUpTimer.Restart();
        }
    }
}
