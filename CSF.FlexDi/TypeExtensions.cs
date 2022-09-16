using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CSF.FlexDi
{
    /// <summary>
    /// Extension methods for <see cref="Type"/>.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Gets a value that indicates whether or not the specified <paramref name="type"/> is assignable
        /// to the open generic type <paramref name="openGenericType"/>, including open generic versions of
        /// <paramref name="type"/> or any of the interfaces or base types it derives from.
        /// </summary>
        /// <param name="type">The type to test.</param>
        /// <param name="openGenericType">The open generic type to test against.</param>
        /// <returns><see langword="true" /> if the <paramref name="type"/>, any interface it implements, or
        /// any base type from which it derives is a generic type, where the open generic version of that type
        /// is equal to <paramref name="openGenericType"/>; <see langword="false" /> otherwise.</returns>
        public static bool IsAssignableToOpenGeneric(this Type type, Type openGenericType)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            if (openGenericType is null)
                throw new ArgumentNullException(nameof(openGenericType));
            
            var openGenericTypeInfo = openGenericType.GetTypeInfo();
            if (!openGenericTypeInfo.IsGenericTypeDefinition)
                throw new ArgumentException(String.Format(Resources.ExceptionFormats.TypeMustBeOpenGeneric, openGenericType.FullName), nameof(openGenericType));

            var typesToTest = type.GetTypeInfo().ImplementedInterfaces.Select(x => x.GetTypeInfo())
                .Union(GetBaseTypes(type.GetTypeInfo()))
                .Where(x => x.IsGenericType)
                .Select(x => x.GetGenericTypeDefinition().GetTypeInfo())
                .ToList();
            return typesToTest.Any(x => openGenericTypeInfo.IsAssignableFrom(x));
        }

        static IEnumerable<TypeInfo> GetBaseTypes(TypeInfo type)
        {
            for (var current = type; current != null; current = current.BaseType?.GetTypeInfo())
                yield return current;
        }
    }
}