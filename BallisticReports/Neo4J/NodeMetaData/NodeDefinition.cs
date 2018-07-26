using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BallisticReports.Neo4J.Attributes;
using BallisticReports.Neo4J.Converters;
using Neo4j.Driver.V1;

namespace BallisticReports.Neo4J.NodeMetaData
{
    internal sealed class NodeDefinition
    {
        public NodeDefinition(Type type)
        {
            Properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Select(propertyInfo => (NodeMemberDefinition)new NodePropertyDefinition(propertyInfo))
                .Union(type.GetFields(BindingFlags.Instance | BindingFlags.Public)
                    .Select(fieldInfo => (NodeMemberDefinition)new NodeFieldDefinition(fieldInfo)))
                .ToArray();
            Labels = type.GetCustomAttributes<NodeLabelAttribute>(true)?
                .Select(attribute => attribute.Labels)
                .SelectMany(_ => _)
                .ToArray();
            if (!Labels.Any())
            {
                Labels = new[] { type.Name };
            }

            _createNewEntity = Expression.Lambda<Func<object>>(Expression.New(type)).Compile();
        }
        public string[] Labels { get; }
        public NodeMemberDefinition[] Properties { get; }

        public IEnumerable<NodeMemberDefinition> Identifiers => Properties.Where(property => property.Identifier);
        public IEnumerable<NodeMemberDefinition> NonIdentifiers => Properties.Where(property => !property.Identifier);

        public T Convert<T>(IEntity node)
        {
            var entity = _createNewEntity();
            foreach (var property in Properties)
            {
                if (node.Properties.TryGetValue(property.Name, out var nodeProperty))
                {
                    property.SetValue(entity, nodeProperty);
                }
            }
            return (T)entity;
        }

        public IDictionary<string, object> ToDictionary(object entity, IEnumerable<ICustomPropertySerializer> customSerializers = null)
        {
            return Properties.ToDictionary(property => property.OriginalMemberName, property => property.GetValue(entity));
        }
        private readonly Func<object> _createNewEntity;
    }
}
