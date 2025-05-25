using System;
using System.Collections.Generic;
using System.Text;

namespace Monopoly.MonopolyGame.Model
{
    class InformationText
    {
        public string text { get; set; }
        public Lobby.Lobby Lobby {get; set;}

        public override string ToString()
        {
            return string.Format("{0}", text);
        }
        public InformationText(Lobby.Lobby lobby, string text)
        {
            this.Lobby = lobby;
            this.text = text;
        }
    }
}
