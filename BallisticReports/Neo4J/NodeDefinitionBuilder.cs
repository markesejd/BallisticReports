using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BallisticReports.Neo4J.NodeMetaData;

namespace BallisticReports.Neo4J
{
    internal static class NodeDefinitionBuilder
    {
        private static readonly ConcurrentDictionary<Type, NodeDefinition> NodeDefinitionCache = new ConcurrentDictionary<Type, NodeDefinition>();

        public static NodeDefinition GetNodeDefinition(Type type)
        {
            return NodeDefinitionCache.GetOrAdd(type, BuildNodeDefinition);
        }
        private static NodeDefinition BuildNodeDefinition(Type type) => new NodeDefinition(type);
    }
}
