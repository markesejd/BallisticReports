using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BallisticReports.Neo4J.Converters
{
    public static class PropertySerializers
    {
        public static IList<ICustomPropertySerializer> Default { get; set; } = new List<ICustomPropertySerializer>();
    }
}
