using Newtonsoft.Json;

namespace BallisticReports.DataModel
{
    public partial class League
    {
        [JsonProperty("Name")]
        public string Name { get; set; }
    }
}
