using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BallisticReports.Data;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json;

namespace BallisticReports
{
    public class DataImporter
    {
        private IDisposable _webApp;

        public void Start()
        {
            _webApp = WebApp.Start<Startup>("http://localhost:45678");
        }

        public void Stop()
        {
            _webApp?.Dispose();
        }
    }
}
