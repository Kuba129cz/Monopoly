using Monopoly.Communication;
using Monopoly.MonopolyGame.Model;
using Monopoly.MonopolyGame.Model.Tiles;
using Monopoly.Play.View;
using MonopolyServer.Board.Tiles;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monopoly.MonopolyGame.Controller.States
{
    class PlayerLandedState:State
    {
        public PlayerLandedState(State nextState) : base(nextState) { }
        public override void Execute()
        {
            int lastCurrentPosition = playerOnMove.CurrentPosition;
            bool goToJail = false;
            Communication.Query.UpdateThisPlayerInActualLobby();
            playerOnMove = Communication.Query.GetPlayerOnTurn();
            Lobby.Lobby lobby = Query.GetActualLobby(false);
            Tile currentTile = Board.allTiles[playerOnMove.CurrentPosition];          

            if (playerOnTurn && !buyingIsInProgress)
            {
                buyingIsInProgress = true;
                Communication.Query.GamePlayerLandedState(playerOnMove);

                if (currentTile is Street)
                {
                    var currentTileAsStreet = currentTile as Street;
                    if (currentTileAsStreet.Owner == Guid.Empty && playerOnMove.Money >= currentTileAsStreet.Price)
                    {
                        GameState.GetRenderer().DialogOfEvent(String.Format("Chceš zakoupit {0} za {1}$", currentTileAsStreet.Name, currentTileAsStreet.Price));
                    }else
                     if(currentTileAsStreet.Owner == playerOnMove.IDPlayer)
                    {
                        for (int i = 0; i < currentTileAsStreet.Houses.Length; i++)
                        {
                            if(currentTileAsStreet.Houses[i] == false)
                            {
                                if(i!=4)
                                {
                                    GameState.GetRenderer().DialogOfEvent(String.Format("Chceš koupit nový dům na {0} za {1}$", currentTileAsStreet.Name, currentTileAsStreet.PriceHouse), true);
                                    break;
                                }else
                                {
                                    GameState.GetRenderer().DialogOfEvent(String.Format("Chceš koupit hotel na {0} za {1}$", currentTileAsStreet.Name, currentTileAsStreet.PriceHouse), true);
                                    break;
                                }

                            }
                        }
                    }
                }
                else
                if (currentTile is Train)
                {
                    var currentTileAsTrain = currentTile as Train;
                    if (currentTileAsTrain.Owner == Guid.Empty && playerOnMove.Money >= currentTileAsTrain.Price)
                    {
                        GameState.GetRenderer().DialogOfEvent(String.Format("Chceš zakoupit {0} za {1}$", currentTileAsTrain.Name, currentTileAsTrain.Price));
                    }
                }
                else
                if (currentTile is DiceCard)
                {
                    var currentTileAsDiceCard = currentTile as DiceCard;
                    if (currentTileAsDiceCard.Owner == Guid.Empty && playerOnMove.Money >= currentTileAsDiceCard.Price)
                    {
                        GameState.GetRenderer().DialogOfEvent(String.Format("Chceš zakoupit {0} za {1}$", currentTileAsDiceCard.Name, currentTileAsDiceCard.Price));
                    }
                }else
                if (currentTile is SpecialTile)
                {
                    var currentTileAsSpecial = currentTile as SpecialTile;
                    if (currentTileAsSpecial.Index == 30)
                    {
                        goToJail = true;
                        playerOnMove.CurrentPosition = 10;
                        GameState.GetRenderer().MovePlayer(playerOnMove.Pawn, playerOnMove.CurrentPosition);
                    }
                    if(currentTileAsSpecial.Index == 10)
                    {
                        goToJail = false;
                    }
                }

            }
            if(playerOnMove.CurrentPosition != lastCurrentPosition)
            {
                StateMachine.MoveState();
            }
            if (currentTile is Street || currentTile is Train || currentTile is DiceCard)
            {
                if (lobby.EndOfBuying) //signal bude true prepnout
                {
                    lobby.EndOfBuying = false;
                    buyingIsInProgress = false;
                    StateMachine.ChangeState();
                }
            }
            else
            {
                if(!goToJail)
                buyingIsInProgress = false;
                StateMachine.ChangeState();
            }
        }

    }
}
