using Monopoly.Communication.Informations;
using Monopoly.Intro;
using Monopoly.MatchHistory;
using Monopoly.MonopolyGame.Model;
using Monopoly.MonopolyGame.Model.Tiles;
using Monopoly.Rooms;
using MonopolyServer.Board.Tiles;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Timers;

namespace Monopoly.Communication
{
    class ProcessReceivedData
    {
        private Timer timerPing = new Timer();
        public void UpdateRoom(string resolt)
        {
            try
            {
                Data.room = JsonConvert.DeserializeObject<Room>(resolt);
                Query.UpdateThisPlayerInRoomList();

                if (GameState.GetCurrentState() == GameStates.Room)
                {
                    GameState.GetRoom().updatePlayersInPlayersList();
                }
            }catch(Exception ex)
            {
                Error.HandleError(ex);
            }
        }
        public void UpdateThisPlayer(string resolt)
        {
            try
            {
                Player player = JsonConvert.DeserializeObject<Player>(resolt);
                if (Data.ThisPlayer != null)
                    Data.ThisPlayer.CopyPlayer(player);
                else
                    Data.ThisPlayer = player;
            }catch(Exception ex)
            {
                Error.HandleError(ex);
            }
        }
        public void UpdateSpecificLobby(string resolt)
        {
            try
            {
                Lobby.Lobby updatedLobby;
                updatedLobby = JsonConvert.DeserializeObject<Lobby.Lobby>(resolt);
                if (!updateLobbyInTheRoom(updatedLobby))
                    Data.room.Lobbies.Add(updatedLobby);
                if (GameState.GetCurrentState() == GameStates.Lobby)
                    Lobby.DesignLobby.UpdateListOfPlayersInLobby(updatedLobby);
                Query.UpdateThisPlayerInRoomList();
            }
            catch (Exception ex)
            {
                Error.HandleError(ex);
            }
        }
        private static bool updateLobbyInTheRoom(Lobby.Lobby updatedLobby)
        {
            try
            {
                for (int i = 0; i < Data.room.Lobbies.Count; i++)
                    if (updatedLobby.IDLobby == Data.room.Lobbies[i].IDLobby)
                    {
                        Data.room.Lobbies[i].Coppy(updatedLobby);
                        return true;
                    }
                return false;
            }
            catch (Exception ex)
            {
                Error.HandleError(ex);
                return false;
            }
        }

        public void UpdateListOfLobbies(string resolt)
        {try
            {
                Data.room.Lobbies = JsonConvert.DeserializeObject<ObservableCollection<Lobby.Lobby>>(resolt);
                for (int i = 0; i < Data.room.Players.Count; i++)
                {
                    for (int j = 0; j < Data.room.Lobbies.Count; j++)
                    {
                        for (int k = 0; k < Data.room.Lobbies[j].players.Length; k++)
                        {
                            if (Data.room.Lobbies[j].players[k] != null && Data.room.Lobbies[j].players[k].IDPlayer == Data.room.Players[i].IDPlayer)
                            {
                                Data.room.Players[i] = Data.room.Lobbies[j].players[k];
                                break;
                            }
                        }
                    }
                }
                Query.UpdateThisPlayerInRoomList();
                if (GameState.GetCurrentState() == GameStates.Room)
                    DesignRoom.UpdateListOfRooms();
            }
            catch (Exception ex) { Error.HandleError(ex); }
        }

        internal void WriteMessage(string desObj)
        {try
            {
                Chat.Chat chat = JsonConvert.DeserializeObject<Chat.Chat>(desObj);
                if (GameState.GetCurrentState() == GameStates.Game)
                    GameState.GetRenderer().addMessageToChatList(chat);
                if (GameState.GetCurrentState() == GameStates.Room)
                    GameState.GetRoom().addMessageToChatList(chat);
                if (GameState.GetCurrentState() == GameStates.Lobby)
                    GameState.GetLobby().addMessageToChatList(chat);
            }
            catch (Exception ex) { Error.HandleError(ex); }
        }

        internal void WriteInformationText(string desObj)
        {
            try
            {
                InformationText infText = JsonConvert.DeserializeObject<InformationText>(desObj);
                GameState.GetRenderer().AddTextToHistoryList(infText.ToString());
            }catch(Exception ex)
            {
                Error.HandleError(ex);
            }
        }
        internal void user(string desObj)
        {
            try
            {
                Data.user = JsonConvert.DeserializeObject<User>(desObj);
            }catch(Exception ex)
            {
                Error.HandleError(ex);
            }
        }
        internal void ReceivedStatistics(string desObj)
        {
            try
            {
                Data.Statistics = JsonConvert.DeserializeObject<List<BasicStatistics>>(desObj);
            }catch(Exception ex) { Error.HandleError(ex); }
        }
        internal void updateBoard(string desObj)
        {try
            {
                JsonConverter[] converters = { new BoardConverter() };
                Board.allTiles = JsonConvert.DeserializeObject<List<Tile>>(desObj, new JsonSerializerSettings() { Converters = converters });
                for (int i = 0; i < Board.allTiles.Count; i++)
                {
                    if (Board.allTiles[i] is Street)
                    {
                        Street currentStreet = (Street)Board.allTiles[i];
                        if (currentStreet.Owner != Guid.Empty)
                        {
                            GameState.GetRenderer().ShowTileOwner(Query.FindPlayerByID(currentStreet.Owner).Pawn, currentStreet.Index);
                            GameState.GetRenderer().ShowHouses(Query.FindPlayerByID(currentStreet.Owner).Pawn, currentStreet);
                        }
                    }
                    else if (Board.allTiles[i] is Train)
                    {
                        Train currentTrain = (Train)Board.allTiles[i];
                        if (currentTrain.Owner != Guid.Empty)
                        {
                            GameState.GetRenderer().ShowTileOwner(Query.FindPlayerByID(currentTrain.Owner).Pawn, currentTrain.Index);
                        }
                    }
                    else if (Board.allTiles[i] is DiceCard)
                    {
                        DiceCard currentDiceCard = (DiceCard)Board.allTiles[i];
                        if (currentDiceCard.Owner != Guid.Empty)
                        {
                            GameState.GetRenderer().ShowTileOwner(Query.FindPlayerByID(currentDiceCard.Owner).Pawn, currentDiceCard.Index);
                        }
                    }
                }
            }
            catch (Exception ex) { Error.HandleError(ex); }
        }

        internal void ReceivedPing(string desObj)
        {
            try
            {
                //do deseti sekund
                timerPing.Close();
                timerPing.Interval = 10000;//pokud se neozve do teto doby, hodi error a odpoji od serveru
                timerPing.Elapsed += lostConection;
                timerPing.Start();
            }catch(Exception ex) { Error.HandleError(ex); }
        }

        private void lostConection(object sender, ElapsedEventArgs e)
        {try
            {
                Query.DisconectFromServer();
                Data.LostConnection = true;
                Data.LostConnectionMessage = "Ztraceno spojení se serverem.";
                timerPing.Close();
                GameState.ChangeGameState(GameStates.Intro);
                GameState.ShowMessageBox("Ztraceno spojení se serverem.");
            }
            catch (Exception ex) { Error.HandleError(ex); }
        }

        internal void KickPlayer(string desObj)
        {try
            {
                Query.DisconectFromServer();
                Data.LostConnection = true;
                Data.LostConnectionMessage = "Byl jsi vyhozen ze serveru.";
                timerPing.Close();
            }
            catch (Exception ex) { Error.HandleError(ex); }
        }
    }
}
