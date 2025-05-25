using System;
using System.Collections.Generic;
using Monopoly.MonopolyGame.Model.Enums;
using Monopoly.MonopolyGame.Model.Tiles;

namespace Monopoly
{
    class Player
    {
        public const int TOTAL_NUMBER_OF_TILES = 40;
        public const int INITIAL_PLAYER_MONEY = 1500;
        public Guid IDPlayer { get; set; }
        public Pawn Pawn { get; set; }
        public string Nick { get; set; }
        public Guid IDLobby { get; set; }
        public DateTime TimeInLobby { get; set; }
   //     public List<Street> Streets { get; set; } = new List<Street>();
        public int CurrentPosition { get; set; } = 0;
        public bool IsInJail { get; set; } = false;
        public int Money { get; set; } = INITIAL_PLAYER_MONEY;
        public bool IsOnTurn { get; set; } = false;
        public int whiteDiceNumber { get; set; }
        public int blackDiceNumber { get; set; }
        public bool ShouldPlayerMove { get; set; } = false;
        public bool Tax { get; set; } = false; //false -200$, true - 10%
        public int RoundInJail { get; set; }
        public bool Leave { get; set; } = false;

        //   public int TotalPositionToMove { get; set; }
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
        internal void CopyPlayer(Player player)
        {
            this.IDPlayer = player.IDPlayer;
            this.Pawn = player.Pawn;
            this.Nick = player.Nick;
            this.IDLobby = player.IDLobby;
            this.TimeInLobby = player.TimeInLobby;
          //  this.Streets = player.Streets;
            this.CurrentPosition = player.CurrentPosition;
            this.IsInJail = player.IsInJail;
            this.Money = player.Money;
            this.IsOnTurn = player.IsOnTurn;
            this.whiteDiceNumber = player.whiteDiceNumber;
            this.blackDiceNumber = player.blackDiceNumber;
            this.ShouldPlayerMove = player.ShouldPlayerMove;
            this.Tax = player.Tax;
            this.RoundInJail = player.RoundInJail;
          //  this.Streets = player.Streets; //snad jedno
        }
        public void DecrementMoney(int amount)
        {
            this.Money -= amount;
        }
        public void IncrementMoney(int amount)
        {
            this.Money += amount;
        }
        public void SetPosition(int newPosition)
        {
            int modifiedPosition = newPosition;
            if(modifiedPosition<0)
            {
                modifiedPosition += TOTAL_NUMBER_OF_TILES;
            }
            if(modifiedPosition >=TOTAL_NUMBER_OF_TILES)
            {
                modifiedPosition -= TOTAL_NUMBER_OF_TILES;
                this.IncrementMoney(200);
            }
            this.CurrentPosition = modifiedPosition;
        }
    }
}
