using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo4j.Driver.V1;

namespace BallisticReports.Neo4J
{
    public static partial class RunnerExtensions
    {
        /// <summary>
        /// Executes a Cypher query and converts the results using a custom converter
        /// </summary>
        public static async Task<IEnumerable<TResult>> Query<TResult>(this IStatementRunner runner, string query, object parameters, Func<IRecord, TResult> convert)
        {
            var cursor = await runner.RunAsync(query, parameters);

            var records = await cursor.ToListAsync();

            return records.Select(convert);
        }

        public static Task<IEnumerable<T1>> Query<T1>(this IStatementRunner runner, string query, object parameters = null)
        {
            return runner.Query(query, parameters, new RecordConverter().Convert<T1>);
        }

        public static Task<IEnumerable<(T1, T2)>> Query<T1, T2>(this IStatementRunner runner, string query, object parameters = null)
        {
            return runner.Query(query, parameters, new RecordConverter().Convert<T1, T2>);
        }

        public static Task<IEnumerable<(T1, T2, T3)>> Query<T1, T2, T3>(this IStatementRunner runner, string query, object parameters = null)
        {
            return runner.Query(query, parameters, new RecordConverter().Convert<T1, T2, T3>);
        }

        public static Task<IEnumerable<(T1, T2, T3, T4)>> Query<T1, T2, T3, T4>(this IStatementRunner runner, string query, object parameters = null)
        {
            return runner.Query(query, parameters, new RecordConverter().Convert<T1, T2, T3, T4>);
        }

        public static Task<IEnumerable<(T1, T2, T3, T4, T5)>> Query<T1, T2, T3, T4, T5>(this IStatementRunner runner, string query, object parameters = null)
        {
            return runner.Query(query, parameters, new RecordConverter().Convert<T1, T2, T3, T4, T5>);
        }
    }
}
