﻿//
//    LazyFactory.cs
//
//    Copyright 2018  Craig Fowler
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//
//    For further copyright info, including a complete author/contributor
//    list, please refer to the file NOTICE.txt
using System;
using System.Linq;
using System.Reflection;

namespace CSF.FlexDi.Resolution.Proxies
{
    /// <summary>
    /// A service which creates instances of <c>System.Lazy&lt;T&gt;</c>.
    /// </summary>
    public static class LazyFactory
    {
        static readonly Type
            LazyOpenGenericType = typeof(Lazy<>);
        static readonly MethodInfo CreateLazyObjectMethod = typeof(LazyFactory).GetTypeInfo().DeclaredMethods
            .FirstOrDefault(x => x.Name == nameof(CreateLazyObject));

        /// <summary>
        /// Gets a value which indicates whether or not the specified type is lazy or not.
        /// </summary>
        /// <returns><c>true</c>, if the specified type is a <c>System.Lazy&lt;T&gt;</c>, <c>false</c> otherwise.</returns>
        /// <param name="type">Type.</param>
        public static bool IsLazyType(Type type) => GetInnerLazyType(type) != null;

        /// <summary>
        /// Gets the 'inner' type of a <c>System.Lazy&lt;T&gt;</c>.
        /// </summary>
        /// <returns>The inner type.</returns>
        /// <param name="type">A lazy type.</param>
        public static Type GetInnerLazyType(Type type)
        {
            if(type == null) return null;
            if(!type.GetTypeInfo().IsGenericType) return null;
            var genericTypeDef = type.GetTypeInfo().GetGenericTypeDefinition();
            if(genericTypeDef != LazyOpenGenericType) return null;
            return type.GetTypeInfo().GenericTypeArguments[0];
        }

        /// <summary>
        /// Creates and returns a <c>System.Lazy&lt;T&gt;</c> which wraps the given factory delegate.
        /// </summary>
        /// <returns>The lazy object.</returns>
        /// <param name="innerLazyType">Inner lazy type.</param>
        /// <param name="factory">The delegate which would create the value for the lazy instance.</param>
        public static object GetLazyObject(Type innerLazyType, Func<object> factory)
            => GetLazyObject(innerLazyType, (Delegate) factory);

        static object GetLazyObject(Type innerLazyType, Delegate factory)
        {
            if(innerLazyType == null)
                throw new ArgumentNullException(nameof(innerLazyType));
            if(factory == null)
                throw new ArgumentNullException(nameof(factory));

            var method = CreateLazyObjectMethod.MakeGenericMethod(innerLazyType);
            return method.Invoke(null, new [] { factory });
        }

        static object CreateLazyObject<T>(Delegate factory)
        {
            return new Lazy<T>(() => {
                try
                {
                    return (T) factory.DynamicInvoke();
                }
                catch(TargetInvocationException ex)
                {
                    var message = String.Format(Resources.ExceptionFormats.LazyResolutionException, typeof(T).FullName);
                    throw new ResolutionException(message, ex);
                }
            });
        }
    }
}
