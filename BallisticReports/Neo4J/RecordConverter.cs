using System;
using System.Collections.Concurrent;
using Neo4j.Driver.V1;

namespace BallisticReports.Neo4J
{
    internal sealed class RecordConverter
    {
        private readonly ConcurrentDictionary<long, object> _convertedEntities = new ConcurrentDictionary<long, object>();
        public T1 Convert<T1>(IRecord record)
        {
            ExpectKeys(record, 1);
            return (T1)_convertedEntities.GetOrAdd(((IEntity)record[0]).Id, EntityConverter<T1>.Convert((IEntity)record[0]));
        }

        public (T1, T2) Convert<T1, T2>(IRecord record)
        {
            ExpectKeys(record, 2);
            return ValueTuple.Create(
              (T1)_convertedEntities.GetOrAdd(((IEntity)record[0]).Id, EntityConverter<T1>.Convert((IEntity)record[0])),
              (T2)_convertedEntities.GetOrAdd(((IEntity)record[1]).Id, EntityConverter<T2>.Convert((IEntity)record[1])));
        }

        public (T1, T2, T3) Convert<T1, T2, T3>(IRecord record)
        {
            ExpectKeys(record, 3);

            return ValueTuple.Create(
              (T1)_convertedEntities.GetOrAdd(((IEntity)record[0]).Id, EntityConverter<T1>.Convert((IEntity)record[0])),
              (T2)_convertedEntities.GetOrAdd(((IEntity)record[1]).Id, EntityConverter<T2>.Convert((IEntity)record[1])),
              (T3)_convertedEntities.GetOrAdd(((IEntity)record[2]).Id, EntityConverter<T3>.Convert((IEntity)record[2])));
        }


        public (T1, T2, T3, T4) Convert<T1, T2, T3, T4>(IRecord record)
        {
            ExpectKeys(record, 4);

            return ValueTuple.Create(
              (T1)_convertedEntities.GetOrAdd(((IEntity)record[0]).Id, EntityConverter<T1>.Convert((IEntity)record[0])),
              (T2)_convertedEntities.GetOrAdd(((IEntity)record[1]).Id, EntityConverter<T2>.Convert((IEntity)record[1])),
              (T3)_convertedEntities.GetOrAdd(((IEntity)record[2]).Id, EntityConverter<T3>.Convert((IEntity)record[2])),
              (T4)_convertedEntities.GetOrAdd(((IEntity)record[3]).Id, EntityConverter<T4>.Convert((IEntity)record[3])));
        }

        public (T1, T2, T3, T4, T5) Convert<T1, T2, T3, T4, T5>(IRecord record)
        {
            ExpectKeys(record, 5);

            return ValueTuple.Create(
              (T1)_convertedEntities.GetOrAdd(((IEntity)record[0]).Id, EntityConverter<T1>.Convert((IEntity)record[0])),
              (T2)_convertedEntities.GetOrAdd(((IEntity)record[1]).Id, EntityConverter<T2>.Convert((IEntity)record[1])),
              (T3)_convertedEntities.GetOrAdd(((IEntity)record[2]).Id, EntityConverter<T3>.Convert((IEntity)record[2])),
              (T4)_convertedEntities.GetOrAdd(((IEntity)record[3]).Id, EntityConverter<T4>.Convert((IEntity)record[3])),
              (T5)_convertedEntities.GetOrAdd(((IEntity)record[4]).Id, EntityConverter<T5>.Convert((IEntity)record[4])));
        }

        private static void ExpectKeys(IRecord record, int keys)
        {
            if (record.Keys.Count != keys)
            {
                throw new InvalidOperationException($"Expected {keys} key, but record contains {record.Values.Count} keys");
            }
        }
    }
}
