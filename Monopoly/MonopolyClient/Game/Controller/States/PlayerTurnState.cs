using Monopoly.Play.View;
using System;
using System.Collections.Generic;
using System.Text;
using Myra.Graphics2D.UI;
using Monopoly.MonopolyGame.Model.Enums;
using System.Threading;
using Monopoly.Communication;

namespace Monopoly.MonopolyGame.Controller.States
{
    class PlayerTurnState : State
   {
        private Renderer renderer;
        private TextButton buttonThrowDice;
        public PlayerTurnState(State nextState): base(nextState)
        {
            renderer = GameState.GetRenderer();
            buttonThrowDice = renderer.ButtonThrow;
            buttonThrowDice.Click += ThrowDiceClick;
            buttonThrowDice.Enabled = false;
        }

        public override void Execute()
        {
            Communication.Query.UpdateThisPlayerInActualLobby();
            playerOnMove = Query.GetPlayerOnTurn(false);
            if (Data.ThisPlayer.IDPlayer == playerOnMove.IDPlayer)
            {
                buttonThrowDice.Enabled = true;
                if(playerOnMove.IsInJail)
                    StateMachine.ChangeState();
            }
            else
            {
                buttonThrowDice.Enabled = false;
                if ( Data.ThisPlayer.IDPlayer != playerOnMove.IDPlayer && playerOnMove.ShouldPlayerMove)
                    {
                    playerOnTurn = false;
                    StateMachine.ChangeState();//pokud jiny hrac jiz odehral prepnout do noveho stavu
                }
            }
        }
        private void ThrowDiceClick(object sender, EventArgs e)
        {
            Communication.Query.PlayerMove(Data.ThisPlayer);
            buttonThrowDice.Enabled = false;
            playerOnTurn = true;
            StateMachine.ChangeState();
        }
    }
}
