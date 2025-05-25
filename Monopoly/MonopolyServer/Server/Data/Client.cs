using MonopolyServer.Server.Data;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace MonopolyServer
{
    class Client
    {
        public List<TcpClient> TcpClients;
        public Room Room { get; set; }
        public Client()
        {
            Room = new Room();
            TcpClients = new List<TcpClient>();
        }
    }
}
