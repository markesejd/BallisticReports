using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using Topshelf;

namespace BallisticReports
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<DataImporter>(s =>
                {
                    s.ConstructUsing(name => new DataImporter());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });

                x.RunAsLocalSystem();
                x.SetDescription("Ballistic Reports UI");
                x.SetDisplayName("BallisticReports");
            });
        }
    }
}
