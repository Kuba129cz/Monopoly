namespace MonopolyServer.Board.Interfaces
{
    interface ITile
    {
        int Index { get; }
        string Name { get; }
        string ActOnPlayer(Player player);
    }
}
