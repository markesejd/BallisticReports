using System.Collections.Generic;
using Newtonsoft.Json;

namespace BallisticReports.DataModel
{
    public static class Serialize
    {
        public static string ToJson(this List<GameOdds> self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }
}
