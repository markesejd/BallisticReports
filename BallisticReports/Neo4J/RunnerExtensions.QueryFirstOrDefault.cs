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
        private static async Task<TResult> QueryFirstOrDefault<TResult>(this IStatementRunner runner, string query, object parameters, Func<IRecord, TResult> convert)
        {
            var cursor = await runner.RunAsync(query, parameters);

            var hasResult = await cursor.FetchAsync();

            if (!hasResult)
            {
                return default(TResult);
            }

            return convert(cursor.Current);
        }

        public static Task<T1> QueryFirstOrDefault<T1>(this IStatementRunner runner, string query, object parameters = null)
        {
            return runner.QueryFirstOrDefault(query, parameters, new RecordConverter().Convert<T1>);
        }

        public static Task<(T1, T2)> QueryFirstOrDefault<T1, T2>(this IStatementRunner runner, string query, object parameters = null)
        {
            return runner.QueryFirstOrDefault(query, parameters, new RecordConverter().Convert<T1, T2>);
        }

        public static Task<(T1, T2, T3)> QueryFirstOrDefault<T1, T2, T3>(this IStatementRunner runner, string query, object parameters = null)
        {
            return runner.QueryFirstOrDefault(query, parameters, new RecordConverter().Convert<T1, T2, T3>);
        }

        public static Task<(T1, T2, T3, T4)> QueryFirstOrDefault<T1, T2, T3, T4>(this IStatementRunner runner, string query, object parameters = null)
        {
            return runner.QueryFirstOrDefault(query, parameters, new RecordConverter().Convert<T1, T2, T3, T4>);
        }

        public static Task<(T1, T2, T3, T4, T5)> QueryFirstOrDefault<T1, T2, T3, T4, T5>(this IStatementRunner runner, string query, object parameters = null)
        {
            return runner.QueryFirstOrDefault(query, parameters, new RecordConverter().Convert<T1, T2, T3, T4, T5>);
        }
    }
}
