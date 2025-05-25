using System;
using System.Collections.Generic;
using System.Text;

namespace MonopolyServer
{
    public enum RECEIVING_CODES
    {
        startClient, registration, rooms, createRoom, joinRoom, removePL, removePlFmL, launchGame,
        updatePlayer, updateLobby, playerMove, getSpecificLobbyByPlayer, message, informationText, playerLandedState, endTurn,
        buyStreet, endBuy, buyHouse, login, leaveFromGame, basicStatistics, detailedStatistics
    }
    public enum SENDING_CODES
    {
       room, thisPlayer, specificLobby, ListOfLobbies, received, message, informationText, buyStreet, board, user, endMatch, Statistics, ping, kickPlayer
    }
}
