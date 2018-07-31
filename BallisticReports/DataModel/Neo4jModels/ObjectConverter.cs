using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BallisticReports.DataModel.Neo4jModels
{
    public static class ObjectConverter
    {
        public static GameEvent GetEvent(GameOdds gameOdds) => new GameEvent
        {
            Id = gameOdds.Id,
            HomeTeam = gameOdds.HomeTeam ?? string.Empty,
            AwayTeam = gameOdds.AwayTeam ?? string.Empty,
            Sport = gameOdds.Sport,
            MatchTime = gameOdds.MatchTime.ToString(),
            Details = gameOdds.Details ?? string.Empty,
            HomePitcher = gameOdds.HomePitcher ?? string.Empty,
            AwayPitcher = gameOdds.AwayPitcher ?? string.Empty,
            HomeRot = gameOdds.HomeRot ?? string.Empty,
            AwayRot = gameOdds.AwayRot ?? string.Empty,
            League = gameOdds.League?.Name ?? string.Empty,
            DisplayLeague = gameOdds.DisplayLeague ?? string.Empty
        };

        public static EventOdd GetEventOdd(Odd odd) => new EventOdd
        {
            Id = odd.Id,
            EventId = odd.EventId,
            MoneyLineAway = odd.MoneyLineAway ?? string.Empty,
            MoneyLineHome = odd.MoneyLineHome ?? string.Empty,
            OverLine = odd.OverLine ?? string.Empty,
            TotalNumber = odd.TotalNumber ?? string.Empty,
            UnderLine = odd.UnderLine ?? string.Empty,
            PointSpreadAway = odd.PointSpreadAway ?? string.Empty,
            PointSpreadHome = odd.PointSpreadHome ?? string.Empty,
            PointSpreadAwayLine = odd.PointSpreadAwayLine ?? string.Empty,
            PointSpreadHomeLine = odd.PointSpreadHomeLine ?? string.Empty,
            DrawLine = odd.DrawLine ?? string.Empty,
            SiteId = odd.SiteId,
            LastUpdated = odd.LastUpdated.ToString(),
            Participant = odd.Participant?.Name ?? string.Empty
        };
    }
}
