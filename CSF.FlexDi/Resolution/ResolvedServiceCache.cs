﻿//
//    ResolvedServiceCache.cs
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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CSF.FlexDi.Registration;

namespace CSF.FlexDi.Resolution
{
    /// <summary>
    /// Implementation of <see cref="ICachesResolvedServiceInstances"/> which uses an in-memory cache to
    /// store resolved instances.
    /// </summary>
    public class ResolvedServiceCache : ICachesResolvedServiceInstances
    {
        readonly ConcurrentDictionary<ServiceCacheKey,object> instances;
        readonly object syncRoot;
        static readonly IComparer<ServiceCacheKey> specificityComparer = new CacheKeySpecificityComparer();

        /// <summary>
        /// Adds a component to the cache.
        /// </summary>
        /// <param name="registration">Registration.</param>
        /// <param name="instance">Instance.</param>
        public void Add(IServiceRegistration registration, object instance)
        {
            if(registration == null)
                throw new ArgumentNullException(nameof(registration));
            var key = ServiceRegistrationKey.FromRegistration(registration);

            var cacheKeys = ServiceCacheKey.CreateFromRegistrationKeyAndInstance(key, instance);
            foreach(var cacheKey in cacheKeys)
                instances.TryAdd(cacheKey, instance);
        }

        /// <summary>
        /// Gets a value indicating whether or not the cache contains a component which matches the given key.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the cache contains a matching component; <c>false</c> otherwise.</returns>
        /// <param name="key">Key.</param>
        public bool Has(ServiceRegistrationKey key)
        {
            if(key == null)
                throw new ArgumentNullException(nameof(key));
            return GetBestMatchingCacheKey(key) != null;
        }

        /// <summary>
        /// Attempts to get a service/component instance from the cache, matching a given registration.
        /// </summary>
        /// <returns>
        /// <c>true</c>, if a component was found and retrieved, <c>false</c> otherwise.</returns>
        /// <param name="registration">The registration for which to get a component.</param>
        /// <param name="instance">Exposes the component instance found (only if this method returns <c>true</c>).</param>
        public bool TryGet(IServiceRegistration registration, out object instance)
        {
            var registrationKey = ServiceRegistrationKey.FromRegistration(registration);

            instance = null;
            var cacheKey = GetBestMatchingCacheKey(registrationKey);
            if(cacheKey == null) return false;

            return instances.TryGetValue(cacheKey, out instance);
        }

        ServiceCacheKey GetBestMatchingCacheKey(ServiceRegistrationKey registrationKey)
        {
            var cacheKeys = ServiceCacheKey.CreateFromRegistrationKey(registrationKey);

            lock(syncRoot)
            {
                return GetCandidateCacheKeysLocked(cacheKeys, registrationKey.Name)
                    .OrderByDescending(x => x, specificityComparer)
                    .FirstOrDefault();
            }
        }

        IEnumerable<ServiceCacheKey> GetCandidateCacheKeysLocked(IEnumerable<ServiceCacheKey> cacheKeys, string registrationName)
        {
            foreach(var cacheKey in cacheKeys)
            {
                if(instances.ContainsKey(cacheKey))
                    yield return cacheKey;

                if(registrationName != null) continue;
                var otherMatchingKeys = instances.Keys
                    .Where(x => cacheKey.ImplementationType.GetTypeInfo().IsAssignableFrom(x.ImplementationType.GetTypeInfo()));

                foreach(var otherMatchingKey in otherMatchingKeys)
                    yield return otherMatchingKey;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CSF.FlexDi.Resolution.ResolvedServiceCache"/> class.
        /// </summary>
        public ResolvedServiceCache()
        {
            instances = new ConcurrentDictionary<ServiceCacheKey, object>();
            syncRoot = new object();
        }
    }
}
