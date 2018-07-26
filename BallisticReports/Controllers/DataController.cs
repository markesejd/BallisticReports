using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using BallisticReports.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BallisticReports.Controllers
{
    public class DataController : ApiController
    {
        private const string ApiKey = "d99d357c-6d8e-11e8-91fa-06aae780a1ef";
        private const string BaseUrl = "https://jsonodds.com/api/";

        public List<GameOdds> GetOdds()
        {
            List<GameOdds> gameOdds = new List<GameOdds>();

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Basic ZGV2OmRldg==");
                client.DefaultRequestHeaders.Add("x-api-key", ApiKey);
                HttpResponseMessage response = client.GetAsync(BaseUrl + "odds").Result;
                if (!response.IsSuccessStatusCode) return gameOdds;
                var jsonString = response.Content.ReadAsStringAsync().Result;
                gameOdds.AddRange(GameOdds.FromJson(jsonString));
            }
            
            
            return gameOdds;
        }
    }
}
