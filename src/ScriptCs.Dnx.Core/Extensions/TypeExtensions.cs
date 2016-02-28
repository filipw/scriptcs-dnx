using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ScriptCs.Dnx.Core.Extensions
{
    internal static class TypeExtensions
    {
        internal static IEnumerable<MethodInfo> GetExtensionMethods(this Type type)
        {
            return type.GetTypeInfo().Assembly.GetExportedTypes().Where(x => !IntrospectionExtensions.GetTypeInfo(x).IsGenericType && !x.IsNested && IntrospectionExtensions.GetTypeInfo(x).IsSealed)
                .SelectMany(x => x.GetMethods(BindingFlags.Static | BindingFlags.Public))
                .Where(x => x.IsDefined(typeof(ExtensionAttribute), false))
                .Where(x => x.GetParameters()[0].ParameterType == type);
        }

        internal static IEnumerable<MethodInfo> GetAllMethods(this Type type)
        {
            return type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(m => !m.IsSpecialName).Union(type.GetExtensionMethods()).OrderBy(x => x.Name);
        }
    }
}