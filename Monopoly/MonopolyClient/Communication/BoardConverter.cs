using Monopoly.Game.Model.Tiles;
using Monopoly.MonopolyGame.Model;
using Monopoly.MonopolyGame.Model.Tiles;
using MonopolyServer.Board.Tiles;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monopoly.Communication
{//SpecialTile - A,  Street - B, ChestCard - C, TAX - D, Train - E, ChanceCard - F, DiceCard - G
    class BoardConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Tile));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            if (jo["Marker"].Value<char>() == 'A')
                return jo.ToObject<SpecialTile>(serializer);

            if (jo["Marker"].Value<char>() == 'B')
                return jo.ToObject<Street>(serializer);

            if (jo["Marker"].Value<char>() == 'C')
                return jo.ToObject<ChestCard>(serializer);

            if (jo["Marker"].Value<char>() == 'D')
                return jo.ToObject<Tax>(serializer);

            if (jo["Marker"].Value<char>() == 'E')
                return jo.ToObject<Train>(serializer);

            if (jo["Marker"].Value<char>() == 'F')
                return jo.ToObject<ChanceCard>(serializer);

            if (jo["Marker"].Value<char>() == 'G')
                return jo.ToObject<DiceCard>(serializer);

            return null;
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
