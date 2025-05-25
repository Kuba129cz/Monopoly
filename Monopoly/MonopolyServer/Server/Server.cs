using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using MonopolyServer.Server.Data;
using MonopolyServer.Board.Tiles;
using System.Net.NetworkInformation;

namespace MonopolyServer.Server
{//tato cast se stara o komunikaci s klienty
   partial class MonopolyServer
    {
        private delegate object MethodForSendData(string desObj);
        private delegate void VoidMethodForSendData(string desObj);
        private static Client client;
        public static List<Board.Board> Boards;
        private static Database.Connection mysqlConnection = null;
        public void Run(string[] args)
        {
            int port = 2050;
            if (args.Length > 0)
            {
                if (args[0].All(char.IsDigit))
                    port = int.Parse(args[0]);
                else
                    Console.WriteLine("Incorrectly entered port number", args[0]);
            }
            client = new Client();
            Boards = new List<Board.Board>();
            TcpListener listener = null;
            mysqlConnection = new Database.Connection();
          //  Board.Board.InitializeBoard();
            client.Room.Lobbies.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(broadcastListOfLobbiesToWaitingPlayers);
            try
            {
                Console.WriteLine(writeTime() + "Server is running at port: {0}", port);
                Console.WriteLine(writeTime() + "To display the built-in help for all commands, type \"/help\"");
                //Thread myCommandLine = new Thread(commandLine);
                //myCommandLine.IsBackground = true;
                //myCommandLine.Start();
                listener = new TcpListener(IPAddress.Any, port);
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(checkingClients), null);
                    ThreadPool.QueueUserWorkItem(new WaitCallback(pingClients), null);
                    ThreadPool.QueueUserWorkItem(new WaitCallback(commandLine), null);
                    listener.Start();
                    while (true)
                    {
                        Console.WriteLine(writeTime() + "Looking for someone to talk to... ");
                        TcpClient newClient = listener.AcceptTcpClient();
                        Console.WriteLine(writeTime() + "Connected to new client.");
                        ThreadPool.QueueUserWorkItem(new WaitCallback(registerClient),
                        newClient);
                    }
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("{0}SocketException: {1}", writeTime(), e);
            }
            finally
            {
                // Shut it down.
                listener.Stop();
            }
        }

