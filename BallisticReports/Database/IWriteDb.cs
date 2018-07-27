using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BallisticReports.DataModel;

namespace BallisticReports.Database
{
    public interface IWriteDb
    {
       void UpdateGameOdds(List<GameOdds> games);
    }
}
