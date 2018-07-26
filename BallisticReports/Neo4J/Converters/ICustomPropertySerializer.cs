using System;

namespace BallisticReports.Neo4J.Converters
{
    public interface ICustomPropertySerializer
    {
        bool CanSerialize(Type type);
        object Parse(object target);
        object Serialize(object target);
    }
}
