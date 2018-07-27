using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BallisticReports.DataImporters
{
    public interface IScheduledImport
    {
        void Start();
        void Stop();
        Task Import(DateTime startDate, DateTime endDate);
    }
}
