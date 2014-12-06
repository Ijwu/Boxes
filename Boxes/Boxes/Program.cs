using System;

namespace Boxes
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Boxes game = new Boxes())
            {
                game.Run();
            }
        }
    }
#endif
}

