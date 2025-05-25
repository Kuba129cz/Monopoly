using MonopolyServer.Board.Interfaces;
using System;

namespace MonopolyServer.Board.Tiles
{
    class Tax : Tile, ITile
    {
        public int TaxAmount { get; private set; }

        public Tax(int index, string name, int taxAmount, char marker) : base(index, name, marker)
        {
            this.TaxAmount = taxAmount;
        }

        public override string ActOnPlayer(Player player)
        {
            Lobby lobby = Server.MonopolyServer.FindLobby(player.IDLobby);
            Board board = Server.MonopolyServer.FindBoard(lobby.IDLobby);
            if (board.allTiles[player.CurrentPosition].Index == 4)
            {
                    //spocitat 10% z hotovych penez, z ceny pozemku, domu a hotelu
                    int sum = player.Money;
                    for (int i = 0; i < board.allTiles.Count; i++)
                    {
                        if (board.allTiles[i] is Street)
                        {
                            Street street = (Street)board.allTiles[i];
                            if (street.Owner == player.IDPlayer)
                            {
                                sum += street.Price;
                                for (int j = 0; j < street.Houses.Length; j++)
                                {
                                    if (street.Houses[j])
                                        sum += street.PriceHouse;
                                }
                            }
                        }
                        else
                        if (board.allTiles[i] is Train)
                        {
                            Train train = (Train)board.allTiles[i];
                            if (train.Owner == player.IDPlayer)
                            {
                                sum += train.Price;
                            }
                        }
                        else
                            if (board.allTiles[i] is DiceCard)
                        {
                            DiceCard diceCard = (DiceCard)board.allTiles[i];
                            if (diceCard.Owner == player.IDPlayer)
                            {
                                sum += diceCard.Price;
                            }
                        }
                    }
                    int tenProcent = (int)(0.1 * sum);
                    player.DecrementMoney(tenProcent);
                    return String.Format("Hráč {0} zaplatil na daních {1}$.", player.Nick, tenProcent);
                }
            else
            {
                player.DecrementMoney(this.TaxAmount);
                return String.Format("{0}: činí {1} $", this.Name, this.TaxAmount);
            }
        }
    }
}
