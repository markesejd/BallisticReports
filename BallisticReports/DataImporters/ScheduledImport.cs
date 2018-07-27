using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace BallisticReports.DataImporters
{
    public abstract class ScheduledImport : IScheduledImport
    {
        protected IObservable<long> Observable;
        protected DateTime LastImportDate;
        protected readonly ILog Logger = LogManager.GetLogger(typeof(ScheduledImport));
        private bool _isImporting;

        protected abstract TimeSpan[] TimesToProcess();
        protected string ImportName => GetType().Name;

        protected ScheduledImport()
        {
            _isImporting = false;
        }

        public void Start()
        {
            Logger.Debug($"Starting {ImportName} import timer");
            Observable = Daily(Scheduler.Default, TimesToProcess());
            Observable.Subscribe(async value =>
            {
                var yesterday = DateTime.Now.AddDays(-1).Date;
                await Import(yesterday, yesterday);
            });
        }


        //date range goes here?
        public abstract Task Run(DateTime startDate, DateTime endDate); //override this method with your import-specific implementation logic

        public void Stop()
        {
            Logger.Debug($"Stopping {ImportName} import");
            Observable = null;
        }

        public async Task Import(DateTime startDate, DateTime endDate)
        {
            try
            {
                if (_isImporting)
                {
                    Logger.Error($"Import {ImportName} is already running!");
                    return;
                }

                _isImporting = true;
                Logger.Debug($"Starting import {ImportName}");
                await Run(startDate, endDate);
                Logger.Debug($"Completed import {ImportName}");
            }
            catch (Exception err)
            {
                Logger.Error($"Error running import {ImportName}: {err.Message}", err);
            }
            _isImporting = false;
        }

        protected IObservable<long> Daily(IScheduler scheduler, params TimeSpan[] times)
        {
            if (times.Length == 0)
                return System.Reactive.Linq.Observable.Never<long>();

            // Do not sort in place.
            var sortedTimes = times.ToList();

            sortedTimes.Sort();

            return System.Reactive.Linq.Observable.Defer(() =>
            {
                var now = DateTime.Now;

                var next = sortedTimes.FirstOrDefault(time => now.TimeOfDay < time);

                var nextScheduledImportTime = next > TimeSpan.Zero
                  ? now.Date.Add(next)
                  : now.Date.AddDays(1).Add(sortedTimes[0]);

                LastImportDate = nextScheduledImportTime;
                return System.Reactive.Linq.Observable.Timer(nextScheduledImportTime, scheduler);
            })
              .Repeat();
        }
    }
}
