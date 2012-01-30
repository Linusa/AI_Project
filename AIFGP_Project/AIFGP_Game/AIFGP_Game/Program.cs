using System;

namespace AIFGP_Game
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (SensorsGame game = new SensorsGame())
            {
                game.Run();
            }
        }
    }
#endif
}

