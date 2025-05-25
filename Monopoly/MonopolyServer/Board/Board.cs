using Monopoly.Game.Model.Tiles;
using MonopolyServer.Board.Enums;
using MonopolyServer.Board.Tiles;
using System;
using System.Collections.Generic;

namespace MonopolyServer.Board
{
   public class Board
    {
        public List<Tile> allTiles;
        public Guid IdLobby { get; set; } 

        public Board(Guid idLobby)
        {
            initializeBoard();
            this.IdLobby = idLobby;
        }
        private void initializeBoard()
        {
            //SpecialTile - A,  Street - B, ChestCard - C, TAX - D, Train - E, ChanceCard - F, DiceCard - G
            allTiles = new List<Tile>()
            {
                new SpecialTile(0,"GO!", 'A'),
                new Street(1, "Bráník", NeighbourHoodType.Orange, 120, 5, 20, 60, 180, 320, 420, 50, 'B'), //price, rent
                new ChestCard (2, "Pokladna", 'C'),
                new Street(3, "Podolí", NeighbourHoodType.Orange, 100, 5, 10, 30, 90, 160, 250, 50, 'B'), //price, rent
                new Tax(4, "Daň z majetku", 200, 'D'), //zlepsit
                new Train(5, "Praha-Podmokly", 200, 25, 'E'),
                new Street(6, "Masarykův stadion", NeighbourHoodType.CyanBlue, 120, 10, 40, 100, 300, 450, 600, 50, 'B'),
                new ChanceCard(7, "Šance", 'F'),
                new DiceCard(8, "Telefon", 150, 'G'),
                new Street(9, "Petřín", NeighbourHoodType.CyanBlue, 140, 10, 50, 150, 450, 625, 750, 100, 'B'),
                new SpecialTile(10, "Žalář", 'A'),
                new Street(11, "Pohořelec", NeighbourHoodType.CyanBlue, 140, 10, 50, 150, 450, 625, 700, 100, 'B'),
                new DiceCard(12, "Elektrický proud", 150, 'G'),
                new Street(13, "Primátorská třída", NeighbourHoodType.Red, 180, 15, 65, 100, 550, 720, 950, 100, 'B'),
                new Street(14, "Královská třída", NeighbourHoodType.Red, 180, 15, 65, 190, 550, 720, 950, 100, 'B'),
                new Train(15, "Praha-Plzeň", 200, 25, 'E'),
                new Street(16, "Táborská ulice", NeighbourHoodType.Red, 180, 15, 70, 200, 550, 750, 950, 100, 'B'),
                new ChestCard(17, "Pokladna", 'C'),
                new DiceCard(18, "Rádiová koncese", 200, 'G'),
                new Street(19, "Husova třída", NeighbourHoodType.SkyBlue, 200, 15, 80, 220, 600, 800, 1000, 100, 'B'),
                new SpecialTile(20, "Free parking", 'A'),
                new Street(21, "Vítkov", NeighbourHoodType.SkyBlue, 180, 15, 70, 200, 550, 750, 950, 100, 'B'),
                new ChanceCard(22, "Šance", 'F'),
                new Street(23, "Karlovo náměstí", NeighbourHoodType.SkyBlue, 240, 20, 100, 300, 750, 925, 1100, 150, 'B'),
                new Street(24, "Vyšehradská třída", NeighbourHoodType.SkyBlue, 220, 20, 90, 250, 800, 875, 1050, 150, 'B'),
                new Train(25, "Praha-Hradec Králové", 200, 25, 'E'),
                new Street(26, "Botanická zahrada", NeighbourHoodType.DarkBlue, 220, 20, 90, 250, 700, 875, 1050, 150, 'B'),
                new Street(27, "Fochova třída", NeighbourHoodType.DarkBlue, 200, 20, 110, 330, 800, 975, 1150, 150, 'B'),
                new DiceCard(28, "Vodovod", 150, 'G'),
                new Street(29, "Náměstí míru", NeighbourHoodType.Brown, 280, 25, 120, 360, 850, 1025, 1200, 150, 'B'),
                new SpecialTile(30, "Jdi do žaláře", 'A'),
                new Street(31, "Ovocný trh", NeighbourHoodType.Brown, 300, 25, 130, 390, 900, 1100, 1275, 200, 'B'),
                new DiceCard(32, "Rádiová koncese", 200, 'G'),
                new ChestCard(33, "Pokladna", 'C'),
                new Street(34, "Staroměstské náměstí", NeighbourHoodType.Brown, 360, 30, 150, 450, 1000, 1200, 1400, 200, 'B'),
                new Train(35, "Praha-Brno", 200, 25, 'E'),
                new ChanceCard(36, "Šance", 'F'),
                new Street(37, "Národní třída", NeighbourHoodType.Purple, 350, 35, 175, 500, 1000, 1300, 1500, 200, 'B'),
                new Tax(38, "Luxusní taxa",200, 'D'),
                new Street(39, "Václavské náměstí", NeighbourHoodType.Purple, 300, 50, 200, 600, 1400, 1700, 2000, 200, 'B'),
            };
        }
        public string AddTileToPlayer(Player player)
        {
            if (allTiles[player.CurrentPosition] is Street)
            {
                Street currentStreet = (Street)allTiles[player.CurrentPosition];
                currentStreet.Owner = player.IDPlayer;
               // player.Streets.Add(currentStreet);
                player.DecrementMoney(currentStreet.Price);
                return currentStreet.Name;
            }else
              if (allTiles[player.CurrentPosition] is Train)
            {
                Train currentTrain = (Train)allTiles[player.CurrentPosition];
                currentTrain.Owner = player.IDPlayer;
            //    player.Streets.Add(currentTrain);
                player.DecrementMoney(currentTrain.Price);
                return currentTrain.Name;
            }
            else
             if (allTiles[player.CurrentPosition] is DiceCard)
            {
                DiceCard currentDiceCard = (DiceCard)allTiles[player.CurrentPosition];
                currentDiceCard.Owner = player.IDPlayer;
                //    player.Streets.Add(currentTrain);
                player.DecrementMoney(currentDiceCard.Price);
                return currentDiceCard.Name;
            }
            return null;
        }
        public void AutoSell(Player player)
        {
                Program.MonopolyServer.AddInformationText(Server.MonopolyServer.FindLobby(player.IDLobby), String.Format("Hráč {0} bankrotuje, probíhá automatický prodej pozemků.", player.Nick));
                
                for (int i = 0; (i < Server.MonopolyServer.FindBoard(player.IDLobby).allTiles.Count) && (player.Money < 0); i++)
                {
                    if (allTiles[i] is Street)
                    {
                        Street currentStreet = (Street)allTiles[i];
                        if(currentStreet.Owner == player.IDPlayer)
                        {
                           for(int j= currentStreet.Houses.Length-1; (j> 0) && (player.Money < 0); j--)
                            {
                                if(currentStreet.Houses[j])
                                {
                                currentStreet.Houses[j] = false;
                                player.IncrementMoney(currentStreet.PriceHouse);
                                }
                            }
                            if (player.Money < 0)
                            {
                                currentStreet.Owner = Guid.Empty;
                                player.IncrementMoney(currentStreet.Price);
                            }
                        }
                    }
                    else
                    if (allTiles[i] is Train)
                    {
                        Train currentTrain = (Train)allTiles[i];
                        currentTrain.Owner = Guid.Empty;
                        player.IncrementMoney(currentTrain.Price);
                    }
                    else
                    if (allTiles[i] is DiceCard)
                    {
                        DiceCard currentDiceCard = (DiceCard)allTiles[i];
                        currentDiceCard.Owner = Guid.Empty;
                        player.IncrementMoney(currentDiceCard.Price);
                    }
                }
        }
        public string BuildHouse(Player player)
        {
            if (allTiles[player.CurrentPosition] is Street)
            {
                Street currentStreet = (Street)allTiles[player.CurrentPosition];
                for(int i=0; i<currentStreet.Houses.Length; i++)
                {
                    if(currentStreet.Houses[i] == false)
                    {
                        currentStreet.Houses[i] = true;
                        player.DecrementMoney(currentStreet.PriceHouse);
                        currentStreet.Rent = currentStreet.RentWithHouses[i];
                        if (i != 4)
                            return String.Format("{0} si koupil {1} dům za {2}.", player.Nick, i+1, currentStreet.PriceHouse);
                        else
                            return String.Format("{0} si koupil hotel.", player.Nick);
                    }
                }
            }
            return null;
        }
    }
}
