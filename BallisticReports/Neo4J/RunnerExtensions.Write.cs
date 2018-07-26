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
        public static async Task Write(this IStatementRunner runner, string query, object parameters)
        {
            await runner.RunAsync(query, parameters);
        }

        public static async Task Write(this IStatementRunner runner, string query, IDictionary<string, object> parameters)
        {
            await runner.RunAsync(query, parameters);
        }
        public static async Task Write<T>(this IStatementRunner runner, T entity, IEnumerable<ICustomPropertySerializer> customSerializers = null)
        {
            var nodeDefinition = NodeDefinitionBuilder.GetNodeDefinition(entity.GetType());
            var labelsClause = nodeDefinition.Labels.Aggregate(string.Empty, (prev, label) => prev + $":`{label.Replace("`", "``")}`");
            var propertiesClause = string.Join(",", nodeDefinition.Properties.Select(property => $"`{property.Name.Replace("`", "``")}`: ${property.OriginalMemberName}"));
            var query = $"CREATE(o{labelsClause}{{{propertiesClause}}})";
            await runner.Write(query, nodeDefinition.ToDictionary(entity, customSerializers));

        }

        public static async Task WriteAll<T>(this IStatementRunner runner, IEnumerable<T> enumerable, IEnumerable<ICustomPropertySerializer> customSerializers = null)
        {
            var customSerializersArray = customSerializers?.ToArray();
            foreach (var entity in enumerable)
            {
                await runner.Write(entity, customSerializersArray);
            }
        }
    }
}
