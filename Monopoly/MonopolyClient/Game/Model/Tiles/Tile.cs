using Monopoly.MonopolyGame.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monopoly.MonopolyGame.Model
{
    abstract class Tile:ITile
    {
        public int Index { get; private set; }
        public string Name { get; private set; }
        public abstract string ActOnPlayer(Player player);
        public Tile(int index, string name)
        {
            this.Index = index;
            this.Name = name;
        }
    }
}
