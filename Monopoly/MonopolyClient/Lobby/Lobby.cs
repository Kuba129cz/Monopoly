using Monopoly.MonopolyGame.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monopoly.Lobby
{
    class Lobby
    {
        public string Name { get; set; }
        public Player Master { get; set; }
        public Player[] players = new Player[4];
        public Guid IDLobby { get; set; }
        public bool isInGame { get; set; } = false;
        public bool EndOfBuying { get; set; } = false;
        public Lobby(string name)
        {
            this.Name = name;
            IDLobby = Guid.NewGuid();
        }
        public Player[] GetPlayers()
        {
            return players;
        }
        public bool AddPlayer(Player pl)
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] == null)
                {
                    players[i] = pl;
                    return true;
                }
            }
            return false; //vrati false kdyz uz bylo dosazeno max. kapacity
        }
        public bool RemovePlayer(Player pl)
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].IDPlayer == pl.IDPlayer)
                {
                    players[i] = null;
                    return true;
                }
            }
            return false;
        }
        public int NumberOfPlayers()
        {
            int count = 0;
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] != null)
                {
                    count++;
                }
            }
            return count;
        }

        internal void Coppy(Lobby updatedLobby)
        {
            this.Name = updatedLobby.Name;
            this.Master.CopyPlayer(updatedLobby.Master);
            this.IDLobby = updatedLobby.IDLobby;
            this.isInGame = updatedLobby.isInGame;
            this.EndOfBuying = updatedLobby.EndOfBuying;
            for(int i=0;i<updatedLobby.players.Length;i++)
            {
                if(updatedLobby.players[i]!= null)
                {
                    if (this.players[i] == null)
                        this.players[i] = new Player();
                    this.players[i].CopyPlayer(updatedLobby.players[i]);
                }
            }
    }

        public Player GetMaster()
        {
            Player pl = null;
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] != null)
                {
                    pl = players[i];
                    break;
                }
            }
            if (pl != null)
            {
                for (int i = 0; i < players.Length; i++)
                {
                    if (players[i] != null)
                        if (players[i].TimeInLobby > pl.TimeInLobby)
                        pl = players[i];
                }
                return pl;
            }
            else
                return null;
        }
        public List<Player> GetListOfPlayers()
        {
            List<Player> pls = new List<Player>();
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] != null)
                    pls.Add(players[i]);
            }
            return pls;
        }
        public Player PlayerOnTurn()
        {
            List<Player> listOfPlayers = GetListOfPlayers();
            for(int i=0;i< listOfPlayers.Count;i++)
            {
                if (listOfPlayers[i].IsOnTurn == true)
                    return listOfPlayers[i];
            }
            return null;
        }
    }
}

