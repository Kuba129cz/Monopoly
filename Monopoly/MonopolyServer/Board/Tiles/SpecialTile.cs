using MonopolyServer.Board.Interfaces;
using System;

namespace MonopolyServer.Board.Tiles
{
    class SpecialTile : Tile, ITile
    {
        public SpecialTile(int index, string name, char marker) : base(index, name, marker)
        {

        }

        public override string ActOnPlayer(Player player)
        {
            if(this.Index == 0)
            {
                player.IncrementMoney(200);//mozna to zde nema byt
                return "Zdoláváš GO. Banka ti věnuje 200 $!";
            }
            else if(this.Index==10)
            {
                if (player.IsInJail)
                    return String.Format("Ještě následující {0} kola si v žaláři.", player.RoundInJail);
                else
                return "Jsi na návštěvě svého drahého přítele ve vězení.";
            }else if(this.Index == 20)
            {
                return "Skončil si na Free Parking. Nic se neděje.";
            }else
            {
                //player.IsInJail = true;
                //player.RoundInJail = 3;
                return "Jsi ve vězení! Následující tři tahy vynecháváš.";
            }
        }

        //private void playerIsInJail(Player player)
        //{
        //    if(player.IsInJail == false)
        //    {
        //        player.RoundEnteredToJail = Server.MonopolyServer.FindLobby(player.IDLobby).Round;
        //        player.IsInJail = true;
        //    }
        //    else
        //    {
        //        if(Server.MonopolyServer.FindLobby(player.IDLobby).Round -3 >=player.RoundEnteredToJail)
        //        {
        //            player.IsInJail = false;
        //            player.RoundEnteredToJail = 0;
        //        }
        //    }
        //}
    }
}
