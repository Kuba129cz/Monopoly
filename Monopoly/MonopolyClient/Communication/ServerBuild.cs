using System;
using System.Collections.Generic;
using System.Text;

namespace Monopoly.Communication
{
    class ServerBuild
    {
        public string Name { get; set; }
        public int Port { get; set; }
        public string IP { get; set; }
        public bool Online { get; set; } = false;
        public bool Join { get; set; } = false;
        public ServerBuild()
        {
        }
    }
}
