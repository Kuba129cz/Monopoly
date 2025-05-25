using Monopoly.Intro;
using Monopoly.MatchHistory;
using Monopoly.Rooms;
using System.Collections.Generic;

namespace Monopoly.Communication
{
    static class Data
    {
        public static Player ThisPlayer;
        public static Room room;
        public static User user;
        public static List<BasicStatistics> Statistics;

        public static bool LostConnection = false;
        public static string LostConnectionMessage = null;
    }
}
