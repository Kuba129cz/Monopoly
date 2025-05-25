using Monopoly.MonopolyGame.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monopoly.Play.View
{
   abstract class AbstractRenderer
    {
        public abstract void DrawBoard();
        public abstract void ShowTileOwner(Pawn pawn, int currentPlayerPosition);
        public abstract void MovePlayer(Pawn pawn, int currentPosition);
    }
}
