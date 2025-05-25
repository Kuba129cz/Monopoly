using Monopoly.Communication;
using Monopoly.Play.View;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monopoly.MonopolyGame.Controller.States
{
    class PlayerMoveState:State
    {
        private Renderer renderer;
        public PlayerMoveState(State nextState) : base(nextState)
        {
            renderer = GameState.GetRenderer();
        }

        public override void Execute()       
        {
            if (playerOnMove.IsInJail)
                StateMachine.ChangeState();
            else
            {
                if (playerOnMove != null)
                    renderer.MovePlayer(playerOnMove.Pawn, playerOnMove.CurrentPosition);

                if (renderer.ShouldPlayerMove == false) //indikuje ze se figurka uz nehybe
                {
                    StateMachine.ChangeState();
                }
            }
        }
    }
}
