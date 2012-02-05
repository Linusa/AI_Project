namespace AIFGP_Game
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// This class provides a timing mechanism based on
    /// repeated <c>Microsoft.Xna.Framework.GameTime</c> readings.
    /// This class contains some repeated code, but as the
    /// methods within are going to be called numerous times,
    /// I want it to be as fast as possible.
    /// </summary>
    public class Timer
    {
        private float timeout = 0.0f;
        private float time = 0.0f;
        private bool active = false;

        /// <summary>
        /// Construct a <c>Timer</c> with a timeout interval equal
        /// to <paramref name="expireTime"/>. The Timer instance will
        /// not be active until <see cref="Start"/> (or <see cref="Restart()"/>)
        /// has been called.
        /// </summary>
        /// <param name="expireTime">The timeout interval.</param>
        public Timer(float expireTime)
        {
            timeout = expireTime;
        }

        /// <summary>
        /// The <c>Timeout</c> property represents the <c>Timer</c>'s
        /// timeout interval.
        /// </summary>
        public float Timeout
        {
            get { return timeout; }
            set { timeout = value; }
        }

        /// <summary>
        /// The <c>Active</c> property represents whether or not the
        /// <c>Timer</c> is currently active. An active <c>Timer</c>
        /// will update itself when a <c>Microsoft.Xna.Framework.GameTime</c>
        /// is passed in via <see cref="Expired(GameTime)"/>.
        /// </summary>
        public bool Active
        {
            get { return active; }
        }

        /// <summary>
        /// The <c>Start</c> method activates the <c>Timer</c>.
        /// <seealso cref="Stop"/>
        /// </summary>
        public void Start()
        {
            active = true;

            if (time >= timeout)
                time = 0.0f;
        }

        /// <summary>
        /// The <c>Restart</c> method resets and activates the <c>Timer</c>.
        /// <seealso cref="Restart(float)"/>
        /// </summary>
        public void Restart()
        {
            active = true;
            time = 0.0f;
        }

        /// <summary>
        /// The <c>Restart</c> method resets and activates the <c>Timer</c>
        /// with a new expiration time equal to <paramref name="expireTime"/>.
        /// <seealso cref="Restart()"/>
        /// </summary>
        /// <param name="expireTime">The new expiration time.</param>
        public void Restart(float expireTime)
        {
            active = true;
            time = 0.0f;
            timeout = expireTime;
        }

        /// <summary>
        /// The <c>Stop</c> method deactivates the <c>Timer</c>.
        /// <seealso cref="Start"/>
        /// </summary>
        public void Stop()
        {
            active = false;
        }

        /// <summary>
        /// The <c>Expired</c> method updates the <c>Timer</c> using
        /// <paramref name="gameTime"/> and notifies the caller whether
        /// or not the <c>Timer</c> has expired.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <returns>The <c>Timer</c>'s expiration status.</returns>
        public bool Expired(GameTime gameTime)
        {
            bool timerExpired;

            if (!Active)
                timerExpired = false;
            else
            {
                time += (float)gameTime.ElapsedGameTime.TotalSeconds;
                timerExpired = time >= timeout;

                if (timerExpired)
                    time = 0.0f;
            }

            return timerExpired;
        }
    }
}