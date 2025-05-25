using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyServer.Server
{
    public partial class MonopolyServer
    {
        private static object getLobbies(string des = null)
        {
            return client.Room;
        }
        private object getUpdateLobby(string desObj)
        {
            Lobby lobby = JsonConvert.DeserializeObject<Lobby>(desObj);
            lobby = FindLobby(lobby.IDLobby);
            broadcastActualLobby(lobby);
            return lobby;
        }
        private object getSpecificLobbyByPlayer(string desObj)
        {
            Player player = JsonConvert.DeserializeObject<Player>(desObj);
            if (player.IDLobby != null)
            {
                for (int i = 0; i < client.Room.Lobbies.Count; i++)
                {
                    if (player.IDLobby == client.Room.Lobbies[i].IDLobby)
                        return client.Room.Lobbies[i];
                }
            }
            return null;
        }
        private object getJoinLobby(string desObj)
        {
            Player player = JsonConvert.DeserializeObject<Player>(desObj);
            player.TimeInLobby = DateTime.Now;
            for (int i = 0; i < client.Room.Players.Count; i++)
                if (player.IDPlayer == client.Room.Players[i].IDPlayer)
                {
                    client.Room.Players[i] = player;
                    Console.WriteLine(writeTime() + "Player {0} joined to {1}.", player.Nick, FindLobby(player.IDLobby).Name);
                    break;
                }
            Lobby lobby = null;
            for (int i = 0; i < client.Room.Lobbies.Count; i++)
                if (player.IDLobby == client.Room.Lobbies[i].IDLobby)
                {
                    lobby = client.Room.Lobbies[i];
                    if (!(client.Room.Lobbies[i].AddPlayer(player)))
                        player.IDLobby = Guid.Empty;
                    break;
                }
             if (player.IDLobby != Guid.Empty)
               broadcastActualLobby(lobby); //nemusi se posilat hraci, ktery se pripojuje
            return client.Room;
        }
        private object getCreateLobby(string desObj)
        {
            string[] classes = desObj.Split(';');
            Lobby lobby = JsonConvert.DeserializeObject<Lobby>(classes[0]);
            Player player = JsonConvert.DeserializeObject<Player>(classes[1]);
            Lobby newLobby = new Lobby(lobby.Name); // zalozim novou lobby
                                                    //najdu hrace a soupnu ho do lobby
            for (int i = 0; i < client.Room.Players.Count; i++)
                if (client.Room.Players[i].IDPlayer == player.IDPlayer)
                {
                    client.Room.Players[i].TimeInLobby = DateTime.Now;
                    newLobby.AddPlayer(client.Room.Players[i]);
                    newLobby.Master = client.Room.Players[i];
                    client.Room.Players[i].IDLobby = newLobby.IDLobby;
                    Console.WriteLine(writeTime() + "Player {0} created lobby {1}.", player.Nick, lobby.Name);
                    break;
                }
            client.Room.Lobbies.Add(newLobby);
            broadcastActualLobby(newLobby);

            // return JsonConvert.SerializeObject(room, Formatting.None);
            //broadCastListOfLobbiesToWaitingPlayers();
            return client.Room;
        }
        private object getRemovePlayerFromLobby(string desObj)
        {
            Player player = JsonConvert.DeserializeObject<Player>(desObj);

            for (int i = 0; i < client.Room.Lobbies.Count; i++)
            {
                if (client.Room.Lobbies[i].IDLobby == player.IDLobby)
                {
                    client.Room.Lobbies[i].RemovePlayer(player);
                    if ((client.Room.Lobbies[i].Master = client.Room.Lobbies[i].GetMaster()) == null)
                    {
                        client.Room.Lobbies.Remove(client.Room.Lobbies[i]); //odstrani loby
                    }else
                    {
                        broadcastActualLobby(client.Room.Lobbies[i]);
                    }
                    break;
                }
            }

            for (int i = 0; i < client.Room.Players.Count; i++)
            {
                if (client.Room.Players[i].IDPlayer == player.IDPlayer)
                {
                    client.Room.Players[i].IDLobby = Guid.Empty;
                    client.Room.Players[i].TimeInLobby = default;
                    break;
                }
            }
            // return JsonConvert.SerializeObject(room, Formatting.Indented);
            //broadCastListOfLobbiesToWaitingPlayers();
            return client.Room;
        }
    }
}
