using MonopolyServer.Board.Enums;
using MonopolyServer.Board.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonopolyServer.Board.Tiles
{
    class Train : Tile, ITile
    {
        public Guid Owner { get; set; }
        public NeighbourHoodType Neighbourhood { get; set; }
        public int Price { get; set; }
        public int Rent { get; set; }
        public Train(int index, string name, int price, int rent, char marker)
            : base(index, name, marker)
        {
            this.Neighbourhood = NeighbourHoodType.Station;
            this.Price = price;
            this.Rent = rent;
            this.Owner = Guid.Empty;
        }
        public override string ActOnPlayer(Player player)
        {
            if (this.Owner == player.IDPlayer)
            {
                return string.Format("Tuto nemovitost již vlastníš.");
            }
            else if (this.Owner == Guid.Empty)
            {
                return string.Format("Tato nemovitost je na prodej.");
            }
            else
            {
                //zjistim kolik hrac ma koupeno vlaku
                int trains = 0;
                Lobby lobby = Server.MonopolyServer.FindLobby(player.IDLobby);
                Board board = Server.MonopolyServer.FindBoard(lobby.IDLobby);
                for (int i = 0; i < lobby.players.Length; i++)
                {
                    if (lobby.players[i] != null)
                    {
                        for (int j = 0; j < board.allTiles.Count; j++)
                        {
                            if (board.allTiles[j] is Train)
                            {
                                if (((Train)board.allTiles[j]).Owner == lobby.players[i].IDPlayer)
                                {
                                    trains++;
                                }
                            }
                        }
                    }
                }
                player.DecrementMoney(this.Rent*(int)Math.Pow(2, trains - 1));
                for (int i = 0; i < lobby.players.Length; i++)
                {
                    if (lobby.players[i] != null)
                    {
                        if (lobby.players[i].IDPlayer == this.Owner)
                        {
                            lobby.players[i].IncrementMoney(this.Rent*(int)Math.Pow(2, trains - 1));
                        }
                    }
                }
                return string.Format("{0} vlastní hráč {1}. Zaplatíš mu {2}$.", this.Name, player.Nick, (this.Rent * (int)Math.Pow(2, trains - 1)));
            }
        }
    }
}
