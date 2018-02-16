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

    /// <summary>
    /// Gets a value which indicates whether or not a matching registration is contained within the current instance.
    /// </summary>
    /// <param name="key">A service registration key.</param>
    public bool Contains(ServiceRegistrationKey key)
    {
      if(key == null)
        throw new ArgumentNullException(nameof(key));

      var candidates = GetCandidateRegistrations(key);
      return candidates.Any();
    }

    /// <summary>
    /// Add the specified component registration.
    /// </summary>
    /// <param name="registration">Registration.</param>
    public void Add(IServiceRegistration registration)
    {
      if(registration == null)
        throw new ArgumentNullException(nameof(registration));

      registration.AssertIsValid();

      var key = ServiceRegistrationKey.ForRegistration(registration);
      lock(syncRoot)
      {
        IServiceRegistration removed;
        registrations.TryRemove(key, out removed);
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

      var candidates = GetCandidateRegistrations(key).OrderByDescending(x => x.Priority);
      return candidates.FirstOrDefault();
    }

    IReadOnlyList<IServiceRegistration> GetCandidateRegistrations(ServiceRegistrationKey key)
    {
      var output = new List<IServiceRegistration>();

      lock(syncRoot)
      {
        IServiceRegistration exactMatch;
        if(registrations.TryGetValue(key, out exactMatch))
          output.Add(exactMatch);

        var otherMatches = (from registration in registrations.Values
                            where
                              registration.MatchesKey(key)
                              && (exactMatch == null || !ReferenceEquals(registration, exactMatch))
                            select registration)
          .ToArray();
        output.AddRange(otherMatches);
      }

      return output.ToArray();
    }

    /// <summary>
    /// Gets all of the registrations which can fulfil a given service/component type.
    /// </summary>
    /// <returns>All of the matching registrations.</returns>
    /// <param name="serviceType">A service type.</param>
    public IReadOnlyCollection<IServiceRegistration> GetAll(Type serviceType)
    {
      if(serviceType == null)
        throw new ArgumentNullException(nameof(serviceType));

      return registrations
        .Where(x => x.Key.ServiceType == serviceType)
        .Select(x => x.Value)
        .ToArray();
    }

    /// <summary>
    /// Gets all of the registrations available to the current provider
    /// </summary>
    /// <returns>All of the registrations.</returns>
    public IReadOnlyCollection<IServiceRegistration> GetAll()
    {
      return registrations.Values.ToArray();
    }

    /// <summary>
    /// Gets a value which indicates whether or not the current provider has a specified registrations.
    /// </summary>
    /// <returns>
    /// <c>true</c>, if the registration is contained within this provider, <c>false</c> otherwise.</returns>
    /// <param name="registration">A registration.</param>
    public bool HasRegistration(IServiceRegistration registration)
    {
      if(registration == null)
        throw new ArgumentNullException(nameof(registration));

      return registrations.Values.Any(x => ReferenceEquals(x, registration));
    }

    IServiceRegistration IServiceRegistrationProvider.Get(ResolutionRequest request)
    {
      if(request == null)
        throw new ArgumentNullException(nameof(request));

      var key = ServiceRegistrationKey.FromRequest(request);
      return Get(key);
    }

    bool IServiceRegistrationProvider.CanFulfilRequest(ResolutionRequest request)
    {
      if(request == null)
        throw new ArgumentNullException(nameof(request));

      var key = ServiceRegistrationKey.FromRequest(request);
      return Contains(key);
    }

    bool IServiceRegistrationProvider.HasRegistration(ServiceRegistrationKey key)
    {
      if(key == null)
        throw new ArgumentNullException(nameof(key));
      return Contains(key);
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
