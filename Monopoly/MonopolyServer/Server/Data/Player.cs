using MonopolyServer.Board.Tiles;
using System;
using System.Collections.Generic;
namespace MonopolyServer
{
   public enum Pawn { blue, green, red, yellow }
    public class Player
    {
        public const int TOTAL_NUMBER_OF_TILES = 40;
        public const int INITIAL_PLAYER_MONEY = 1500;
        public Guid IDPlayer { get; set; }
        public Pawn Pawn { get; set; }
        public string Nick { get; set; }
        public Guid IDLobby { get; set; }
        public DateTime TimeInLobby { get; set; }
        public int CurrentPosition { get; set; } = 0;
        public bool IsInJail { get; set; } = false;
        public int RoundEnteredToJail { get; set; } = 0;
        public int Money { get; set; } = INITIAL_PLAYER_MONEY;
        public bool IsOnTurn { get; set; } = false;
        public bool ShouldPlayerMove { get; set; } = false;
        public int whiteDiceNumber { get; set; }
        public int blackDiceNumber { get; set; }
        public bool Tax { get; set; } = false; //false -200$, true - 10%
        public int RoundInJail { get; set; }
        public bool Leave { get; set; } = false;
        public Player(string nick)
        {
            IDPlayer = Guid.NewGuid();
            IDLobby = Guid.Empty;
            this.Nick = nick;
        }
        public Player()
        {
            IDPlayer = Guid.NewGuid();
        }
        public void DecrementMoney(int amount)
        {
            this.Money -= amount;
            if(this.Money < 0)
            {               
               Program.MonopolyServer.AddInformationText(Server.MonopolyServer.FindLobby(this.IDLobby), String.Format("Hráč {0} bankrotuje, probíhá automatický prodej pozemků.", this.Nick));
               Server.MonopolyServer.FindBoard(this.IDLobby).AutoSell(this);
                Program.MonopolyServer.broadcastBoard(Server.MonopolyServer.FindBoard(this.IDLobby));
                if (this.Money < 0)
                {
                    Program.MonopolyServer.AddInformationText(Server.MonopolyServer.FindLobby(this.IDLobby), String.Format("Hráč {0} zbankrotoval.", this.Nick));
                    Program.MonopolyServer.EndMatch(this);
                }
            }
        }
        public void IncrementMoney(int amount)
        {
            this.Money += amount;
        }
        public void SetPosition(int newPosition)
        {
            int modifiedPosition = newPosition;
            if (modifiedPosition < 0)
            {
                modifiedPosition += TOTAL_NUMBER_OF_TILES;
            }
            if (modifiedPosition >= TOTAL_NUMBER_OF_TILES)
            {
                modifiedPosition -= TOTAL_NUMBER_OF_TILES;
                this.IncrementMoney(200);
            }
            this.CurrentPosition = modifiedPosition;
        }
    }
}

