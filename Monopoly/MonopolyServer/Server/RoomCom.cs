using System;
using System.Collections.Generic;
using System.Text;

namespace MonopolyServer.Server
{
    public partial class MonopolyServer
    {
        private static void updatePlayerInRoom(Player player)
        {
            for (int i = 0; i < client.Room.Players.Count; i++)
            {
                if (player.IDPlayer == client.Room.Players[i].IDPlayer)
                {
                    client.Room.Players[i] = player;
                    return;
                }
            }
        }
    }
}
