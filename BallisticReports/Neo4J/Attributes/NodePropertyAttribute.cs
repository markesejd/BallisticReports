using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BallisticReports.Neo4J.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class NodePropertyAttribute : Attribute
    {
        public string Name { get; }
        public bool Identifier { get; }
        public NodePropertyAttribute(string name = default(string), bool identifier = false)
        {
            Name = name;
            Identifier = identifier;
        }
    }
}
