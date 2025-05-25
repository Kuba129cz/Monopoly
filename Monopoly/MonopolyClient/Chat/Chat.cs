using System;

namespace Monopoly.Chat
{
    class Chat
    {
       public string message { get; set; }
       public string nickOfPlayer { get; set; }
       public Guid ID { get; set; }
       public char marker { get; set; } //L - lobby, P - player, R - players in room

        public Chat(string nick, char marker, Guid id)
        {
            this.nickOfPlayer = nick;
            this.marker = marker;
            this.ID = id;
        }

        public void SendMessage(string msg)
        {
            this.message = msg;
            Communication.Query.SendMessage(this);
        }
        public override string ToString()
        {
            if (this.nickOfPlayer != null || this.message != null)
                return string.Format("{0}: {1}", this.nickOfPlayer, this.message);
            else
                return string.Empty;
        }
    }
}
