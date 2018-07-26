using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BallisticReports.Neo4J
{
    internal static class PropertyRetriever
    {
        private static readonly ConcurrentDictionary<Type, PropertyInfo[]> PropertyCache = new ConcurrentDictionary<Type, PropertyInfo[]>();
        internal static PropertyInfo[] GetPropertiesWithCache(this Type type)
        {
            return PropertyCache.GetOrAdd(type, type.GetProperties());
        }



        private static readonly ConcurrentDictionary<PropertyInfo, Func<object, object>> GetterDelegateCache = new ConcurrentDictionary<PropertyInfo, Func<object, object>>();
        internal static object GetValueWithDelegateCache(this PropertyInfo property, object target)
        {
            return GetterDelegateCache.GetOrAdd(property, CreateDelegate(property));
        }

        private static Func<object, object> CreateDelegate(PropertyInfo property)
        {
            return (Func<object, object>)Delegate.CreateDelegate(
                typeof(Func<object, object>),
                property.GetMethod);
        }
    }
}
