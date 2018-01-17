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
  public class Registry : IRegistersServices
  {
    readonly object syncRoot;
    readonly ConcurrentDictionary<ServiceRegistrationKey,IServiceRegistration> registrations;

    public bool Contains(ServiceRegistrationKey key)
    {
      if(key == null)
        throw new ArgumentNullException(nameof(key));

      var candidates = GetCandidateRegistrations(key);
      return candidates.Any();
    }

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

    public IReadOnlyCollection<IServiceRegistration> GetAll(Type serviceType)
    {
      if(serviceType == null)
        throw new ArgumentNullException(nameof(serviceType));

      return registrations
        .Where(x => x.Key.ServiceType == serviceType)
        .Select(x => x.Value)
        .ToArray();
    }

    public IReadOnlyCollection<IServiceRegistration> GetAll()
    {
      return registrations.Values.ToArray();
    }

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

    public Registry()
    {
      syncRoot = new object();
      registrations = new ConcurrentDictionary<ServiceRegistrationKey, IServiceRegistration>();
    }
  }
}
