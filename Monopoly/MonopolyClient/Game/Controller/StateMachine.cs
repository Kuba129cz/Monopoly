using Monopoly.Communication;
using Monopoly.MonopolyGame.Controller.States;
using Monopoly.MonopolyGame.Model;
using Monopoly.Play.View;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Monopoly.MonopolyGame.Controller
{
    static class StateMachine
    {
        public static State CurrentState;
        public static Dictionary<string, State> States = new Dictionary<string, State>();
        private static InitialState initialState;
        private static PlayerTurnState  playerTurnState;
        private static PlayerRollState playerRollState;
        private static PlayerMoveState playerMoveState;
        private static PlayerLandedState playerLandedState;
        private static EndTurnState endTurnState;
        public static void Initialize()
        {
            endTurnState = new EndTurnState(playerTurnState);
            playerLandedState = new PlayerLandedState(endTurnState);
            playerMoveState = new PlayerMoveState(playerLandedState);
            playerRollState = new PlayerRollState(playerMoveState);
            playerTurnState = new PlayerTurnState(playerRollState);
            initialState = new InitialState(playerTurnState);

            endTurnState.NextState = playerTurnState;
            if (!States.ContainsKey("InitialState"))
            {
                States.Add("InitialState", initialState);
                States.Add("PlayerTurnState", playerTurnState);
                States.Add("PlayerRollState", playerRollState);
                States.Add("PlayerMoveState", playerMoveState);
                States.Add("PlayerLandedState", playerLandedState);
                States.Add("EndTurnState", endTurnState);
            }
        }

        public static void ChangeState()
        {
            CurrentState = CurrentState.NextState;
        }
        public static void MoveState()
        {
            CurrentState = playerMoveState;
        }

    }
}
