using Monopoly;
using Monopoly.MonopolyGame.Model;
using Monopoly.MonopolyGame.Model.Interfaces;
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
            public DiceCard(int index, string name, int price)
                : base(index, name)
            {
                this.Neighbourhood = NeighbourHoodType.Utility;
                this.Price = price;
                this.Owner = Guid.Empty;
            }
        public override string ActOnPlayer(Player player)
        {
            throw new NotImplementedException();
        }
    }
}
