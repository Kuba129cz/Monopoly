using MonopolyServer.Server.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonopolyServer.Server
{
    partial class MonopolyServer
    {
        private object GetBasicStatistics(string desObj)
        {
            Guid idPlayer = JsonConvert.DeserializeObject<Guid>(desObj);
           return mysqlConnection.ReadBasicStatistics(idPlayer);
        }
        private object GetDetailedStatistics(string desObj)
        {
            BasicStatistics basicStatistics = JsonConvert.DeserializeObject<BasicStatistics>(desObj);
            return mysqlConnection.ReadDetailedStatistics(basicStatistics.IDPlayer, basicStatistics.IDLobby);
        }
    }
}
