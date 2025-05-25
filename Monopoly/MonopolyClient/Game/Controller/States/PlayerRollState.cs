using Monopoly.Communication;
using Monopoly.MonopolyGame.Model;
using Monopoly.Play.View;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Monopoly.MonopolyGame.Controller.States
{
    class PlayerRollState : State
    {
        private Renderer render;
        public PlayerRollState(State nextState):base(nextState)
        {
            render = GameState.GetRenderer();
        }
        public override void Execute()
        {
            //int currentPlayerPosition = Board.players[Board.CurrentPlayerIndex].CurrentPosition;
            //  Player playerOnMove = Query.GetPlayerOnTurn();
            if (playerOnMove.IsInJail)
                StateMachine.ChangeState();
            else
            {
                render.BlackDice.ChangeDiceImage(playerOnMove.blackDiceNumber);
                render.WhiteDice.ChangeDiceImage(playerOnMove.whiteDiceNumber);
                StateMachine.ChangeState();
            }
        }
    }
}
