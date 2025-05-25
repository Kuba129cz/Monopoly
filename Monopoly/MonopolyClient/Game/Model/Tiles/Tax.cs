using Monopoly.MonopolyGame.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monopoly.MonopolyGame.Model.Tiles
{
    class Tax : Tile, ITile
    {
        public int TaxAmount { get; private set; }

        public Tax(int index, string name, int taxAmount) : base(index, name)
        {
            this.TaxAmount = taxAmount;
        }

        public override string ActOnPlayer(Player player)
        {
            player.DecrementMoney(this.TaxAmount);
            return String.Format("{0}: {1}", this.Name, this.TaxAmount);
        }
    }
}
