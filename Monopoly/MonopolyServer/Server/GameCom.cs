using MonopolyServer.Board.Tiles;
using MonopolyServer.Board;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonopolyServer.Server.Data;

namespace MonopolyServer.Server
{
    public partial class MonopolyServer
    {
       private Dictionary<Pawn, string> dictionaryOfPawn;
        private object getLaunchGame(string desObj)
        {
            Lobby lobby = JsonConvert.DeserializeObject<Lobby>(desObj);
            lobby = FindLobby(lobby.IDLobby);
            lobby.isInGame = true;
            lobby.Master.IsOnTurn = true;

            List<Pawn> freePawns = new List<Pawn> { Pawn.blue, Pawn.green, Pawn.red, Pawn.yellow };
            freePawns = freePawns.OrderBy(x => (new Random().Next(0, 4))).ToList();
            for (int i = 0; i < lobby.players.Length; i++)
            {
                if (lobby.players[i] != null)
                    lobby.players[i].Pawn = freePawns[i];
            }

            dictionaryOfPawn = new Dictionary<Pawn, string>();//for translate to cz
            dictionaryOfPawn.Add(Pawn.blue, "Modrá");
            dictionaryOfPawn.Add(Pawn.green, "Zelená");
            dictionaryOfPawn.Add(Pawn.red, "Červená");
            dictionaryOfPawn.Add(Pawn.yellow, "Žlutá");

            broadcastActualLobby(lobby);
            AddInformationText(lobby,
                string.Format("{0} figurka hráče {1} je na tahu.",dictionaryOfPawn[lobby.Master.Pawn], lobby.Master.Nick));

            Boards.Add(new Board.Board(lobby.IDLobby));
            mysqlConnection.CreateLobby(lobby);
            Console.WriteLine(writeTime() + "Player {0} launched game {1}.", FindPlayerByHisID(lobby.Master.IDPlayer).Nick, lobby.Name);
            return client.Room;
        }

        public void AddInformationText(Lobby lobby,string v)
        {
            InformationText infText = new InformationText(lobby, v);
            broadCastInformationTextToPlayers(infText);
        }
        private object playerMove(string desObj)
        {
            Player pl = JsonConvert.DeserializeObject<Player>(desObj);
            Player player = findPlayer(pl);

            Lobby lobby = FindLobby(player.IDLobby);
            for(int i=0;i<4;i++)
            {
                if(lobby.players[i]!= null)
                {
                    lobby.players[i].ShouldPlayerMove = false;
                }
            }
            player.ShouldPlayerMove = true;

                player.whiteDiceNumber = new Random().Next(1, 7);
                player.blackDiceNumber = new Random().Next(1, 7);

            int totalPositionsToMove = player.whiteDiceNumber + player.blackDiceNumber;
            player.SetPosition(totalPositionsToMove + player.CurrentPosition);

            AddInformationText(FindLobby(player.IDLobby),
       string.Format("Hráč {0} hodil: {1}.", player.Nick, player.whiteDiceNumber+ player.blackDiceNumber));

            broadcastActualLobby(FindLobby(player.IDLobby));
            return null;
        }
        private object playerLandedState(string desObj)
        {
            Player player = JsonConvert.DeserializeObject<Player>(desObj);
            player = findPlayer(player);
           
            Tile currentTile = FindBoard(player.IDLobby).allTiles[player.CurrentPosition];
                AddInformationText(FindLobby(player.IDLobby),
    string.Format("Hráč {0} se posunul na políčko: {1}.", player.Nick, currentTile.Name));

                AddInformationText(FindLobby(player.IDLobby), currentTile.ActOnPlayer(player));


                //if (currentTile is SpecialTile) nefunguje tak jak by mělo
                //{
                //    var currentTileAsSpecial = currentTile as SpecialTile;
                //    if (currentTileAsSpecial.Index == 30)
                //    {
                //        player.CurrentPosition = 10;
                //    }
                //}
                broadcastActualLobby(FindLobby(player.IDLobby));
            return null;
        }
        private object playerEndState(string desObj)
        {
            Player player = JsonConvert.DeserializeObject<Player>(desObj);
            player = findPlayer(player);

            AddInformationText(FindLobby(player.IDLobby), string.Format("Hráč {0} ukončil tah.", player.Nick));
            Lobby lobby = FindLobby(player.IDLobby);

