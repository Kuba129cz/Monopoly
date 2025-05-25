using Monopoly.Communication;
using Monopoly.MonopolyGame.Model;
using Monopoly.Play.View;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monopoly.MonopolyGame.Controller.States
{
    abstract class State
    {
        public State NextState{ get; set; }
        protected Desktop desktop;
        protected static Player playerOnMove = null;
        protected static bool playerOnTurn = false;
        protected static bool buyingIsInProgress = false;
     //   public Board Board;
        public State(State nextState)
        {
            this.NextState = nextState;
            desktop = new Desktop();
          //  this.Board = new Board();
        }
        public abstract void Execute();
        public void Draw(Renderer renderer)
        {
            renderer.DrawBoard();
            desktop.Render();
        }
        protected Player GetPlayerOnShouldMove()
        {
            Lobby.Lobby lobby = Query.GetActualLobby(false);
            for (int i = 0; i < lobby.players.Length; i++)
            {
                if (lobby.players[i] != null && lobby.players[i].ShouldPlayerMove)
                {
                    return lobby.players[i];
                }
            }
            return null;
        }

    }
}
