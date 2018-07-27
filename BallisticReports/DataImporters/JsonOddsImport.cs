using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BallisticReports.Database;
using BallisticReports.DataModel;

namespace BallisticReports.DataImporters
{
    public class JsonOddsImport : ScheduledImport
    {
        private const string ApiKey = "d99d357c-6d8e-11e8-91fa-06aae780a1ef";
        private const string BaseUrl = "https://jsonodds.com/api/";
        private IWriteDb _writeDb;

        public  JsonOddsImport(IWriteDb db)
        {
            _writeDb = db; 
        }

        protected override TimeSpan[] TimesToProcess() => new[] { new TimeSpan(17, 08, 0) }; //run right at 10 AM why not

        public override async Task Run(DateTime startDate, DateTime endDate)
        {
            Logger.Debug("Running AdWords Import");

            List<GameOdds> gameOdds = new List<GameOdds>();


            //Get Data From API
            using (HttpClient client = new HttpClient())
            {
                //client.DefaultRequestHeaders.Add("Authorization", "Basic ZGV2OmRldg==");
                //client.DefaultRequestHeaders.Add("x-api-key", ApiKey);
                HttpResponseMessage response = await client.GetAsync("https://jsonodds.com/api/test/odds");
                if (!response.IsSuccessStatusCode) return;
                var jsonString = response.Content.ReadAsStringAsync().Result;
                gameOdds.AddRange(GameOdds.FromJson(jsonString));
            }



        }
    }
}
