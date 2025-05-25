using System;
using System.Collections.Generic;
using System.Text;

namespace MonopolyServer
{
    class InformationText
    {
        public string text { get; set; }
        public Lobby Lobby {get; set;}
        public InformationText(Lobby lobby, string text)
        {
            this.Lobby = lobby;
            this.text = text;
        }
        public override string ToString()
        {
            return string.Format("{0}", text);
        }
    }
}
