//
//    Registry.cs
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
using CSF.FlexDi.Resolution;

namespace CSF.FlexDi.Registration
{
    /// <summary>
    /// Concrete implementation of <see cref="IRegistersServices"/> and <see cref="IServiceRegistrationProvider"/>,
    /// which keeps an in-memory collection of a number of registrations and coordinates addition and querying.
    /// </summary>
    public class Registry : IRegistersServices
    {
        readonly object syncRoot;
        readonly ConcurrentDictionary<ServiceRegistrationKey,IServiceRegistration> registrations;

        /// <inheritdoc />
        public bool HasRegistration(ServiceRegistrationKey key) => Get(key) != null;

        /// <summary>
        /// Add the specified component registration.
        /// </summary>
        /// <param name="registration">Registration.</param>
        public void Add(IServiceRegistration registration)
        {
            if(registration == null)
                throw new ArgumentNullException(nameof(registration));

            registration.AssertIsValid();

            var key = ServiceRegistrationKey.FromRegistration(registration);
            lock(syncRoot)
            {
                registrations.TryRemove(key, out _);
                registrations.TryAdd(key, registration);
            }
        }

        /// <summary>
        /// Gets a registration matching the specified key.
        /// </summary>
        /// <param name="key">Key.</param>
        public IServiceRegistration Get(ServiceRegistrationKey key)
        {
            if(key == null)
                throw new ArgumentNullException(nameof(key));

            return GetMatchingRegistrations(key)
                .OrderByDescending(x => x.Priority)
                .FirstOrDefault();
        }

        IServiceRegistration IServiceRegistrationProvider.Get(ResolutionRequest request)=> Get(request);

        IReadOnlyList<IServiceRegistration> GetMatchingRegistrations(ServiceRegistrationKey key)
        {
            lock(syncRoot)
            {
                var hasExactMatch = registrations.TryGetValue(key, out var exactMatch);
                var exactMatches = hasExactMatch ? new[] { exactMatch } : new IServiceRegistration[0];

                var otherMatches = (from registration in GetAll()
                                    where
                                        registration.MatchesKey(key)
                                     && (!hasExactMatch || !ReferenceEquals(registration, exactMatch))
                                    select registration);
                
                return exactMatches.Union(otherMatches).ToList();
            }
        }

        /// <inheritdoc />
        public IReadOnlyCollection<IServiceRegistration> GetAll(Type serviceType = null)
        {
            return (from registration in registrations
                    where
                        serviceType is null
                     || registration.Key.ServiceType == serviceType
                    select registration.Value)
                .ToList();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Registry"/> class.
        /// </summary>
        public Registry()
        {
            syncRoot = new object();
            registrations = new ConcurrentDictionary<ServiceRegistrationKey, IServiceRegistration>();
        }
    }
}
