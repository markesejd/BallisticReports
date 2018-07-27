using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo4j.Driver.V1;

namespace BallisticReports.Database
{
    public class ReadDatabase : IReadDb
    {
        private readonly IDriver _driver;
        public ReadDatabase(IDriver driver)
        {
            _driver = driver;
        }
    }
}
