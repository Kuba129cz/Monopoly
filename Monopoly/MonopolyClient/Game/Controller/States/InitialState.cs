using Monopoly.Communication;
using Monopoly.MonopolyGame.Model;
using System.Threading;

namespace Monopoly.MonopolyGame.Controller.States
{
    class InitialState : State
    {
        public InitialState(State nextState) : base(nextState)
        {
        }

        private void updating()
        {
        }

        public override void Execute()
        {
            Board.InitializeBoard();
        }
    }
}
