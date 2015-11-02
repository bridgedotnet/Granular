using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Granular.Extensions
{
    public static class AssemblyExtensions
    {
        private static readonly Dictionary<string, IEnumerable<object>> attributesCache = new Dictionary<string, IEnumerable<object>>();

        public static IEnumerable<T> GetCustomAttributesCached<T>(this Assembly assembly)
        {
            IEnumerable<object> attributes;

            if (attributesCache.TryGetValue(assembly.FullName, out attributes))
            {
                return attributes.OfType<T>();
            }

            attributes = assembly.GetCustomAttributes(false) ?? new object[0];

            attributesCache.Add(assembly.FullName, attributes);
            return attributes.OfType<T>();
        }

        public static T FirstOrDefaultCustomAttributeCached<T>(this Assembly assembly)
        {
            return assembly.GetCustomAttributesCached<T>().FirstOrDefault();
        }
    }
}
