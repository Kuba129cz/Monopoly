using System;

namespace Monopoly
{
    public static class Program
    {
        public static Monopoly Game;
        [STAThread]
        static void Main()
        {
            using (Game = new Monopoly())
                Game.Run();
        }
    }
}
