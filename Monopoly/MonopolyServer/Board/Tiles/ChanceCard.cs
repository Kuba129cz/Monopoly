using MonopolyServer.Board.Interfaces;

namespace MonopolyServer.Board.Tiles
{
    class ChanceCard : Tile, ITile
    {
        public ChanceCard(int index, string name, char marker): base(index, name, marker)
        {

        }
        public override string ActOnPlayer(Player player)
        {
            return ChanceCardGenerator.GenerateRandomCard(player);
        }
    }
}
