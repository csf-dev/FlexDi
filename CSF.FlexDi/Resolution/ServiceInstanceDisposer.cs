//
//    ServiceInstanceDisposer.cs
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
    /// Default implementation of <see cref="IDisposesOfResolvedInstances"/>.
    /// </summary>
    public class ServiceInstanceDisposer : IDisposesOfResolvedInstances
    {
        /// <summary>
        /// Coordinates the disposal of all disposable service/component instances within the given cache.
        /// </summary>
        /// <param name="registrationProvider">Registration provider.</param>
        /// <param name="instances">A cache which provides access to the component instances.</param>
        public void DisposeInstances(IServiceRegistrationProvider registrationProvider,
                                     ICachesResolvedServiceInstances instances)
        {
            var registrationsToDispose = GetRegistrationsToDispose(registrationProvider);
            DisposeRegistrations(registrationsToDispose, instances);
        }

        /// <summary>
        /// Gets a collection of the service/component registrations which are eligible for disposal.
        /// </summary>
        /// <returns>The registrations to dispose.</returns>
        /// <param name="registrationProvider">Registration provider.</param>
        static IReadOnlyCollection<IServiceRegistration> GetRegistrationsToDispose(IServiceRegistrationProvider registrationProvider)
        {
            if(registrationProvider == null)
                throw new ArgumentNullException(nameof(registrationProvider));

            return registrationProvider
                .GetAll()
                .Where(x => x.Cacheable && x.DisposeWithContainer)
                .ToArray();
        }

        /// <summary>
        /// Disposes of any instances contained within the cache, which are eligible for disposal.
        /// </summary>
        /// <param name="registrations">Registrations.</param>
        /// <param name="instanceCache">Instance cache.</param>
        static void DisposeRegistrations(IReadOnlyCollection<IServiceRegistration> registrations,
                                                    ICachesResolvedServiceInstances instanceCache)
        {
            foreach(var reg in registrations)
                DisposeRegistration(reg, instanceCache);
        }

        /// <summary>
        /// Disposes of a single component instance, matching a given registration.
        /// </summary>
        /// <param name="registration">Registration.</param>
        /// <param name="instanceCache">Instance cache.</param>
        static void DisposeRegistration(IServiceRegistration registration, ICachesResolvedServiceInstances instanceCache)
        {
            if(registration == null)
                throw new ArgumentNullException(nameof(registration));
            if(instanceCache == null)
                throw new ArgumentNullException(nameof(instanceCache));

            if(!instanceCache.TryGet(registration, out var instance))
                return;

            if(instance is IDisposable disposable)
                disposable.Dispose();
        }
    }
}
