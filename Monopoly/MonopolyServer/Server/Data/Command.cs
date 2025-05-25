using System;
using System.Collections.Generic;
using System.Text;

namespace MonopolyServer.Server.Data
{
     class Command
    {
        public delegate void function();
        public string Com { get; set; }
        public function Fun { get; set; }
        public string Info { get; set; }
        public Command(string command, function function, string info)
        {
            this.Com = command;
            this.Fun = function;
            this.Info = info;
        }
    }
}