        private static string writeTime()
        {
            return DateTime.Now.ToString("[hh:mm:ss] ");
        }
        private void checkingClients(object state)
        {
            try
            {
                do
                {
                  //  Parallel.For(0, client.TcpClients.Count, (int i) =>
                  for(int i=0; i< client.TcpClients.Count; i++)
                      { 
                          if (client.TcpClients[i] != null)
                          {
                              if (!client.TcpClients[i].Connected)
                              {
                                  client.TcpClients.Remove(client.TcpClients[i]);
                                  client.Room.Players.Remove(client.Room.Players[i]);//doufam ze to odstrani toho hrace
                              }else
                              if (client.TcpClients[i].GetStream().DataAvailable)
                              {
                                  ProcessClient(client.TcpClients[i]);
                              }
                          }
                      }//);
                } while (true);
            }catch(Exception ex)
            {
                Console.WriteLine(writeTime() + "Line 89" + ex);
                checkingClients(null);
            }
        }
        private void pingClients(object state)
        {
            try
            {

                do
                {
                    Thread.Sleep(1000);
                   // Parallel.For(0, client.TcpClients.Count, (int i) =>
                   for(int i=0; i<client.TcpClients.Count;i++)
                    {
                        if (client.TcpClients[i] != null)
                        {
                            if (client.TcpClients[i].Connected)
                            {
                                sendObject(new MyPing(), client.TcpClients[i]);
                            }
                        }
                    }//);
                } while (true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(writeTime() + "Line 115" + ex);
                pingClients(null);
            }
        }
        private void broadcastListOfLobbiesToWaitingPlayers(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            try
            {
                for (int i = 0; i < client.TcpClients.Count; i++)
                {
                    if (client.Room.Players[i].IDLobby == Guid.Empty)
                    {
                        sendObject(client.Room.Lobbies, client.TcpClients[i]);
                    }
                }
            }catch(Exception ex)
            {
                Console.WriteLine(writeTime() + ex);
            }
        }
        //private void broadcastDiceThrow(Lobby lobby)
        //{
        //    for (int i = 0; i < client.TcpClients.Count; i++)
        //    {
        //        if (client.Room.Players[i].IDLobby == lobby.IDLobby)
        //        {
        //            sendObject(lobby, client.TcpClients[i]);
        //        }
        //        //    sendObject(room.Lobbies, clients[i].tcpClient);
        //    }
        //}
        private void broadcastActualLobby(Lobby lobby)
        {
            try
            {
                for (int i = 0; i < client.TcpClients.Count; i++)
                {
                    if (client.Room.Players[i].IDLobby == lobby.IDLobby)
                    {
                        sendObject(lobby, client.TcpClients[i]);
                    }
                }
            }catch(Exception ex)
            {
                Console.WriteLine(writeTime() + ex);
            }
        }
        public void broadcastBoard(Board.Board board)
        {
            try
            {
                for (int i = 0; i < client.TcpClients.Count; i++)
                {
                    if (client.Room.Players[i].IDLobby == board.IdLobby)
                    {
                        sendObject(board.allTiles, client.TcpClients[i]);
                    }
                }
            }catch(Exception ex)
            {
                Console.WriteLine(writeTime() + ex);
            }
        }
        //private void broadCastSignalOfEndBuying(Guid idLobby)
        //{
        //    for (int i = 0; i < client.TcpClients.Count; i++)
        //    {
        //        if (client.Room.Players[i].IDLobby == idLobby)
        //        {
        //            sendObject(new { buyed = true }, client.TcpClients[i]) ;
        //        }
        //    }
        //}
        private void broadCastMessageToPlayers(Chat chat) //snad bdue fungovat
        {
            try
            {
                //L - lobby, P - player, R - players in room
                if (chat.marker == 'L')
                    for (int i = 0; i < client.TcpClients.Count; i++)
                        if (client.Room.Players[i].IDLobby == (chat.ID))
                            sendObject(chat, client.TcpClients[i]);
                if (chat.marker == 'P')
                    for (int i = 0; i < client.TcpClients.Count; i++)
                        if (client.Room.Players[i].IDPlayer == chat.ID)
                            sendObject(chat, client.TcpClients[i]);
                if (chat.marker == 'R')
                    for (int i = 0; i < client.TcpClients.Count; i++)
                        if (client.Room.Players[i].IDLobby == Guid.Empty)
                            sendObject(chat, client.TcpClients[i]);
            }catch(Exception ex)
            {
                Console.WriteLine(writeTime() + ex);
            }
        }
        //private void DoIt()
        //{
        //    int lastCountOfLobbies;
        //    for(int i=0;i<clients.Count;i++)
        //    {
        //        if(!clients[i].tcpClient.Connected)
        //        {
        //            //odstran hrace z kolekce
        //        }
        //    }
        //}
        private void registerClient(object newclient)//dochazi k pripojeni noveho hrace na server
        {
            try
            {
                TcpClient myClient = (TcpClient)newclient;
                Player newPlayer = new Player();
                newPlayer.IDLobby = Guid.Empty;
                client.TcpClients.Add(myClient);
                client.Room.Players.Add(newPlayer);
                sendObject(newPlayer, myClient); //send thisPlayer to server
                writeStatistics();
            }
            catch(Exception ex)
            {
                Console.WriteLine(writeTime() + ex);
            }
        }
        private void ProcessClient(object client)
        {
            try {
                TcpClient newClient = (TcpClient)client;
                object resolt = receiveData(newClient);
                string enData = serializeData(resolt);
                sendData(newClient, enData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(writeTime() + "Failure, {0}", ex);
            }
            finally
            {
            }
        }
        private static string serializeData(object obj)
        {
            try
            {
                if (obj is Room)
                    return pack(SENDING_CODES.room, obj);
                if (obj is Lobby)
                    return pack(SENDING_CODES.specificLobby, obj);
                if (obj is Player)
                    return pack(SENDING_CODES.thisPlayer, obj);
                if (obj is ObservableCollection<Lobby>)
                    return pack(SENDING_CODES.ListOfLobbies, obj);
                if (obj is Chat)
                    return pack(SENDING_CODES.message, obj);
                if (obj is InformationText)
                    return pack(SENDING_CODES.informationText, obj);
                if (obj is List<Tile>)
                    return pack(SENDING_CODES.board, obj);
                if (obj is User)
                    return pack(SENDING_CODES.user, obj);
                if (obj is BuyTile)//asi vymazat
                    return pack(SENDING_CODES.buyStreet, obj);
                if (obj is List<BasicStatistics>)
                    return pack(SENDING_CODES.Statistics, obj);
                if (obj is MyPing)
                    return pack(SENDING_CODES.ping, obj);
                if(obj is KickPlayer)
                    return pack(SENDING_CODES.kickPlayer, obj);
                return pack(SENDING_CODES.received, obj);
            }catch(Exception ex)
            {
                Console.WriteLine(writeTime() + ex);
                return null;
            }
        }
        private static string pack(SENDING_CODES code, object obj)
        {
            return JsonConvert.SerializeObject(code, Formatting.Indented) + ";" + JsonConvert.SerializeObject(obj, Formatting.Indented);
        }
        private void sendObject(object obj, TcpClient client)
        {
           string encodedData = serializeData(obj);
            sendData(client, encodedData);
        }
        private object receiveData(TcpClient newClient)
        {
            try
            {
                MethodForSendData methodForSendData = null;
                VoidMethodForSendData voidMethodForSendData = null;
                string desObj = null;
                string request;
                BinaryReader binaryReader = new BinaryReader(newClient.GetStream());
                request = binaryReader.ReadString();
               // Console.WriteLine("request received, {0}", request);

                string code = request;
                if (request.Contains(';'))
                {
                    code = request.MySplit(';');
                    for (int i = code.Length + 1; i < request.Length; i++)
                    {
                        desObj += request[i];
                    }
                }
                RECEIVING_CODES key = JsonConvert.DeserializeObject<RECEIVING_CODES>(code);
                switch (key)
                {
                    case RECEIVING_CODES.registration:
                        methodForSendData = getRegistration;
                        break;
                    case RECEIVING_CODES.rooms:
                        methodForSendData = getLobbies;
                        break;
                    case RECEIVING_CODES.createRoom:
                        methodForSendData = getCreateLobby;
                        break;
                    case RECEIVING_CODES.joinRoom:
                        methodForSendData = getJoinLobby;
                        break;
                    case RECEIVING_CODES.removePL:
                        methodForSendData = getRemovePlayer;
                        break;
                    case RECEIVING_CODES.removePlFmL:
                        methodForSendData = getRemovePlayerFromLobby;
                        break;
                    case RECEIVING_CODES.launchGame:
                        methodForSendData = getLaunchGame;
                        break;
                    case RECEIVING_CODES.updatePlayer:
                        methodForSendData = getUpdatePlayer;
                        break;
                    case RECEIVING_CODES.updateLobby:
                        methodForSendData = getUpdateLobby;
                        break;
                    case RECEIVING_CODES.playerMove:
                        methodForSendData = playerMove;
                        break;
                    case RECEIVING_CODES.getSpecificLobbyByPlayer:
                        methodForSendData = getSpecificLobbyByPlayer;
                        break;
                    case RECEIVING_CODES.message:
                        methodForSendData = sendMessageToPlayers;
                        break;
                    case RECEIVING_CODES.informationText:
                        methodForSendData = sendInformatinTextToPlayers;
                        break;
                    case RECEIVING_CODES.playerLandedState:
                        methodForSendData = playerLandedState;
                        break;
                    case RECEIVING_CODES.endTurn:
                        methodForSendData = playerEndState;
                        break;
                    case RECEIVING_CODES.buyStreet:
                        methodForSendData = buyStreet;
                        break;
                    case RECEIVING_CODES.endBuy:
                        methodForSendData = EndBuying;
                        break;
                    case RECEIVING_CODES.buyHouse:
                        methodForSendData = buyHouse;
                        break;
                    case RECEIVING_CODES.login:
                        methodForSendData = loginPlayer;
                        break;
                    case RECEIVING_CODES.leaveFromGame:
                        methodForSendData = leaveFromGame;
                        break;
                    case RECEIVING_CODES.basicStatistics:
                        methodForSendData = GetBasicStatistics;
                        break;
                    case RECEIVING_CODES.detailedStatistics:
                        methodForSendData = GetDetailedStatistics;
                        break;
                        //case RECEIVING_CODES.taxTenProcent:
                        //    methodForSendData = taxTenProcent;
                        //    break;
                        //case RECEIVING_CODES.taxTwoHundred:
                        //    methodForSendData = taxTwoHundred;
                        //    break;
                        //case RECEIVING_CODES.tax:
                        //    methodForSendData = tax;
                        //    break;
                }
                //Console.WriteLine("Data came successfuly to server.");
                //autoResetEventSlim.Reset();
                return methodForSendData.Invoke(desObj);
                    //broadcastListOfLobbiesToWaitingPlayers();
            }
            catch(Exception ex)
            {
                Console.WriteLine(writeTime() + "In method receiveData An error has occurred." + ex);
                return null;
            }
        }

        private object sendInformatinTextToPlayers(string desObj)
        {
            try
            {
                InformationText infText = JsonConvert.DeserializeObject<InformationText>(desObj);
                broadCastInformationTextToPlayers(infText);
                return null;
            }catch(Exception ex)
            {
                Console.WriteLine(writeTime() + ex);
                return null;
            }
        }

        private void broadCastInformationTextToPlayers(InformationText infText)
        {
            try
            {
                for (int i = 0; i < client.TcpClients.Count; i++)
                {
                    if (client.Room.Players[i].IDLobby == infText.Lobby.IDLobby)
                    {
                        sendObject(infText, client.TcpClients[i]);
                    }
                }
            }catch(Exception ex)
            {
                Console.WriteLine(writeTime() + ex);
            }
        }
        private void UpdatePlayerInClient(Guid IDPlayer)
        {
            try
            {
                for (int i = 0; i < client.TcpClients.Count; i++)
                {
                    if (client.Room.Players[i].IDPlayer == IDPlayer)
                    {
                        sendObject(client.Room.Players[i], client.TcpClients[i]);
                    }
                }
            }catch(Exception ex)
            {
                Console.WriteLine(writeTime() + ex);
            }
        }
        private void broadcastBuyStreet(BuyTile buyStreet)
        {
            try
            {
                for (int i = 0; i < client.TcpClients.Count; i++)
                {
                    if (client.Room.Players[i].IDLobby == buyStreet.IdLobby)
                    {
                        sendObject(buyStreet, client.TcpClients[i]);
                    }
                }
            }catch(Exception ex)
            {
                Console.WriteLine(writeTime() + ex);
            }
        }
        private object sendMessageToPlayers(string desObj)
        {
            try
            {
                Chat chat = JsonConvert.DeserializeObject<Chat>(desObj);
                broadCastMessageToPlayers(chat);
                return null;
            }catch(Exception ex)
            {
                Console.WriteLine(writeTime() + ex);
                return null;
            }
        }

        private void sendData(TcpClient newClient, string data)
        {
            try
            {
                if (newClient.Connected)
                {
                    BinaryWriter binaryWriter = new BinaryWriter(newClient.GetStream());
                    binaryWriter.Write(data);
                    binaryWriter.Flush();
                }
                //Console.WriteLine("Data were successfuly sended.");
            }catch
            {
                // Console.WriteLine("an error occurred while sending data, disconnecting client" + ex); 
                Console.WriteLine(writeTime() + "Client has disconnected.");
                for (int i = 0; i < client.TcpClients.Count; i++)
                {
                    if (client.TcpClients[i] == newClient)
                    {
                        client.TcpClients.Remove(client.TcpClients[i]);
                        client.Room.Players.Remove(client.Room.Players[i]);//doufam ze to odstrani toho hrace
                        checkLobbies();
                    }
                }
            }
    }

        private void checkLobbies()
        {
            try
            {
                for (int i = 0; i < client.Room.Lobbies.Count; i++)
                {
                    bool isActive = false;
                    for (int j = 0; j < client.Room.Lobbies[i].players.Length; j++)
                    {
                        if (client.Room.Lobbies[i].players[j] != null)
                        {
                            Player player = FindPlayerByHisID(client.Room.Lobbies[i].players[j].IDPlayer);
                            if (player != null)
                            {
                                if(player.IDLobby == client.Room.Lobbies[i].players[j].IDLobby)
                                {
                                    isActive = true;
                                }
                            }
                        }
                    }
                    if(!isActive)
                    {
                        client.Room.Lobbies.Remove(client.Room.Lobbies[i]);
                        Console.WriteLine(writeTime() + "{0} was remove.", client.Room.Lobbies[i].Name);
                    }
                }
            }
                catch(Exception ex) 
        {
        Console.WriteLine(writeTime() + "checkLobbies {0}", ex); 
            }
        }

        private static object getUpdatePlayer(string desObj)
        {
            try
            {
                Player pl = JsonConvert.DeserializeObject<Player>(desObj);
                Player player = findPlayer(pl);
                player = pl;
                //return JsonConvert.SerializeObject(null, Formatting.Indented);
                return null;
            }catch(Exception ex)
            {
                Console.WriteLine(writeTime() + ex);
                return null;
            }
        }
        private static Player findPlayer(Player player)
        {
            try
            {
                if (player != null)
                {
                    for (int i = 0; i < client.Room.Players.Count; i++)
                    {
                        if (client.Room.Players[i].IDPlayer == player.IDPlayer)
                            return client.Room.Players[i];
                    }
                }
                return null;
            }catch(Exception ex)
            {
                Console.WriteLine(writeTime() + ex);
                return null;
            }
        }
        private static Player findPlayerByHisNick(string nickOfPlayer)
        {
            try
            {
                for (int i = 0; i < client.Room.Players.Count; i++)
                {
                    if (client.Room.Players[i].Nick == nickOfPlayer)
                        return client.Room.Players[i];
                }
            return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(writeTime() + ex);
                return null;
            }
        }
        public static Lobby FindLobby(Guid idLobby)
        {
            try
            {
                for (int i = 0; i < client.Room.Lobbies.Count; i++)
                {
                    if (client.Room.Lobbies[i].IDLobby == idLobby)
                        return client.Room.Lobbies[i];
                }
                return null;
            }catch(Exception ex)
            {
                Console.WriteLine(writeTime() + ex);
                return null;
            }
        }
        private void DeleteLobby(Guid idLobby)
        {
            try
            {
                for (int i = 0; i < client.Room.Lobbies.Count; i++)
                {
                    if (client.Room.Lobbies[i].IDLobby == idLobby)
                        client.Room.Lobbies.Remove(client.Room.Lobbies[i]);
                }
                writeStatistics();
            }
            catch(Exception ex)
            {
                Console.WriteLine(writeTime() + ex);
            }
        }
        private static object getRegistration(string desObj)
        {
            try
            {
                User user = JsonConvert.DeserializeObject<User>(desObj);
                bool isNickFree = mysqlConnection.IsNickFree(user.Nick);
                if (isNickFree)
                {
                    mysqlConnection.RegisterNewPlayer(user);
                    user.success = true;
                    user.info = "Registrace byla úspěšně provedena.";
                    Console.WriteLine(writeTime() + "New player {0} was registered.", user.Nick);
                }
                else
                {
                    user.success = false;
                    user.info = "Tento nick již používá někdo jiný.";
                }
                // Player player = JsonConvert.DeserializeObject<Player>(desObj); //posle player
                //for (int i = 0; i < client.Room.Players.Count; i++)
                //    if (player.Nick == client.Room.Players[i].Nick)
                //    {
                //        player.Nick = string.Empty;
                //        return player;
                //    }
                //updatePlayerInRoom(player);
                //// return JsonConvert.SerializeObject(pl, Formatting.Indented);
                //return player;
                return user;
            }catch(Exception ex)
            {
                Console.WriteLine(writeTime() + ex);
                return null;
            }
        }
        private object loginPlayer(string desObj)
        {
            try
            {
                User user = JsonConvert.DeserializeObject<User>(desObj);
                for (int i = 0; i < client.Room.Players.Count; i++)
                    if (user.Nick == client.Room.Players[i].Nick)
                    {
                        user.success = false;
                        user.info = "Tento hráč je již přihlášen.";
                        return user;
                    }
                if (mysqlConnection.LoginPlayer(user))
                {
                    user.success = true;
                    user.info = "Přihlášení proběhlo úspěšně.";
                    Console.WriteLine(writeTime() + "Player {0} logged in.", user.Nick);
                    UpdatePlayerInClient(user.IDPlayer);
                    for (int i = 0; i < client.Room.Players.Count; i++)
                        if (user.IDPlayer == client.Room.Players[i].IDPlayer)
                        {
                            client.Room.Players[i].Nick = user.Nick;
                            break;
                        }
                }
                else
                {
                    user.success = false;
                    user.info = "Nesprávně zadané jméno, nebo heslo.";
                }
                return user;
            }catch(Exception ex)
            {
                Console.WriteLine(writeTime() + ex);
                return null;
            }
        }
        private static object getRemovePlayer(string desObj)
            {
            try
            {
                //for (int i = 0; i < client.TcpClients.Count; i++)
                //{
                //    // if (!client.TcpClients[i].Connected)
                //    client.TcpClients.Remove(client.TcpClients[i]);
                //}
                Player player = JsonConvert.DeserializeObject<Player>(desObj);
                if (player != null)
                {
                    for (int i = 0; i < client.Room.Players.Count; i++)
                    {
                        if (player.IDPlayer == client.Room.Players[i].IDPlayer)
                        {
                            client.TcpClients.Remove(client.TcpClients[i]);//odhlasi konkretniho hrace
                            for (int j = 0; j < client.Room.Lobbies.Count; j++)
                            {
                                if (client.Room.Players[i].IDLobby == client.Room.Lobbies[j].IDLobby)
                                {
                                    client.Room.Lobbies[j].RemovePlayer(player);
                                    if (client.Room.Lobbies[j].NumberOfPlayers() == 0)
                                    {
                                        Console.WriteLine(writeTime() + "Removing lobby: {0}", client.Room.Lobbies[j]);
                                        client.Room.RemoveLobby(client.Room.Lobbies[j]);
                                        break;
                                    }
                                }
                            }
                            client.Room.RemovePlayer(client.Room.Players[i]);
                            if (client.Room.Players[i] != null && player.IDPlayer == client.Room.Players[i].IDPlayer)
                                client.Room.Players.Remove(client.Room.Players[i]);
                        }
                    }
                    writeStatistics();
                }
                Console.WriteLine(writeTime() + "Player: {0} disconnect.", player.Nick);
                // return JsonConvert.SerializeObject(null, Formatting.None);
                //broadCastListOfLobbiesToWaitingPlayers();
                return null;
            }catch(Exception ex)
            {
                Console.WriteLine(writeTime() + ex);
                return null;
            }
            }
        public static Player FindPlayerByHisID(Guid idPlayer)
        {
            try
            {
                for (int i = 0; i < client.Room.Players.Count; i++)
                {
                    if (client.Room.Players[i].IDPlayer == idPlayer)
                        return client.Room.Players[i];
                }
                return null;
            }catch(Exception ex)
            {
                Console.WriteLine(writeTime() + ex);
                return null;
            }
        }
        private void kickPlayer(string nickOfPlayer)
        {
            Player player = findPlayerByHisNick(nickOfPlayer);
            if (player != null)
            {
                try
                {
                    for (int i = 0; i < client.TcpClients.Count; i++)
                    {
                        if (client.Room.Players[i].IDPlayer == player.IDPlayer)
                        {
                            sendObject(new KickPlayer(), client.TcpClients[i]);
                        }
                    }
                    getRemovePlayer(JsonConvert.SerializeObject(player));
                    writeStatistics();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(writeTime() + ex);
                }
            }
            else
                Console.WriteLine("This player does not exist on this server.");
        }
        private static void writeStatistics()
        {
            using (StreamWriter sw = new StreamWriter(@"statistics.txt", true))
            {
                sw.WriteLine("{0}; Number of players on the server: {1}; Number of lobbies: {2}", DateTime.Now, client.Room.Players.Count, client.Room.Lobbies.Count);
                sw.Flush();
            }
        }
        //private object sendStat(string desObj)
        //{
        //    Player player = JsonConvert.DeserializeObject<Player>(desObj);
        //    return null; //todo
        //}
        }
    }
