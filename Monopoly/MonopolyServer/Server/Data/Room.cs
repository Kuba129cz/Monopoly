using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MonopolyServer
{
    class Room
    {
        public List<Player> Players = null;
        public ObservableCollection<Lobby> Lobbies = null;
        public Room()
        {
            Players = new List<Player>();
            Lobbies = new ObservableCollection<Lobby>();
        }
        public bool RemoveLobby(Lobby lobby)
        {
            for(int i=0; i<Lobbies.Count;i++)
            {
                if(Lobbies[i].IDLobby == lobby.IDLobby)
                {
                    Lobbies.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }
        public bool RemovePlayer(Player pl)
        {
            for (int i = 0; i < Players.Count; i++)
            {
                if (Players[i].IDLobby != Guid.Empty && Players[i].IDLobby == pl.IDLobby)
                {
                    Players.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }
        public bool RemovePlayerFromLobby(Lobby lb, Player pl)
        {
            return lb.RemovePlayer(pl);
        }
    }
}
