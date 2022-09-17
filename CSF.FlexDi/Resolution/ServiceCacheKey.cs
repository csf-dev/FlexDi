//
//    ServiceCacheKey.cs
//
//    Copyright 2018  Craig Fowler et al
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
using System.Collections.Generic;
using System.Linq;
using CSF.FlexDi.Registration;

namespace CSF.FlexDi.Resolution
{
    /// <summary>
    /// Represents a key for caching a service/component instance within a <see cref="ICachesResolvedServiceInstances"/>
    /// implementation.
    /// </summary>
    public sealed class ServiceCacheKey : IEquatable<ServiceCacheKey>
    {
        /// <summary>
        /// Gets the implementation type of the service/component.
        /// </summary>
        /// <value>The type of the implementation.</value>
        public Type ImplementationType { get; private set; }

        /// <summary>
        /// Gets the registration name for the component.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="T:CSF.FlexDi.Resolution.ServiceCacheKey"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="T:CSF.FlexDi.Resolution.ServiceCacheKey"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="T:CSF.FlexDi.Resolution.ServiceCacheKey"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as ServiceCacheKey);
        }

        /// <summary>
        /// Determines whether the specified <see cref="ServiceCacheKey"/> is equal to the current <see cref="T:CSF.FlexDi.Resolution.ServiceCacheKey"/>.
        /// </summary>
        /// <param name="other">The <see cref="ServiceCacheKey"/> to compare with the current <see cref="T:CSF.FlexDi.Resolution.ServiceCacheKey"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="ServiceCacheKey"/> is equal to the current
        /// <see cref="T:CSF.FlexDi.Resolution.ServiceCacheKey"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(ServiceCacheKey other)
        {
            if(ReferenceEquals(other, null))
                return false;
            if(ReferenceEquals(other, this))
                return true;

            return (other.ImplementationType == ImplementationType
                            && other.Name == Name);
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="T:CSF.FlexDi.Resolution.ServiceCacheKey"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
        public override int GetHashCode()
        {
            var typeHash = ImplementationType.GetHashCode();
            var nameHash = Name?.GetHashCode() ?? 0;

            return typeHash ^ nameHash;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CSF.FlexDi.Resolution.ServiceCacheKey"/> class.
        /// </summary>
        /// <param name="implementationType">Implementation type.</param>
        /// <param name="name">Name.</param>
        public ServiceCacheKey(Type implementationType, string name)
        {
            if(implementationType == null)
                throw new ArgumentNullException(nameof(implementationType));

            ImplementationType = implementationType;
            Name = name;
        }

        /// <summary>
        /// Creates a collection of cache keys from a service registration key and component instance.
        /// </summary>
        /// <returns>The created keys.</returns>
        /// <param name="key">Key.</param>
        /// <param name="instance">Instance.</param>
        public static IReadOnlyCollection<ServiceCacheKey> CreateFromRegistrationKeyAndInstance(ServiceRegistrationKey key, object instance)
        {
            if(key == null)
                throw new ArgumentNullException(nameof(key));

            var fromRegistrationOnly = CreateFromRegistrationKey(key);
            if(ReferenceEquals(instance, null))
                return fromRegistrationOnly;

            return fromRegistrationOnly
                .Union(new [] { new ServiceCacheKey(instance.GetType(), key.Name) })
                .ToArray();
        }

        /// <summary>
        /// Creates a collection of cache keys from a service registration key.
        /// </summary>
        /// <returns>The created keys.</returns>
        /// <param name="key">Key.</param>
        public static IReadOnlyCollection<ServiceCacheKey> CreateFromRegistrationKey(ServiceRegistrationKey key)
        {
            if(key == null)
                throw new ArgumentNullException(nameof(key));

            return new[] { new ServiceCacheKey(key.ServiceType, key.Name) };
        }
    }
}
