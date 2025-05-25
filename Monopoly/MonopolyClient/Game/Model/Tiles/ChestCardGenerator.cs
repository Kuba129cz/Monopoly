using Monopoly.MonopolyGame.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monopoly.Game.Model.Tiles
{
    class ChestCardGenerator
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
        };
        private static string BankIsGivingYouMoney(Player player)
        {
            player.IncrementMoney(BANK_MONEY_BONUS);
            return "Banka ti dává 100$.\nVyužij je dobře!";
        }
        private static string YouArePrettyGivingBonus(Player player)
        {
            player.IncrementMoney(PRETTY_BONUS);
            return "Vyhrál si návrhářskou soutěž.\nObdržíš 50$";
        }

        public static string GenerateRandomCard(Player player)
        {
            Func<Player, string> randomChanceCard = listOfChanceCards[rng.Next(0, 3)];
            return randomChanceCard.Invoke(player);
        }
    }
}
