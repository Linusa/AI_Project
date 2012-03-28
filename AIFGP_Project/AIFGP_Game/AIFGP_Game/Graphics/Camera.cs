namespace AIFGP_Game
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Camera is a scrolling camera implementation that supports
    /// magnification and rotation. Provides a view transformation
    /// based on position and viewport size, so this class could
    /// easily be used for split-screen games.
    /// </summary>
    public class Camera
    {
        private Vector2 position;

        private float rotation = 0.0f;
        private float magnification = 1.0f;

        private GraphicsDevice gfxDev;

        public Camera(GraphicsDevice graphicsDevice, Vector2 position)
        {
            Position = position;
            gfxDev = graphicsDevice;
        }

        public GraphicsDevice GraphicsDevice
        {
            get { return gfxDev; }
        }

        /// <summary>
        /// Two-dimensional view transformation that represents a viewing
        /// space centered at the camera position. The size of the viewing
        /// space is the size of the viewport associated with the camera
        /// instance's GraphicsDevice.
        /// </summary>
        public Matrix Transformation
        {
            get
            {
                Vector3 negCameraPos = new Vector3(-position, 0.0f);
                Vector3 toViewportCenter = new Vector3(gfxDev.Viewport.Width, gfxDev.Viewport.Height, 0.0f) * 0.5f;
                Vector3 scaleValues = new Vector3(magnification, magnification, 1.0f);

                Matrix cameraPosTranslation = Matrix.CreateTranslation(negCameraPos);
                Matrix toViewportCenterTranslation = Matrix.CreateTranslation(toViewportCenter);
                Matrix rotationMat = Matrix.CreateRotationZ(rotation);
                Matrix scaleMat = Matrix.CreateScale(scaleValues);

                return cameraPosTranslation * rotationMat * scaleMat * toViewportCenterTranslation;
            }
        }
        
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        
        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }
        
        public float Magnification
        {
            get { return magnification; }
            set { magnification = MathHelper.Max(0.1f, value); }
        }

        public void RotateInDegrees(float degrees)
        {
            RotateInRadians(MathHelper.ToRadians(degrees));
        }

        public void RotateInRadians(float radians)
        {
            while (radians < 0.0f)
                radians += MathHelper.TwoPi;

            rotation = (rotation + radians) % MathHelper.TwoPi;
        }
    }
}
