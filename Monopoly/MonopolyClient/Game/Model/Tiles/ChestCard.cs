using Monopoly.MonopolyGame.Model;
using Monopoly.MonopolyGame.Model.Interfaces;
using Monopoly.MonopolyGame.Model.Tiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monopoly.Game.Model.Tiles
{
    class ChestCard : Tile, ITile
    {
        public ChestCard(int index, string name) : base(index, name)
        {

        }
        public override string ActOnPlayer(Player player)
        {
            return ChestCardGenerator.GenerateRandomCard(player);
        }
    }
}
