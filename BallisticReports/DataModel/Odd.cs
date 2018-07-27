using System;
using Newtonsoft.Json;

namespace BallisticReports.DataModel
{
    public partial class Odd
    {
        [JsonProperty("ID")]
        public string Id { get; set; }

        [JsonProperty("EventID")]
        public string EventId { get; set; }

        [JsonProperty("OddType")]
        public OddType OddType { get; set; }

        [JsonProperty("MoneyLineAway")]
        public string MoneyLineAway { get; set; }

        [JsonProperty("MoneyLineHome")]
        public string MoneyLineHome { get; set; }

        [JsonProperty("OverLine")]
        public string OverLine { get; set; }

        [JsonProperty("TotalNumber")]
        public string TotalNumber { get; set; }

        [JsonProperty("UnderLine")]
        public string UnderLine { get; set; }

        [JsonProperty("PointSpreadAway")]
        public string PointSpreadAway { get; set; }

        [JsonProperty("PointSpreadHome")]
        public string PointSpreadHome { get; set; }

        [JsonProperty("PointSpreadAwayLine")]
        public string PointSpreadAwayLine { get; set; }

        [JsonProperty("PointSpreadHomeLine")]
        public string PointSpreadHomeLine { get; set; }

        [JsonProperty("DrawLine")]
        public string DrawLine { get; set; }

        [JsonProperty("SiteID")]
        public long SiteId { get; set; }

        [JsonProperty("LastUpdated")]
        public DateTimeOffset LastUpdated { get; set; }

        [JsonProperty("Participant", NullValueHandling = NullValueHandling.Ignore)]
        public League Participant { get; set; }
    }
}
