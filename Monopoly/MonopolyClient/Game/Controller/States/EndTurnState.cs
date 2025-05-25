using Monopoly.Communication;
using Monopoly.MonopolyGame.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monopoly.MonopolyGame.Controller.States
{
    class EndTurnState : State
    {
        public EndTurnState(State endState):base(endState)
        { }
        public override void Execute()
        {
            //  Player playerOnMove = Communication.Query.get();
            Communication.Query.UpdateThisPlayerInActualLobby();
            if (playerOnTurn)
            {
                Communication.Query.GameEndTurn(Data.ThisPlayer);
                //  GameState.GetRenderer().win.Closing += (s, a) => { StateMachine.ChangeState(); };
            }
                StateMachine.ChangeState();

        }
        private void activateEndButton()
        {
          //  Board.CurrentPlayerIndex = (Board.CurrentPlayerIndex + 1) % Board.players.Count; //udava jaky hrac je na rade
        }
    }
}
