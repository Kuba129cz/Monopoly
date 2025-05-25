using System;
using System.Collections.Generic;
using System.Text;

namespace MonopolyServer.Server.Data
{
    class BuyTile
    {
        public Guid IdPlayer { get; set; }
        public Pawn Pawn { get; set; }
        public int CurrentPlayerPosition { get; set; }
        public Guid IdLobby { get; set; }
        //public Board.Board Board { get; set; }
        public BuyTile(Guid idPlayer, Pawn pawn, int currentPlayerPosition, Guid idLobby)
        {
            this.IdPlayer = idPlayer;
            this.Pawn = pawn;
            this.CurrentPlayerPosition = currentPlayerPosition;
            this.IdLobby = idLobby;
            //Board = new Board.Board();
            //Board.InitializeBoard();
        }
    };
}
