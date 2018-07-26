namespace BallisticReports.Data
{
    using Newtonsoft.Json;

    public partial class League
    {
        [JsonProperty("Name")]
        public string Name { get; set; }
    }
}
