using Monopoly.Communication;
using Monopoly.Game.Model.Tiles;
using Monopoly.MonopolyGame.Model.Tiles;
using Monopoly.Play.View;
using MonopolyServer.Board.Tiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monopoly.MonopolyGame.Model
{
    static class Board
    {
        public static List<Tile> allTiles;
        //public Board()
        //{
        //    InitializeBoard();
        //}
        public static void InitializeBoard()
        {
            allTiles = new List<Tile>()
            {
                new SpecialTile(0,"GO!"),
                new Street(1, "Bráník", NeighbourHoodType.Orange, 120, 5, 20, 60, 180, 320, 420, 50), //price, rent
                new ChestCard (2, "Pokladna"),
                new Street(3, "Podolí", NeighbourHoodType.Orange, 100, 5, 10, 30, 90, 160, 250, 50), //price, rent
                new Tax(4, "Daň z majetku", 200), //zlepsit
                new Train(5, "Praha-Podmokly", 200, 25),
                new Street(6, "Masarykův stadion", NeighbourHoodType.CyanBlue, 120, 10, 40, 100, 300, 450, 600, 50),
                new ChanceCard(7, "Šance"),
                new DiceCard(8, "Telefon", 150),
                new Street(9, "Petřín", NeighbourHoodType.CyanBlue, 140, 10, 50, 150, 450, 625, 750, 100),
                new SpecialTile(10, "Žalář"),
                new Street(11, "Pohořelec", NeighbourHoodType.CyanBlue, 140, 10, 50, 150, 450, 625, 700, 100),
                new DiceCard(12, "Elektrický proud", 150),
                new Street(13, "Primátorská třída", NeighbourHoodType.Red, 180, 15, 65, 100, 550, 720, 950, 100),
                new Street(14, "Královská třída", NeighbourHoodType.Red, 180, 15, 65, 190, 550, 720, 950, 100),
                new Train(15, "Praha-Plzeň", 200, 25),
                new Street(16, "Táborská ulice", NeighbourHoodType.Red, 180, 15, 70, 200, 550, 750, 950, 100),
                new ChestCard(17, "Pokladna"),
                new DiceCard(18, "Rádiová koncese", 200),
                new Street(19, "Husova třída", NeighbourHoodType.SkyBlue, 200, 15, 80, 220, 600, 800, 1000, 100),
                new SpecialTile(20, "Free parking"),
                new Street(21, "Vítkov", NeighbourHoodType.SkyBlue, 180, 15, 70, 200, 550, 750, 950, 100),
                new ChanceCard(22, "Šance"),
                new Street(23, "Karlovo náměstí", NeighbourHoodType.SkyBlue, 240, 20, 100, 300, 750, 925, 1100, 150),
                new Street(24, "Vyšehradská třída", NeighbourHoodType.SkyBlue, 220, 20, 90, 250, 800, 875, 1050, 150),
                new Train(25, "Praha-Hradec Králové", 200, 25),
                new Street(26, "Botanická zahrada", NeighbourHoodType.DarkBlue, 220, 20, 90, 250, 700, 875, 1050, 150),
                new Street(27, "Fochova třída", NeighbourHoodType.DarkBlue, 200, 20, 110, 330, 800, 975, 1150, 150),
                new DiceCard(28, "Vodovod", 150),
                new Street(29, "Náměstí míru", NeighbourHoodType.Brown, 280, 25, 120, 360, 850, 1025, 1200, 150),
                new SpecialTile(30, "Jdi do žaláře"),
                new Street(31, "Ovocný trh", NeighbourHoodType.Brown, 300, 25, 130, 390, 900, 1100, 1275, 200),
                new DiceCard(32, "Rádiová koncese", 200),
                new ChestCard(33, "Pokladna"),
                new Street(34, "Staroměstské náměstí", NeighbourHoodType.Brown, 360, 30, 150, 450, 1000, 1200, 1400, 200),
                new Train(35, "Praha-Brno", 200, 25),
                new ChanceCard(36, "Šance"),
                new Street(37, "Národní třída", NeighbourHoodType.Purple, 350, 35, 175, 500, 1000, 1300, 1500, 200),
                new Tax(38, "Luxusní taxa",200),
                new Street(39, "Václavské náměstí", NeighbourHoodType.Purple, 300, 50, 200, 600, 1400, 1700, 2000, 200),
            };
        }
        //public static void AddStreetToPlayer(int streetIndex, int playerIndex)
        //{
        //    Street currentStreet = (Street)allTiles[streetIndex];
        //    currentStreet.Owner = players[playerIndex];

        //    players[playerIndex].Streets.Add(currentStreet);
        //    players[playerIndex].DecrementMoney(currentStreet.Price);

        //}
    }
}
