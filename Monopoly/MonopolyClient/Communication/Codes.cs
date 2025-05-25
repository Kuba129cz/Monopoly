using System;
using System.Collections.Generic;
using System.Text;
namespace Monopoly.Communication
{
    public enum SENDING_CODES { 
        startClient=0,registration, rooms, createRoom, joinRoom, removePL, removePlFmL,
        launchGame, updatePlayer, updateLobby, playerMove, getSpecificLobbyByPlayer, message, informationText, playerLandedState,
        endTurn, buyStreet, endBuy, buyHouse, login, leaveFromGame, basicStatistics, detailedStatistics

    }
    public enum RECEIVING_CODES { 
        room, thisPlayer, specificLobby, ListOfLobbies, received, message, informationText, buyStreet, board, user, endMatch, Statistics, ping, kickPlayer
    }
}
