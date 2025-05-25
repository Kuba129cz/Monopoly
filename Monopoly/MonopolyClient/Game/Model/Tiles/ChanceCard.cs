using Monopoly.MonopolyGame.Model.Interfaces;

namespace Monopoly.MonopolyGame.Model.Tiles
{
    class ChanceCard : Tile, ITile
    {
        public ChanceCard(int index, string name): base(index, name)
        {

        }
        public override string ActOnPlayer(Player player)
        {
            return ChanceCardGenerator.GenerateRandomCard(player);
        }
    }
}
