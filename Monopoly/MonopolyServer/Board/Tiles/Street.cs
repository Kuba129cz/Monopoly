using MonopolyServer.Board.Enums;
using MonopolyServer.Board.Interfaces;
using System;

namespace MonopolyServer.Board.Tiles
{
    public class Street : Tile, ITile
    {
        public NeighbourHoodType Neighbourhood { get; set; }
        public Guid Owner { get; set; }
        public int Price { get; set; }
        public int Rent { get; set; }
        public int[] RentWithHouses;
        public bool[] Houses = { false, false, false, false, false }; //1,2,3,4,hotel
        public int PriceHouse;
        public Street(int index, string name, NeighbourHoodType neighbourhood, int price, int rent, 
            int rentHouse1, int rentHouse2, int rentHouse3, int rentHouse4, int rentHotel, int priceHouse, char marker )
            :base(index, name, marker)
        {
            this.Neighbourhood = neighbourhood;
            this.Price = price;
            this.Rent = rent;
            this.Owner = Guid.Empty;
            this.RentWithHouses = new int[5];
            this.RentWithHouses[0] = rentHouse1;
            this.RentWithHouses[1] = rentHouse2;
            this.RentWithHouses[2] = rentHouse3;
            this.RentWithHouses[3] = rentHouse4;
            this.RentWithHouses[4] = rentHotel;
            this.PriceHouse = priceHouse;
        }
        public override string ActOnPlayer(Player player)
        {
            if(this.Owner == player.IDPlayer)
            {
                // return "You already own" + this.Name;
                return string.Format("Tuto nemovitost již vlastníš.");
            }
            else if(this.Owner == Guid.Empty)
            {
               // return this.Name + "is avaible \n for purchase";
               return string.Format("Tato nemovitost je na prodej.");
            }
            else
            {
                player.DecrementMoney(this.Rent);
                Lobby lobby = Server.MonopolyServer.FindLobby(player.IDLobby);
                Board board = Server.MonopolyServer.FindBoard(lobby.IDLobby);
                for(int i=0; i<lobby.players.Length;i++)
                {
                    if (lobby.players[i] != null)
                    {

                        for (int j = 0; j < board.allTiles.Count; j++)
                        {
                            if (board.allTiles[j] is Street)
                            {
                                if (((Street)board.allTiles[j]).Owner == lobby.players[i].IDPlayer)
                                {
                                    lobby.players[i].IncrementMoney(this.Rent);
                                    return string.Format("{0} vlastní hráč {1}. Zaplatíš mu {2}$.", this.Name, player.Nick, this.Rent);
                                }
                            }
                        }
                    }
                    else
                        continue;
                }
                return null;
            }
        }
    }
}
