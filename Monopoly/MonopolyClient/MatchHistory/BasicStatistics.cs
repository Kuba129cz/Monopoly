using System;
using System.Collections.Generic;
using System.Text;

namespace Monopoly.MatchHistory
{
    class BasicStatistics
    {
        public int IDLobby { get; set; }
        public Guid IDPlayer { get; set; }
        public int ID { get; set; } = 0;
        public string Record { get; set; }
        public BasicStatistics(int id)
        {
            this.ID = id;
        }
    }
}
