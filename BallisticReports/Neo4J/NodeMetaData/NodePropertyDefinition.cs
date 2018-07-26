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
    internal sealed class NodePropertyDefinition : NodeMemberDefinition
    {
        private readonly PropertyInfo _propertyInfo;
        public NodePropertyDefinition(PropertyInfo propertyInfo) : base(
            propertyInfo.GetCustomAttribute<NodePropertyAttribute>(true)?.Name ?? propertyInfo.Name,
            propertyInfo.Name,
            propertyInfo.PropertyType,
            propertyInfo.GetCustomAttribute<NodePropertyAttribute>(true)?.Identifier ?? false)
        {
            _propertyInfo = propertyInfo;
        }

        protected override Action<object, object> BuildSetValueMethod(Type fromType)
        {
            var entity = Expression.Parameter(typeof(object));
            var value = Expression.Parameter(typeof(object));

            var unboxEntity = Expression.Convert(entity, _propertyInfo.DeclaringType);
            var unboxValue = Expression.Convert(value, fromType);
            var convertValue = Expression.Convert(unboxValue, _propertyInfo.PropertyType);

            var setMethod = Expression.Call(unboxEntity, _propertyInfo.SetMethod, convertValue);
            return Expression.Lambda<Action<object, object>>(setMethod, entity, value).Compile();
        }

        protected override Func<object, object> BuildGetValueMethod()
        {
            var parameter = Expression.Parameter(typeof(object));
            var myType = _propertyInfo.DeclaringType;
            var castToHostType = Expression.TypeAs(parameter, myType);
            var getMethod = Expression.Call(castToHostType, _propertyInfo.GetMethod);
            var convertToObject = Expression.Convert(getMethod, typeof(object));
            return Expression.Lambda<Func<object, object>>(convertToObject, parameter).Compile();
        }
    }
}
