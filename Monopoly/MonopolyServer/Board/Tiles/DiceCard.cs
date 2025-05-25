using MonopolyServer.Board.Enums;
using MonopolyServer.Board.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonopolyServer.Board.Tiles
{
    class DiceCard : Tile, ITile
    {
        public Guid Owner { get; set; }
        public NeighbourHoodType Neighbourhood { get; set; }
        public int Price { get; set; }
        public int Rent { get; set; }
        public DiceCard(int index, string name, int price, char marker)
            : base(index, name, marker)
        {
            this.Neighbourhood = NeighbourHoodType.Utility;
            this.Price = price;
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
                //rozdelit na radio a na ostatni
                Lobby lobby = Server.MonopolyServer.FindLobby(player.IDLobby);
                Board board = Server.MonopolyServer.FindBoard(lobby.IDLobby);
                DiceCard currentTile = (DiceCard)board.allTiles[player.CurrentPosition];
                int toPay = 0;
                if (currentTile.Index == 18 || currentTile.Index == 32)
                {
                    //radio, zjistit kolik radii hrac vlastni
                    if (((DiceCard)board.allTiles[18]).Owner == ((DiceCard)board.allTiles[32]).Owner)
                    {
                        toPay = (player.blackDiceNumber + player.whiteDiceNumber) * 50;
                        player.DecrementMoney(toPay);
                    }
                    else
                    {
                        toPay = 50;
                        player.DecrementMoney(toPay);
                    }
                    Server.MonopolyServer.FindPlayerByHisID(this.Owner).IncrementMoney(toPay);
                    return string.Format("{0} vlastní hráč {1}. Za poslech mu zaplatíš {2}", this.Name, player.Nick, toPay);
                }
                else
                {
                    int countCards = 0;
                    if (this.Owner == ((DiceCard)board.allTiles[8]).Owner)
                        countCards++;
                    if (this.Owner == ((DiceCard)board.allTiles[12]).Owner)
                        countCards++;
                    if (this.Owner == ((DiceCard)board.allTiles[28]).Owner)
                        countCards++;

                    if (countCards == 1)
                        toPay = 5 * (player.whiteDiceNumber + player.blackDiceNumber);
                    if (countCards == 2)
                        toPay = 10 * (player.whiteDiceNumber + player.blackDiceNumber);
                    if (countCards == 3)
                        toPay = 20 * (player.whiteDiceNumber + player.blackDiceNumber);

                    player.DecrementMoney(toPay);
                    Server.MonopolyServer.FindPlayerByHisID(this.Owner).IncrementMoney(toPay);
                    return string.Format("{0} vlastní hráč {1}. Zaplatíš mu {2}", this.Name, player.Nick, toPay);
                }
            }
        }
    }
}
