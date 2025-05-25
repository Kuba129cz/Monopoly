using MonopolyServer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monopoly.Game.Model.Tiles
{
    class ChestCardGenerator
    {
        private static readonly Random rng = new Random();
        private const int Stocks = 45;

        private static List<Func<Player, string>> listOfChanceCards = new List<Func<Player, string>>
        {
            stocksAreHigher,
            sickLeave,
            lifeInsurance
        };
        private static string stocksAreHigher(Player player)
        {
            player.IncrementMoney(Stocks);
            return "Cenné papíry stoupají obdržíš 45$.";
        }
        private static string sickLeave(Player player)
        {
            player.DecrementMoney(100);
            return "Nemocenské pokladně zaplať 100$.";
        }
        private static string lifeInsurance(Player player)
        {
            player.DecrementMoney(100);
            return "Za premií životní pojistky zaplať 100$.";
        }

        public static string GenerateRandomCard(Player player)
        {
            Func<Player, string> randomChanceCard = listOfChanceCards[rng.Next(0, 3)];
            return randomChanceCard.Invoke(player);
        }
    }
}
