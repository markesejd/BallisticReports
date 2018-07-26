namespace BallisticReports.Data
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public static class Serialize
    {
        public static string ToJson(this List<GameOdds> self) => JsonConvert.SerializeObject(self, BallisticReports.Data.Converter.Settings);
    }
}
