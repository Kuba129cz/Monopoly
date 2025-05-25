using System.Text;
using System.Net.Sockets;
using Newtonsoft.Json;
using System.Threading;
using System.IO;
using System.Net;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Monopoly.Communication
{
    class Communication
    {
        private List<ServerBuild> servers = null;
        public bool ConnectToServer = false;
        private TcpClient client = null;
        //private NetworkStream networkStream = null;
        private ProcessReceivedData processReceivedData;
        private delegate void MethodForReceiveData(string desObj);
        // private const string IP = /*"192.168.1.107";*/ "62.141.28.76";
        //private static string IP = Dns.GetHostName();
        //private const string IP = "3.139.100.251";
        //private const int PORT = 2050;
        //  private Mutex socketLock = new Mutex();
        private AutoResetEvent autoResetEventSlim;
        private Dictionary<SENDING_CODES, RECEIVING_CODES> checking;
        private SENDING_CODES sendedCode;
        public Communication(List<ServerBuild> servers)
        {
            if (servers == null || servers.Count<0)
            {
                throw new Exception("There is avaible no server to connect.");
            }
            this.servers = servers;
            checking = new Dictionary<SENDING_CODES, RECEIVING_CODES>();
            InitializeChecking();
           // JoinToServer();
        }

        private void InitializeChecking()
        {
            checking.Add(SENDING_CODES.startClient, RECEIVING_CODES.thisPlayer);//
            checking.Add(SENDING_CODES.registration, RECEIVING_CODES.user);
            checking.Add(SENDING_CODES.rooms, RECEIVING_CODES.room);
            checking.Add(SENDING_CODES.createRoom, RECEIVING_CODES.room);
            checking.Add(SENDING_CODES.joinRoom, RECEIVING_CODES.room);
            checking.Add(SENDING_CODES.removePL, RECEIVING_CODES.received);
            checking.Add(SENDING_CODES.removePlFmL, RECEIVING_CODES.room);
            checking.Add(SENDING_CODES.launchGame, RECEIVING_CODES.room);
            checking.Add(SENDING_CODES.updatePlayer, RECEIVING_CODES.received);
            checking.Add(SENDING_CODES.updateLobby, RECEIVING_CODES.specificLobby);
            checking.Add(SENDING_CODES.playerMove, RECEIVING_CODES.received);
            checking.Add(SENDING_CODES.getSpecificLobbyByPlayer, RECEIVING_CODES.specificLobby);
            checking.Add(SENDING_CODES.message, RECEIVING_CODES.message);
            checking.Add(SENDING_CODES.informationText, RECEIVING_CODES.informationText);
            checking.Add(SENDING_CODES.playerLandedState, RECEIVING_CODES.received);
            checking.Add(SENDING_CODES.endTurn, RECEIVING_CODES.received);
            checking.Add(SENDING_CODES.buyStreet, RECEIVING_CODES.received);
            checking.Add(SENDING_CODES.endBuy, RECEIVING_CODES.received);
            checking.Add(SENDING_CODES.buyHouse, RECEIVING_CODES.received);
            checking.Add(SENDING_CODES.login, RECEIVING_CODES.user);
            checking.Add(SENDING_CODES.leaveFromGame, RECEIVING_CODES.received);
            checking.Add(SENDING_CODES.basicStatistics, RECEIVING_CODES.Statistics);
            checking.Add(SENDING_CODES.detailedStatistics, RECEIVING_CODES.Statistics);
        }
        public List<ServerBuild> checkServers()
        {
            Parallel.For(0, servers.Count, (int i) =>
            {
                try
                {
                    client = new TcpClient();
                  //  client.ReceiveTimeout=(1500);
                    client.Connect(servers[i].IP, servers[i].Port);
                    servers[i].Online = true;
                    client.Close();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                    servers[i].Online = false;
                }
            });
            //for(int i=0;i<servers.Count;i++)
            //{
            //    if (servers[i].Online)
            //        return i;
            //}
            //return -1;
            return servers;
        }
        private void setFlagJoinServer(bool join, ServerBuild server)
        {
            for(int i=0;i<servers.Count;i++)
            {
                if(server == servers[i])
                {
                    if (join)
                        servers[i].Join = true;
                    else
                        servers[i].Join = false;
                }
            }
        }
        private void setAllServersConnectToFalse()
        {
            for (int i = 0; i < servers.Count; i++)
                servers[i].Join = false;
        }
        public bool JoinToServer(ServerBuild server)
        {
            try
            {
                if(client != null && client.Connected && server!=null)
                {
                    client.Close();
                    setAllServersConnectToFalse();
                }
                processReceivedData = new ProcessReceivedData();
                autoResetEventSlim = new AutoResetEvent(false);
                client = new TcpClient(server.IP, server.Port);
                setFlagJoinServer(true, server);
                Thread thread = new Thread(receiveData);
                thread.IsBackground = true;
                thread.Start();
                ConnectToServer = true;
                autoResetEventSlim.WaitOne();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                //  Program.Game.Exit();
                 Program.Game.ConnectionFailed();
                ConnectToServer = false;
                setAllServersConnectToFalse();
                return false;
            }
        }
        private void receiveData()
        {
            try
            {
                do
                {
                    if (client.GetStream().DataAvailable)
                    {
                        BinaryReader binaryReader = new BinaryReader(client.GetStream());
                        string message = binaryReader.ReadString();
                        decodeMessage(message);
                    }
                    else
                        Thread.Sleep(20);
                } while (client.Connected);
            }
            catch { }
        }

        private void decodeMessage(string message)
        {
            try
            {
                string code = message;
                string desObj = string.Empty;
                if (message.Contains(';'))
                {
                    //code = message.MySplit(';');
                    //for (int i = code.Length + 1; i < message.Length; i++)//ykopiruje message od prvniho ; do konce
                    //{
                    //    desObj += message[i];
                    //}
                    int t = message.IndexOf(';');
                    code = message.Remove(t);
                    desObj = message.Substring(t+1);
                }
                MethodForReceiveData methodForSendData = null;
                RECEIVING_CODES key = JsonConvert.DeserializeObject<RECEIVING_CODES>(code);
                switch (key)
                {
                    case RECEIVING_CODES.room:
                        methodForSendData = processReceivedData.UpdateRoom;
                        break;
                    case RECEIVING_CODES.thisPlayer:
                        methodForSendData = processReceivedData.UpdateThisPlayer;
                        break;
                    case RECEIVING_CODES.specificLobby:
                        methodForSendData = processReceivedData.UpdateSpecificLobby;
                        break;
                    case RECEIVING_CODES.ListOfLobbies:
                        methodForSendData = processReceivedData.UpdateListOfLobbies;
                        break;
                    case RECEIVING_CODES.message:
                        methodForSendData = processReceivedData.WriteMessage;
                        break;
                    case RECEIVING_CODES.informationText:
                        methodForSendData = processReceivedData.WriteInformationText;
                        break;
                    //case RECEIVING_CODES.buyStreet:
                    ////    methodForSendData = processReceivedData.buyStreet;
                    //    break;
                    case RECEIVING_CODES.board:
                        methodForSendData = processReceivedData.updateBoard;
                        break;
                    case RECEIVING_CODES.user:
                        methodForSendData = processReceivedData.user;
                        break;
                    case RECEIVING_CODES.received:
                        break;
                    case RECEIVING_CODES.Statistics:
                        methodForSendData = processReceivedData.ReceivedStatistics;
                        break;
                    case RECEIVING_CODES.ping:
                        methodForSendData = processReceivedData.ReceivedPing;
                        break;
                    case RECEIVING_CODES.kickPlayer:
                        methodForSendData = processReceivedData.KickPlayer;
                        break;
                    default:
                        throw new NotImplementedException("key does not exists");
                }
                if(methodForSendData!=null)
                methodForSendData.Invoke(desObj);
                if (checking[sendedCode] == key)
                {
                    autoResetEventSlim.Set();
                    Latency.ReceiveRequest(key.ToString()); //zjistuje latenci
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public void SendData (SENDING_CODES code, params object[] ob)
        {
            try
            {
                    sendedCode = code;
                    autoResetEventSlim.Reset();
                    string send = JsonConvert.SerializeObject(code, Formatting.Indented);
                    if (ob != null)
                    {
                        for (int i = 0; i < ob.Length; i++)
                            send += ";" + JsonConvert.SerializeObject(ob[i], Formatting.Indented);
                    }
                if (client != null)
                     {
                    lock (this)
                    {
                        BinaryWriter binaryWriter = new BinaryWriter(client.GetStream());
                        binaryWriter.Write(send);
                        binaryWriter.Flush();
                        Latency.SendRequest(code.ToString()); //stopuji latenci mezi odeslanim a prijmutim pozadavku
                        autoResetEventSlim.WaitOne();//cekani
                     }
                    }
            }
            catch
            {
            }
            finally {
                //networkStream.Close();
                //client.Close();
            }
            }

        public void DisconectFromServer()
        {
            try
            {
                if (client != null)
                {
                    client.Close();
                    ConnectToServer = false;
                    setAllServersConnectToFalse();
                }
            }catch
            {

            }
        }
        ~Communication()
        {
            if (client != null)
                client.Close();
            ConnectToServer = false;
        }
    }
}
