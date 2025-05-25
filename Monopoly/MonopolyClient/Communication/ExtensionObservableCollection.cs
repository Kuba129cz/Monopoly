using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Monopoly.Communication
{
    static class ExtensionObservableCollection
    {
        public static ObservableCollection<Lobby.Lobby> Coppy(this ObservableCollection<Lobby.Lobby> roomLobbies, ObservableCollection<Lobby.Lobby> newRoomLobbies)
        {
            for(int i=0;i<Data.room.Players.Count;i++)
            {
                for(int j=0;j< newRoomLobbies.Count; j++)
                {
                    for(int k=0; k<newRoomLobbies[j].players.Length; k++)
                    {
                        if(newRoomLobbies[j].players[k]!=null)
                        {
                            if(Data.room.Players[i].IDPlayer == newRoomLobbies[j].players[k].IDPlayer)
                            {
                                Data.room.Players[i].CopyPlayer(newRoomLobbies[j].players[k]);
                                newRoomLobbies[j].players[k] = Data.room.Players[i];
                                break;
                            }
                        }
                    }
                   // break;
                }
            }
            return newRoomLobbies;
        }
    }
}
