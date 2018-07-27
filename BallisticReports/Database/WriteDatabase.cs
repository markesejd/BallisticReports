using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BallisticReports.DataModel;
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
                    client.Cypher
                        .Merge($"({game.Id}):Game {{ Id: id }}")
                        .OnCreate()
                        .Set($"game = {{game}}")
                        .WithParams(new
                        {
                            id = game.Id,
                            game
                        })
                        .ExecuteWithoutResults();

                    Logger.Debug($"Importer game: ({game.Id}) to Neo4J");
                }
            }
        }
    }
}
