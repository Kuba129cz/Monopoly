namespace Monopoly.MonopolyGame.Model.Interfaces
{
    interface ITile
    {
        int Index { get; }
        string Name { get; }
        string ActOnPlayer(Player player);
    }
}
