using MonopolyServer;
using MonopolyServer.Board.Interfaces;
using MonopolyServer.Board.Tiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monopoly.Game.Model.Tiles
{
    class ChestCard : Tile, ITile
    {
        public ChestCard(int index, string name, char marker) : base(index, name, marker)
        {

        }
        public override string ActOnPlayer(Player player)
        {
            return ChestCardGenerator.GenerateRandomCard(player);
        }
    }
}
