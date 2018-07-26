using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BallisticReports.Neo4J.Attributes;

namespace BallisticReports.Neo4J.NodeMetaData
{
    internal sealed class NodeFieldDefinition : NodeMemberDefinition
    {
        private readonly FieldInfo _fieldInfo;
        public NodeFieldDefinition(FieldInfo fieldInfo) : base(
            fieldInfo.GetCustomAttribute<NodePropertyAttribute>(true)?.Name ?? fieldInfo.Name,
            fieldInfo.Name,
            fieldInfo.FieldType,
            fieldInfo.GetCustomAttribute<NodePropertyAttribute>(true)?.Identifier ?? false)
        {
            _fieldInfo = fieldInfo;
        }

        protected override Action<object, object> BuildSetValueMethod(Type fromType)
        {
            var entity = Expression.Parameter(typeof(object));
            var value = Expression.Parameter(typeof(object));

            var unboxEntity = Expression.Convert(entity, _fieldInfo.DeclaringType);
            var unboxValue = Expression.Convert(value, fromType);
            var convertValue = Expression.Convert(unboxValue, _fieldInfo.FieldType);

            var entityField = Expression.Field(unboxEntity, _fieldInfo);
            var assign = Expression.Assign(entityField, convertValue);
            return Expression.Lambda<Action<object, object>>(assign, entity, value).Compile();
        }

        protected override Func<object, object> BuildGetValueMethod()
        {
            var entity = Expression.Parameter(typeof(object));
            var unboxEntity = Expression.Convert(entity, _fieldInfo.DeclaringType);
            var fieldExpression = Expression.Field(unboxEntity, _fieldInfo);
            var convertToObject = Expression.Convert(fieldExpression, typeof(object));
            return Expression.Lambda<Func<object, object>>(convertToObject, entity).Compile();
        }
    }
}
