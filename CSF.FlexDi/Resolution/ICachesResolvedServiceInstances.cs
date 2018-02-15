//
//    ICachesResolvedServiceInstances.cs
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
using CSF.FlexDi.Registration;

namespace CSF.FlexDi.Resolution
{
  /// <summary>
  /// A service which caches the resolved instances of services/components.  The contents of the cache are stored
  /// by their corresponding <see cref="IServiceRegistration"/>.
  /// </summary>
  public interface ICachesResolvedServiceInstances
  {
    /// <summary>
    /// Adds a component to the cache.
    /// </summary>
    /// <param name="registration">Registration.</param>
    /// <param name="instance">Instance.</param>
    void Add(IServiceRegistration registration, object instance);

    /// <summary>
    /// Gets a value indicating whether or not the cache contains a component which matches the given registration.
    /// </summary>
    /// <returns><c>true</c> if the cache contains a matching component; <c>false</c> otherwise.</returns>
    /// <param name="registration">Registration.</param>
    bool Has(IServiceRegistration registration);

    /// <summary>
    /// Gets a value indicating whether or not the cache contains a component which matches the given key.
    /// </summary>
    /// <returns><c>true</c> if the cache contains a matching component; <c>false</c> otherwise.</returns>
    /// <param name="key">Key.</param>
    bool Has(ServiceRegistrationKey key);

    /// <summary>
    /// Attempts to get a service/component instance from the cache, matching a given registration.
    /// </summary>
    /// <returns><c>true</c>, if a component was found and retrieved, <c>false</c> otherwise.</returns>
    /// <param name="registration">The registration for which to get a component.</param>
    /// <param name="instance">Exposes the component instance found (only if this method returns <c>true</c>).</param>
    bool TryGet(IServiceRegistration registration, out object instance);

    /// <summary>
    /// Attempts to get a service/component instance from the cache, matching a given type and name.
    /// </summary>
    /// <returns><c>true</c>, if a component was found and retrieved, <c>false</c> otherwise.</returns>
    /// <param name="serviceType">The service/component type.</param>
    /// <param name="name">The registration name.</param>
    /// <param name="instance">Exposes the component instance found (only if this method returns <c>true</c>).</param>
    bool TryGet(Type serviceType, string name, out object instance);
  }
}
