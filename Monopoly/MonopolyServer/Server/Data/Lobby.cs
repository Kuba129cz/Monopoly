using System;
using System.Collections.Generic;
using System.Text;

namespace MonopolyServer
{
   public class Lobby
    {
        public string Name { get; set; }
        public Player Master { get; set; }
        public Player[] players = new Player[4];
        public Guid IDLobby { get; set; }
        public bool isInGame { get; set; } = false;
        public bool EndOfBuying { get; set; } = false;
        public int Round { get; set; } = 0;
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
                if (players[i] !=null && players[i].IDPlayer == pl.IDPlayer)
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
        public Player GetMaster()
        {
            Player pl = null;
            for(int i =0; i<players.Length;i++)
            {
                if(players[i]!= null)
                {
                    pl = players[i];
                    break;
                }
            }
            if (pl != null)
            {
                for(int i=0; i< players.Length;i++)
                {
                    if(players[i] != null)
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

        internal void NextPlayerTurn()
        {
            bool assign = false;
            if (GetListOfPlayers().Count > 1)
            {
                int indexTurnPlayer = -1;
                for (int i = 0; i < players.Length-1; i++)
                {
                    if (players[i] != null)
                    {
                        if (players[i].IsOnTurn)
                        {
                            indexTurnPlayer = i;
                        }
                    }
                }
                if(indexTurnPlayer !=-1)
                {
                    for(int i= indexTurnPlayer; i<players.Length-1;i++)
                    {
                        if(players[i+1]!=null)
                        {
                            players[i].IsOnTurn = false;
                            players[i + 1].IsOnTurn = true;
                            assign = true;
                            break;
                        }    
                    }
                    if(!assign)
                    {
                        for(int i=0; i<players.Length;i++)
                        {
                            if(players[i]!=null)
                            {
                                players[indexTurnPlayer].IsOnTurn = false;
                                players[i].IsOnTurn = true;
                                Round++;
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
