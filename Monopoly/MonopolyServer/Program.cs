using System;
using System.Collections.Generic;
using System.Text;

namespace MonopolyServer
{
    class Program
    {
        public static Server.MonopolyServer MonopolyServer;
        [STAThread]
        static void Main(string[] args)
        {
            MonopolyServer = new Server.MonopolyServer();
            MonopolyServer.Run(args);
        }
    }
}
