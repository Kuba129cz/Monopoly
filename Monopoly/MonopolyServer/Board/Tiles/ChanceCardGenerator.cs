using System;
using System.Collections.Generic;

namespace MonopolyServer.Board.Tiles
{
    class ChanceCardGenerator
    {
        private static readonly Random rng = new Random();
        private const int GO_POSITION = 0;
        private const int MAYFAIR_POSITION = 39;
        private const int BANK_MONEY_BONUS = 100;
        private const int PRETTY_BONUS = 50;

        private static List<Func<Player, string>> listOfChanceCards = new List<Func<Player, string>>
        {
            BankIsGivingYouMoney,
            YouArePrettyGivingBonus,
            one,two, three, four, five, six, seven
            //GiveAmountToOtherPlayers
        };
        private static string BankIsGivingYouMoney(Player player)
        {
            player.IncrementMoney(BANK_MONEY_BONUS);
            return "Banka ti dává 100$. Využij je dobře!";
        }
        private static string one(Player player)
        {
            player.IncrementMoney(150);
            return "Jako subvenci na stavbu obdržíš 150$.";
        }
        private static string two(Player player)
        {
            player.CurrentPosition = 15;
            return "Použij osobního vlaku Praha-Plzen a minešli start obdržíš 200$.";
        }
        private static string three(Player player)
        {
            player.CurrentPosition = 39;
            return "Projdi se po Václavském náměstí.";
        }
        private static string four(Player player)
        {
            player.DecrementMoney(150);
            return "Zaplat školné 150$.";
        }
        private static string five(Player player)
        {
            player.CurrentPosition = 0;
            return "Jdi ke startu!";
        }
        private static string six(Player player)
        {
            player.CurrentPosition = 10;
            player.IsInJail = true;
            player.RoundInJail = 3;
            return "Jdi přímo do žaláře.";
        }
        private static string seven(Player player)
        {
            player.CurrentPosition -= 3;
            return "Jdi zpět o 3 místa.";
        }
        private static string eight(Player player)
        {
            player.CurrentPosition = 26;
            return "Prohlédni si Botanickou zahradu, minešli start obdržíš 200$.";
        }
        //private static string nine(Player player)
        //{
        //    player.CurrentPosition = 26;
        //    return "Jsi propuštěn ze žaláře zdarma. Tuto kartu si můžeš ponechat pro starce příhodu.";
        //}
        //private static string six(Player player)
        //{
        //    player.DecrementMoney()
        //    return "Jsi povinen zaplatit činžovní dan, za každý dům 40$, za každý hotel 200$.";
        //}
        private static string YouArePrettyGivingBonus(Player player)
        {
            player.IncrementMoney(PRETTY_BONUS);
            return "Vyhrál si návrhářskou soutěž.\nObdržíš 50$";
        }
        //private static string GiveAmountToOtherPlayers(Player player)
        //{
        //    Board.players[Board.CurrentPlayerIndex].DecrementMoney(30);
        //    Board.players[(Board.CurrentPlayerIndex + 1) % 2].IncrementMoney(30);
        //    return "The other player is smarter.\n Give him 30$";
        //}

        public static string GenerateRandomCard(Player player)
        {
            Func<Player, string> randomChanceCard = listOfChanceCards[rng.Next(0, 2)];
            return randomChanceCard.Invoke(player);
        }
    }
}
