using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BallisticReports.Neo4J.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public sealed class NodeLabelAttribute : Attribute
    {
        public string[] Labels { get; }
        public NodeLabelAttribute(params string[] labels)
        {
            Labels = labels;
        }
    }
}
