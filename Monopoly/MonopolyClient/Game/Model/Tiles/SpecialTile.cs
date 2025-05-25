using Monopoly.MonopolyGame.Model.Interfaces;

namespace Monopoly.MonopolyGame.Model.Tiles
{
    class SpecialTile : Tile, ITile
    {
        public SpecialTile(int index, string name) : base(index, name)
        {

        }

        public override string ActOnPlayer(Player player)
        {
            if(this.Index == 0)
            {
                player.IncrementMoney(200);//mozna to zde nema byt
                return "Zdoláváš GO. \nBanka ti věnuje 200 $!";
            }
            else if(this.Index==10)
            {
                return "Jsi na návštěvě svého\n drahého přítele ve vězení.";
            }else if(this.Index == 20)
            {
                return "Skončil si na Free Parking. \n nic se neděje.";
            }else
            {
                return "Jsi ve vězení! Následující tři tahy vynecháváš.";
            }
        }
    }
}
