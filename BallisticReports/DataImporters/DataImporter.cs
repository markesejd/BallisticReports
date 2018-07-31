using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BallisticReports.DataImporters;
using Microsoft.Owin.Hosting;
using Neo4jClient;
using Newtonsoft.Json;

namespace BallisticReports
{
    public class DataImporter
    {
        private readonly IEnumerable<IScheduledImport> _scheduledImports;
        public DataImporter(IEnumerable<IScheduledImport> scheduledImports)
        {
            _scheduledImports = scheduledImports;
        }

        public void Start()
        {
            foreach (var scheduledImport in _scheduledImports)
            {
                scheduledImport.Start();
            }
        }

        public void Stop()
        {
            foreach (var scheduledImport in _scheduledImports)
            {
                scheduledImport.Stop();
            }
        }
    }
}
