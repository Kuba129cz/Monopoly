using System;
using System.Collections.Generic;
using System.Text;
using Monopoly;
using Newtonsoft.Json;
using Monopoly.Lobby;
using Monopoly.Rooms;
using System.Collections.ObjectModel;
using Monopoly.MonopolyGame.Model;
using Monopoly.Intro;
using Monopoly.MatchHistory;
using System.IO;

namespace Monopoly.Communication
{  
    class Query
    {
        private static Communication cm;
        public Query()
        {
            if(cm==null)
            cm = new Communication(ReadServerFile());
        }
        public static List<ServerBuild> GetListOfSevers()
        {
           return cm.checkServers();
        }
        private static void readBuildString(string str, string match, ServerBuild serverBuild)
        {
            if (str.Contains(match))
            {
                switch(match)
                {
                    case "server-name=":
                        serverBuild.Name = str.Split('=')[1];
                        break;
                    case "server-ip=":
                        serverBuild.IP = str.Split('=')[1];
                        break;
                    case "server-port=":
                        serverBuild.Port = Convert.ToInt32(str.Split('=')[1]);
                        break;
                }
            }
        }
        public static List<ServerBuild> ReadServerFile()
        {
            List<ServerBuild> serverBuilds = null;
            if (File.Exists(@"server.config"))
            {
                using (StreamReader sr = new StreamReader(@"server.config"))
                {
                    //precte
                    serverBuilds = new List<ServerBuild>();
                    string s;
                    while ((s = sr.ReadLine()) != null)
                    {
                        ServerBuild serverBuild = new ServerBuild();
                        readBuildString(s, "server-name=", serverBuild);
                        s = sr.ReadLine();
                        if(s!=null)
                            readBuildString(s, "server-ip=", serverBuild);
                        s = sr.ReadLine();
                        if (s != null)
                            readBuildString(s, "server-port=", serverBuild);
                        if(serverBuild.Name!=null && serverBuild.IP != null && serverBuild.Port != 0)
                        {
                            serverBuilds.Add(serverBuild);
                        }
                        else 
                        {
                            Console.WriteLine("poskoyenz config");
                        }
                    }
                    }
                }
            else
            {
                using (StreamWriter sw = new StreamWriter(@"server.config"))
                {
                    sw.WriteLine("server-name=Amazon");
                    sw.WriteLine("server-ip=3.139.100.251");
                    sw.WriteLine("server-port=2050");
                    sw.WriteLine("server-name=Local");
                    sw.WriteLine("server-ip=" + System.Net.Dns.GetHostName());
                    sw.WriteLine("server-port=2050");
                    sw.Flush();
                }
            }
            return serverBuilds;
        }
        public static string GetLobbyName()
        {
            for (int i = 0; i < Data.room.Lobbies.Count; i++)
                if (Data.ThisPlayer.IDLobby == Data.room.Lobbies[i].IDLobby)
                    return Data.room.Lobbies[i].Name;
            return null;
        }

        internal static bool JoinToServer(ServerBuild serverBuild)
        {
            return cm.JoinToServer(serverBuild);
        }
        
        internal static Player GetPlayerOnTurn(bool update= false)
        {
            Lobby.Lobby thisLobby = Query.GetActualLobby(update);
            for(int i=0;i<thisLobby.players.Length;i++)
            {
                if(thisLobby.players[i] != null && thisLobby.players[i].IsOnTurn)
                {
                    return thisLobby.players[i];
                }
            }
            return null;
            //string s = $"ahoj {thisLobby.players[0].Nick} a takz ";
            //Console.WriteLine("ahoj {0}", thisLobby.players[0].Nick);
        }
        public static void UpdateThisPlayerInActualLobby()
        {
            Lobby.Lobby lob = GetThisLobby();
            for (int i = 0; i < lob.players.Length; i++)
            {
                if (lob.players[i] != null)
                {
                    if (lob.players[i].IDPlayer == Data.ThisPlayer.IDPlayer)
                    {
                        Data.ThisPlayer = lob.players[i];
                    }
                }
            }
        }

