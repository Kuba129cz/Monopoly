using System;
using System.IO;

namespace Monopoly
{
    class Error
    {
        public static void HandleError(Exception ex)
        {
            using (StreamWriter sw = new StreamWriter(@"error.txt"))
            {
                sw.WriteLine(ex.ToString());
                sw.Flush();
            }
            Program.Game.Exit();
        }
    }
}
