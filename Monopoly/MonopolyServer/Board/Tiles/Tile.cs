using MonopolyServer.Board.Interfaces;
namespace MonopolyServer.Board.Tiles
{
    public abstract class Tile : ITile
    {
        public int Index { get; private set; }
        public string Name { get; private set; }
        public char Marker { get; set; }
        public abstract string ActOnPlayer(Player player);
        public Tile(int index, string name, char marker)
        {
            this.Index = index;
            this.Name = name;
            this.Marker = marker;
        }
    }
}
