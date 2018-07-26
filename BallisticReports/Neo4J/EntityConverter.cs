using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo4j.Driver.V1;

namespace BallisticReports.Neo4J
{
    internal static class EntityConverter<TEntity>
    {
        public static Func<object, TEntity> Convert { get; } = CreateConverter();

        /// <summary>
        /// Determines the best approach for converting an object and compiles a converter
        /// </summary>
        private static Func<object, TEntity> CreateConverter()
        {
            var entityType = typeof(TEntity);

            if ((Nullable.GetUnderlyingType(entityType) ?? entityType).GetInterfaces().Contains(typeof(IConvertible)))
            {
                return ConvertConvertible;
            }

            if (entityType == typeof(INode) || entityType == typeof(IRelationship) || entityType == typeof(IPath))
            {
                return ConvertNeo4JType;
            }

            return CompileComplexObjectConverter;
        }

        /// <summary>
        /// Delegate for converting simple types that implement <see cref="IConvertible"/>. This will include <see cref="string"/>, <see cref="int"/>, <see cref="double"/>, etc.
        /// </summary>
        private static TEntity ConvertConvertible(object obj)
        {
            if (obj == null)
            {
                return default(TEntity);
            }
            if (obj is IConvertible convertible)
            {
                var toType = Nullable.GetUnderlyingType(typeof(TEntity)) ?? typeof(TEntity);
                return (TEntity)convertible.ToType(toType, CultureInfo.InvariantCulture);
            }
            throw new InvalidCastException($"Cannot convert {obj.GetType().Name} to {typeof(TEntity).Name}");
        }

        /// <summary>
        /// Delegate for using the Neo4J Driver's built-in conversions for it's types, such as <see cref="INode"/>, <see cref="IRelationship"/>, and <see cref="IPath"/>
        /// </summary>
        private static TEntity ConvertNeo4JType(object obj)
        {
            return obj.As<TEntity>();
        }

        /// <summary>
        /// Uses node definition to convert from a neo4j complex object into an entity
        /// </summary>
        private static TEntity CompileComplexObjectConverter(object neo4JObj)
        {
            switch (neo4JObj)
            {
                case IEntity entity:
                    return NodeDefinitionBuilder.GetNodeDefinition(typeof(TEntity)).Convert<TEntity>(entity);
                default:
                    throw new InvalidCastException("Attempted to convert from " + neo4JObj.GetType() + " to " + typeof(TEntity));
            }
        }
    }
}
