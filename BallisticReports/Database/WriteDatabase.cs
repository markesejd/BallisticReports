using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BallisticReports.DataModel;
using BallisticReports.DataModel.Neo4jModels;
using log4net;
using Neo4j.Driver.V1;
using Neo4jClient;

namespace BallisticReports.Database
{
    public class WriteDatabase : IWriteDb
    {
        protected readonly ILog Logger = LogManager.GetLogger(typeof(WriteDatabase));
        private readonly IGraphClientFactory _graphClientFactory;
        public WriteDatabase(IGraphClientFactory graphClientFactory)
        {
            _graphClientFactory = graphClientFactory;
        }

        public void UpdateGameOdds(List<GameOdds> games)
        {
            using(var client = _graphClientFactory.Create())
            {
                foreach(var game in games)
                {

                    var gameEvent = ObjectConverter.GetEvent(game);

                    // Add node for game
                    client.Cypher
                        .Merge("(event:Event { Id: {id} })")
                        .OnCreate()
                        .Set("event = {gameEvent}")
                        .WithParams(new
                        {
                            id = gameEvent.Id,
                            gameEvent
                        })
                        .ExecuteWithoutResults();

                    Logger.Info($"Imported game: ({gameEvent.Id}) to Neo4J");


                    foreach (var odd in game.Odds)
                    {
                        var neoOdd = ObjectConverter.GetEventOdd(odd);

                        // Add node for game odd
                        client.Cypher
                            .Merge("(odd:EventOdd { Id: {id} })")
                            .OnCreate()
                            .Set("odd = {neoOdd}")
                            .WithParams(new
                            {
                                id = neoOdd.Id,
                                neoOdd
                            })
                            .ExecuteWithoutResults();


                        // Add 'HasLine' relationship for odd and game
                        client.Cypher
                            .Match("(g:Event)", "(o:EventOdd)")
                            .Where((GameEvent g) => g.Id == gameEvent.Id)
                            .AndWhere((EventOdd o) => o.Id == neoOdd.Id)
                            .CreateUnique("(o)-[:HAS_ODD]->(g)")
                            .ExecuteWithoutResults();
                        

                        Logger.Info($"Imported odd ({neoOdd.Id}) for event ({gameEvent.Id}) to Neo4J");
                    }
                }
            }
        }
    }
}
