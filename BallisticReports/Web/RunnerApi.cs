using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using BallisticReports.DataImporters;
using log4net;
using Nancy;
using Nancy.ModelBinding;

namespace BallisticReports.Web
{
    public class RunnerApi : NancyModule
    {
        protected readonly ILog Logger = LogManager.GetLogger(typeof(RunnerApi));

        public RunnerApi(ILifetimeScope scope)
        {
            Post["/api/run/{import}"] = x => RunImport(x.import, scope);
        }

        private object RunImport(string import, ILifetimeScope scope) //don't listen to resharper it'll break everything
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var selectedRunner = assemblies.Select(assembly =>
                    (from t in assembly.GetTypes()
                        where t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(ScheduledImport)) && t.Name.Equals(import, StringComparison.InvariantCultureIgnoreCase)
                        select t)
                    .FirstOrDefault())
                .FirstOrDefault(assemblyImport => assemblyImport != default(Type));

            if (selectedRunner == default(Type))
            {
                var message = $"Cannot run import from API: {import} is not a valid import type";
                Logger.Error(message);
                throw new TypeLoadException(message);
            }

            var dates = this.Bind<ImportDates>();

            var yesterday = DateTime.Today.AddDays(-1);

            if ((dates.StartDate ?? yesterday) > (dates.EndDate ?? yesterday))
            {
                var message = $"Cannot run import from API: Start date must be equal or prior to end date";
                Logger.Error(message);
                throw new ArgumentException(message);
            }

            var r = scope.ResolveNamed<IScheduledImport>(selectedRunner.Name);
            Task.Run(() => r.Import(dates.StartDate ?? yesterday, dates.EndDate ?? yesterday));

            return HttpStatusCode.OK;
        }

        private class ImportDates
        {
            public DateTime? StartDate;
            public DateTime? EndDate;
        }
    }
}