        internal static void GameEndTurn(Player playerOnMove)
        {
            cm.SendData(SENDING_CODES.endTurn, playerOnMove);
        }
        public static void SendMessage(Chat.Chat chat)
        {
            cm.SendData(SENDING_CODES.message, chat);
        }
        public static void LeaveFromGame(Player player)
        {
            cm.SendData(SENDING_CODES.leaveFromGame, player);
        }
        public static void GetListOfStatistics()
        {
            cm.SendData(SENDING_CODES.basicStatistics, Data.ThisPlayer.IDPlayer);
        }
        public static void GetListOfDetailedStatistics(BasicStatistics basicStatistics)
        {
            cm.SendData(SENDING_CODES.detailedStatistics, basicStatistics);
        }
        //public static Lobby.Lobby GetLobby() //vraci referenci pole hracu v loby
        //{
        //    UpdateLobbies();

        //    List<Player> pls = new List<Player>();
        //    for(int i=0; i<Data.room.Lobbies.Count;i++)
        //    {
        //        if(Data.ThisPlayer.IDLobby == Data.room.Lobbies[i].IDLobby)
        //        {
        //            return Data.room.Lobbies[i];
        //        }
        //    }
        //    return null;
        //}
        public static Lobby.Lobby GetActualLobby(bool update=true)
        {
            if(update)
            cm.SendData(SENDING_CODES.getSpecificLobbyByPlayer, Data.ThisPlayer);
            for (int i = 0; i < Data.room.Lobbies.Count; i++)
            {
                if (Data.ThisPlayer.IDLobby == Data.room.Lobbies[i].IDLobby)
                {
                    return Data.room.Lobbies[i];
                }
            }
            return null;
        }
        public static void GamePlayerLandedState(Player playerOnMove)
        {
            cm.SendData(SENDING_CODES.playerLandedState, playerOnMove);
        }
        public static ObservableCollection<Lobby.Lobby> GetLobbies()
        {
            UpdateLobbies();
            //while (Data.room == null)
            //    System.Threading.Thread.Sleep(20);
            return Data.room.Lobbies;
        }
        public static void UpdateLobbies() 
        {
            cm.SendData(SENDING_CODES.rooms); 
        } //zaslani pozadavku na server o room
        public static void RemovePlayerFromLobby() 
        {
            cm.SendData(SENDING_CODES.removePlFmL, Data.ThisPlayer);
        }
        internal static void PlayerMove(Player player)
        {
            cm.SendData(SENDING_CODES.playerMove, player);
            //UpdateLobbies();
        }
        private static void updatePlayerInClient(Player pl)
        {
           updatePlayerInRoom(pl);
           updatePlayerInLobby(pl);
        }
        private static void updatePlayerInLobby(Player pl)
        {
            for(int i=0;i<Data.room.Lobbies.Count;i++)
            {
                if (Data.room.Lobbies[i].IDLobby == pl.IDLobby)
                {
                    for (int j = 0; j < Data.room.Lobbies[i].players.Length; j++)
                    {
                        if (Data.room.Lobbies[i].players[j] != null
                            && Data.room.Lobbies[i].players[j].IDPlayer == pl.IDPlayer)
                        {
                            Data.room.Lobbies[i].players[j] = pl;
                            return;
                        }
                    }
                }
            }
        }
        private static Player updatePlayerInRoom(Player pl)
        {
            for (int i = 0; i < Data.room.Players.Count; i++)
            {
                if (Data.room.Players[i].IDPlayer == pl.IDPlayer)
                {
                    Data.room.Players[i] = pl;
                    break;
                }
            }
            return null;
        }
        public static void CreateLobby(Lobby.Lobby lobby)
        {
            cm.SendData(SENDING_CODES.createRoom, lobby, Data.ThisPlayer);
            UpdateThisPlayerInRoomList();
            UpdateSpecificLobby(GetThisLobby());
        }
        public static bool JoinLobby(Lobby.Lobby lobby)
        {
            Data.ThisPlayer.IDLobby = lobby.IDLobby;
            cm.SendData(SENDING_CODES.joinRoom, Data.ThisPlayer);
            UpdateThisPlayerInRoomList();
            if (Data.ThisPlayer.IDLobby == Guid.Empty)
                return false;
            else
                return true; //chybi akce
        }
        public static bool PlayerRegistration(User user)
        {
           // Data.ThisPlayer.Nick = nick;
            cm.SendData(SENDING_CODES.registration, user);
            if (Data.user.success)
                return true;
            else
                return false;
        }

