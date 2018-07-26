using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BallisticReports.Neo4J.Converters;

namespace BallisticReports.Neo4J.NodeMetaData
{
    internal abstract class NodeMemberDefinition
    {
        private readonly ConcurrentDictionary<Type, Action<object, object>> _setMethods;
        private readonly Type _memberType;
        private Func<object, object> _getValueMethod;
        protected NodeMemberDefinition(string name, string originalMemberName, Type memberType, bool identifier)
        {
            _memberType = memberType;
            Name = name;
            OriginalMemberName = originalMemberName;
            Identifier = identifier;
            _setMethods = new ConcurrentDictionary<Type, Action<object, object>>();
        }
        public string OriginalMemberName { get; }
        public string Name { get; }
        public bool Identifier { get; }
        protected abstract Action<object, object> BuildSetValueMethod(Type fromType);
        protected abstract Func<object, object> BuildGetValueMethod();

        public object GetValue(object entity, IEnumerable<ICustomPropertySerializer> customSerializers = null)
        {
            if (_getValueMethod == null)
            {
                _getValueMethod = BuildGetValueMethod();
            }
            var value = _getValueMethod(entity);
            var customSerializer =
              customSerializers?.FirstOrDefault(serializer => serializer.CanSerialize(_memberType))
              ?? PropertySerializers.Default?.FirstOrDefault(serializer => serializer.CanSerialize(_memberType));

            return customSerializer != null
              ? customSerializer.Serialize(value)
              : value;
        }

        public void SetValue(object entity, object value, IEnumerable<ICustomPropertySerializer> customSerializers = null)
        {
            var customSerializer =
              customSerializers?.FirstOrDefault(serializer => serializer.CanSerialize(_memberType))
              ?? PropertySerializers.Default?.FirstOrDefault(serializer => serializer.CanSerialize(_memberType));
            if (customSerializer != null)
            {
                var parsedValue = customSerializer.Parse(value);
                _setMethods.GetOrAdd(parsedValue.GetType(), BuildSetValueMethod(parsedValue.GetType()))(entity, parsedValue);
                return;
            }
            _setMethods.GetOrAdd(value.GetType(), BuildSetValueMethod(value.GetType()))(entity, value);
        }
    }
}
