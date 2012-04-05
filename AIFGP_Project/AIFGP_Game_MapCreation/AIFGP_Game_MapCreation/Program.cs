using System;

namespace AIFGP_Game_MapCreation
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (MapCreatorGame game = new MapCreatorGame())
            {
                game.Run();
            }
        }
    }
#endif
}

