using Monopoly.MonopolyGame.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monopoly.MonopolyGame.Model.Tiles
{
    class Street : Tile, ITile
    {
        public NeighbourHoodType Neighbourhood { get; set; }
        public Guid Owner { get; set; }
        public int Price { get; set; }
        public int Rent { get; set; }
        public int[] RentWithHouses;
        public bool[] Houses = { false, false, false, false, false }; //1,2,3,4,hotel
        public int PriceHouse;
        public Street(int index, string name, NeighbourHoodType neighbourhood, int price, int rent,
            int rentHouse1, int rentHouse2, int rentHouse3, int rentHouse4, int rentHotel, int priceHouse)
            : base(index, name)
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
                return "You already own" + this.Name;
            }
            else if(this.Owner == Guid.Empty)
            {
                // return this.Name + "is avaible \n for purchase";
                return this.Name + "Je k dispozici pro nákup";
            }
            else
            {
                player.DecrementMoney(this.Rent);

                //Communication.Query.GetThisLobby()

                //this.Owner.IncrementMoney(this.Rent);
                return string.Format("{0}\n is owned by player{1}." +
                    "\nYou paid him {2}!", this.Name, player.IDPlayer, this.Rent);
            }
        }
    }
}
