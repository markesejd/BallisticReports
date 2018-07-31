using System;
using Newtonsoft.Json;

namespace BallisticReports.DataModel
{
    internal class OddTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(OddType) || t == typeof(OddType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "FirstFiveInnings":
                    return OddType.FirstFiveInnings;
                case "First Half":
                    return OddType.FirstHalf;
                case "First Quarter":
                    return OddType.FirstQuarter;
                case "First Period":
                    return OddType.FirstPeriod;
                case "Game":
                    return OddType.Game;
            }
            throw new Exception("Cannot unmarshal type OddType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (OddType)untypedValue;
            switch (value)
            {
                case OddType.FirstFiveInnings:
                    serializer.Serialize(writer, "FirstFiveInnings");
                    return;
                case OddType.Game:
                    serializer.Serialize(writer, "Game");
                    return;
                case OddType.FirstHalf:
                    serializer.Serialize(writer, "First Half");
                    return;
                case OddType.FirstQuarter:
                    serializer.Serialize(writer, "First Quarter");
                    break;
                case OddType.FirstPeriod:
                    serializer.Serialize(writer, "First Period");
                    break;
            }
            throw new Exception("Cannot marshal type OddType");
        }

        public static readonly OddTypeConverter Singleton = new OddTypeConverter();
    }
}
