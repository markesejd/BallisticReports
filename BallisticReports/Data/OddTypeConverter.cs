namespace BallisticReports.Data
{
    using System;
    using Newtonsoft.Json;

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
            }
            throw new Exception("Cannot marshal type OddType");
        }

        public static readonly OddTypeConverter Singleton = new OddTypeConverter();
    }
}