            for(int i=0;i<lobby.players.Length; i++)
            {
                if(lobby.players[i] != null && lobby.players[i].IsInJail)
                {
                    lobby.players[i].RoundInJail--;
                    if (lobby.players[i].RoundInJail == 0)
                        lobby.players[i].IsInJail = false;
                }
            }

            lobby.NextPlayerTurn();

            broadcastActualLobby(lobby);

            for (int i = 0; i < lobby.players.Length; i++)
            {
                if (lobby.players[i] != null && lobby.players[i].IsOnTurn == true)
                {
                    AddInformationText(lobby, string.Format("{0} figurka hráče {1} je na tahu.", dictionaryOfPawn[lobby.players[i].Pawn], lobby.players[i].Nick));
                }
            }

            return null;
        }
        private object buyStreet(string desObj) //hrac koupil pozemek na kterem stoji
        {
            Player player = JsonConvert.DeserializeObject<Player>(desObj);
            player = findPlayer(player);

            string nameOfStreet = FindBoard(player.IDLobby).AddTileToPlayer(player); 
            AddInformationText(FindLobby(player.IDLobby), string.Format("Hráč {0} koupil {1}.", player.Nick, nameOfStreet));
            Lobby lobby = FindLobby(player.IDLobby);

            BuyTile buyStreet = new BuyTile(player.IDPlayer, player.Pawn, player.CurrentPosition, lobby.IDLobby);
            broadcastBuyStreet(buyStreet); //muzu odstranit
            broadcastBoard(FindBoard(player.IDLobby));
            return null;
        }
        private object buyHouse(string desObj) //hrac koupil pozemek na kterem stoji
        {
            Player player = JsonConvert.DeserializeObject<Player>(desObj);
            player = findPlayer(player);

            string msg = FindBoard(player.IDLobby).BuildHouse(player);
            AddInformationText(FindLobby(player.IDLobby), msg);

            Lobby lobby = FindLobby(player.IDLobby);
            broadcastBoard(FindBoard(player.IDLobby));
            return null;
        }
        public static Board.Board FindBoard(Guid idLobby)
        {
            for(int i=0; i< Boards.Count; i++)
            {
                if(idLobby == Boards[i].IdLobby)
                {
                    return Boards[i];
                }
            }
            return null;
        }
        private object EndBuying(string desObj)
        {
            Guid idLobby = JsonConvert.DeserializeObject<Guid>(desObj);
            Lobby lobby = FindLobby(idLobby);
            lobby.EndOfBuying = true;
            broadcastActualLobby(lobby);
            return null;
        }
        private object leaveFromGame(string desObj)
        {
            Player player = JsonConvert.DeserializeObject<Player>(desObj);
            player = FindPlayerByHisID(player.IDPlayer);
            if (player != null)
            {
                player.Leave = true;
                Lobby lobby = FindLobby(player.IDLobby);
                if(lobby!=null)
                    AddInformationText(lobby, string.Format("Hráč {0} odešel ze hry.", player.Nick));
                if(player.IsOnTurn)
                    lobby.NextPlayerTurn();
                EndMatch(player);
                broadcastActualLobby(lobby);
            }
            return null;
        }
        public void EndMatch(Player player)
        {
            if (player == null)
                return;
            Lobby lobby = FindLobby(player.IDLobby);
            if (lobby == null)
                return;
            player.TimeInLobby = DateTime.Now;
            mysqlConnection.WriteStatistics(player, lobby);
            for(int i=0; i<lobby.players.Length; i++)
            {
                if (lobby.players[i] != null && lobby.players[i].IDPlayer == player.IDPlayer)
                    lobby.players[i] = null;
            }
            player.IDLobby = Guid.Empty;
            player.CurrentPosition = 0;
            player.IsInJail = false;
            player.IsOnTurn = false;
            player.Leave = false;
            player.Money = 2500;
            broadcastActualLobby(lobby);

            bool endGame = true;
            for (int i = 0; i < lobby.players.Length; i++)
            {
                if (lobby.players[i] != null && lobby.players[i].IDPlayer != player.IDPlayer)
                    endGame = false;
            }
            if (endGame)
                DeleteLobby(lobby.IDLobby);
            //odpojit hrace z lobby a ulozit jeho vysledek do databaze
        }
    }

}
