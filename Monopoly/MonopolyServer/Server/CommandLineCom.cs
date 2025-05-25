using MonopolyServer.Server.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonopolyServer.Server
{
    public partial class MonopolyServer
    {
        private string commandFromConsole = null;
        List<Command> commands;
        private void commandLine(object obj)
        {
            try
            {
                commands = new List<Command>();
                commands.Add(new Command("/help", showHelp, "/help command to show list of commands."));
                commands.Add(new Command("/players", showPlayers, "/players command to show list of players on the server."));
                commands.Add(new Command("/lobbies", showLobbies, "/lobbies command to show list of lobbies on the server."));
                commands.Add(new Command("/kick", comKickPlayer, "/kick <nick of player> command to forcibly disconnect a player from the server."));
                commands.Add(new Command("/exit", exit, "/exit....shut down the server."));

                do
                {
                    commandFromConsole = Console.ReadLine();
                    bool find = false;
                    for(int i=0; i<commands.Count;i++)
                    { 
                        if(commands[i].Com == commandFromConsole.Split()[0])
                        {
                            find = true;
                            commands[i].Fun.Invoke();
                            break;
                        }
                    }
                    if(!find)
                        Console.WriteLine(writeTime() + "'{0}' is not recognized as command. \n All commands you can show with /help", commandFromConsole);
                } while (true);
            }catch(Exception ex)
            {
                Console.WriteLine(writeTime() + "Error" + ex.ToString());
            }
        }

        private void comKickPlayer()
        {
            try
            {
                if(commandFromConsole.Split().Length != 2) //mame jen dva argumenty
                    throw new Exception();
                kickPlayer(commandFromConsole.Split()[1]);
            }catch
            {
                Console.WriteLine(writeTime() + "wrong number of arguments entered!");
            }
            //rozpytvat a pak zavolat tamtu metodu
        }

        private void showLobbies()
        {
            if(client.Room.Lobbies.Count == 0)
                Console.WriteLine(writeTime() + "There are not any lobbies at this current time.");
            else
            for (int i=0; i<client.Room.Lobbies.Count; i++)
            {
                Console.WriteLine(writeTime() + "{0} in game: {1}", client.Room.Lobbies[i].Name, client.Room.Lobbies[i].isInGame);
            }
        }

        private void exit()
        {
            Program.MonopolyServer.exit();
        }

        private void showPlayers()
        {
            if (client.Room.Players.Count == 0)
            {
                Console.WriteLine(writeTime() + "There are not any players at this current time.");
                return;
            }
            if (client.Room.Players.Count <= 1 && client.Room.Players[0].Nick == null)
                Console.WriteLine(writeTime() + "There are not any players at this current time.");
            else
            for (int i=0; i<client.Room.Players.Count; i++)
            {
                if(client.Room.Players[i].Nick!=null)
                Console.WriteLine(writeTime() + "{0}.) {1}", i + 1, client.Room.Players[i].Nick);
            }
        }

        private void showHelp()
        {
            if (commands == null)
                return;
            Console.WriteLine(writeTime() + "List of commands:");
            foreach (var i in commands)
            Console.WriteLine(i.Info);
        }

    }
}