        internal static Player FindPlayerByNick(string nick)
        {
            for (int i = 0; i < Data.room.Players.Count; i++)
            {
                if (Data.room.Players[i].Nick == nick)
                {
                    return Data.room.Players[i];
                }
            }
            return null;
        }

        public static bool PlayerLogin(User user)
        {
            cm.SendData(SENDING_CODES.login, user);
            if (Data.user.success)
                return true;
            else
                return false;
        }
        public static void RemovePlayer()
        {
            if (Data.ThisPlayer != null)
            {
                cm.SendData(SENDING_CODES.removePL, Data.ThisPlayer);
                cm.DisconectFromServer();
            }
        }
        public static void DisconectFromServer()
        {
            cm.DisconectFromServer();
        }
        public static Player UpdateThisPlayerInRoomList()
        {
            for (int i = 0; i < Data.room.Players.Count; i++)
            {
                if (Data.room.Players[i].IDPlayer == Data.ThisPlayer.IDPlayer)
                {
                    Data.ThisPlayer = Data.room.Players[i];
                    return Data.ThisPlayer;
                }
            }
            return null;
        }
        public static Player GetThisPlayer()
        {
            UpdateThisPlayerInRoomList();
            return Data.ThisPlayer;
        }
        public static Player GetThisPlayerFromLobby()
        {
          Player pl = UpdateThisPlayerInRoomList();
            for(int i=0;i< Data.room.Lobbies.Count;i++)
            {
                if(pl.IDLobby == Data.room.Lobbies[i].IDLobby)
                {
                    for(int j=0;j< Data.room.Lobbies[i].players.Length;j++)
                    {
                        if(pl.IDPlayer == Data.room.Lobbies[i].players[j].IDPlayer)
                        {
                            return Data.room.Lobbies[i].players[j];
                        }
                    }
                }
            }
            return null;
        }
        public static void UpdateSpecificLobby(Lobby.Lobby lobby)
        {           
            if (lobby != null)
                cm.SendData(SENDING_CODES.updateLobby, lobby);
        }

        internal static void BuyStreet(Player player)
        {
            cm.SendData(SENDING_CODES.buyStreet, player);
        }
        internal static void BuyHouse(Player player)
        {
            cm.SendData(SENDING_CODES.buyHouse, player);
        }
        internal static void EndOfBuying()
        {
            cm.SendData(SENDING_CODES.endBuy, Data.ThisPlayer.IDLobby);
        }
        public static void LaunchGame(Lobby.Lobby lb)
        {
            if (lb != null)
                cm.SendData(SENDING_CODES.launchGame, lb);
        }
        public static void UpdatePlayerInServer(Player pl)
        {
            if (pl != null)
               cm.SendData(SENDING_CODES.updatePlayer, pl);
        }
        public static Lobby.Lobby GetThisLobby()
        {
            for(int i=0;i<Data.room.Lobbies.Count;i++)
            {
                if (Data.ThisPlayer.IDLobby == Data.room.Lobbies[i].IDLobby)
                    return Data.room.Lobbies[i];
            }
            return null;
        }
        public static Player FindPlayerByID(Guid idPlayer)
        {
            Lobby.Lobby lobby = GetThisLobby();
            for(int i=0; i<lobby.players.Length;i++)
            {
                if(lobby.players[i] != null)
                {
                    if(lobby.players[i].IDPlayer == idPlayer)
                    {
                        return lobby.players[i];
                    }
                }
            }
            return null;
        }
        public static bool IsConnectToServer()
        {
            return cm.ConnectToServer;
        }
    }
}
