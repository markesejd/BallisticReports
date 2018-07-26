using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BallisticReports.Neo4J.Converters;
using Neo4j.Driver.V1;

namespace BallisticReports.Neo4J
{
    public static partial class RunnerExtensions
    {
        public static async Task Update<T>(this IStatementRunner runner, T entity, IEnumerable<ICustomPropertySerializer> customSerializers = null)
        {
            var nodeDefinition = NodeDefinitionBuilder.GetNodeDefinition(entity.GetType());
            var matchClause = nodeDefinition.Labels.Aggregate(string.Empty, (prev, label) => prev + $":`{label.Replace("`", "``")}`");
            var identifierClause = string.Join(",",
                nodeDefinition.Identifiers.Select(property => $"`{property.Name.Replace("`", "``")}`:${property.OriginalMemberName}"));
            var setClauses = nodeDefinition.NonIdentifiers
                .Aggregate(string.Empty, (prev, property) => prev + $"\r\nSET o.`{property.Name.Replace("`", "``")}` = ${property.OriginalMemberName}");
            var cql = $"MATCH(o{matchClause} {{{identifierClause}}}){setClauses}";
            await runner.Write(cql, nodeDefinition.ToDictionary(entity, customSerializers));
        }
    }
}
