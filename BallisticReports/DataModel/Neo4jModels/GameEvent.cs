using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BallisticReports.DataModel.Neo4jModels
{
    public class GameEvent
    {
        [JsonProperty("ID")]
        public string Id { get; set; }

        [JsonProperty("HomeTeam")]
        public string HomeTeam { get; set; }

        [JsonProperty("AwayTeam")]
        public string AwayTeam { get; set; }

        [JsonProperty("Sport")]
        public long Sport { get; set; }

        [JsonProperty("MatchTime")]
        public string MatchTime { get; set; }

        [JsonProperty("Details", NullValueHandling = NullValueHandling.Ignore)]
        public string Details { get; set; }

        [JsonProperty("HomePitcher", NullValueHandling = NullValueHandling.Ignore)]
        public string HomePitcher { get; set; }

        [JsonProperty("AwayPitcher", NullValueHandling = NullValueHandling.Ignore)]
        public string AwayPitcher { get; set; }

        [JsonProperty("HomeROT", NullValueHandling = NullValueHandling.Ignore)]
        public string HomeRot { get; set; }

        [JsonProperty("AwayROT", NullValueHandling = NullValueHandling.Ignore)]
        public string AwayRot { get; set; }

        [JsonProperty("League", NullValueHandling = NullValueHandling.Ignore)]
        public string League { get; set; }

        [JsonProperty("DisplayLeague", NullValueHandling = NullValueHandling.Ignore)]
        public string DisplayLeague { get; set; }
    }
}
