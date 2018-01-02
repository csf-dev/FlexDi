//
//    StackOfRegistriesRegistrationProvider.cs
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
using CSF.MicroDi.Resolution;

namespace CSF.MicroDi.Registration
{
  public class StackOfRegistriesRegistrationProvider : IServiceRegistrationProvider
  {
    readonly IReadOnlyList<IServiceRegistrationProvider> providers;

    public bool CanFulfilRequest(ResolutionRequest request)
    {
      if(request == null)
        throw new ArgumentNullException(nameof(request));

      return providers.Any(x => x.CanFulfilRequest(request));
    }

    public bool HasRegistration(ServiceRegistrationKey key)
    {
      if(key == null)
        throw new ArgumentNullException(nameof(key));
      return providers.Any(x => x.HasRegistration(key));
    }

    public bool HasRegistration(IServiceRegistration registration)
    {
      if(registration == null)
        throw new ArgumentNullException(nameof(registration));
      return providers.Any(x => x.HasRegistration(registration));
    }

    public IServiceRegistration Get(ResolutionRequest request)
    {
      if(request == null)
        throw new ArgumentNullException(nameof(request));

      var provider = providers.FirstOrDefault(x => x.CanFulfilRequest(request));
      if(provider == null)
        return null;
      
      return provider.Get(request);
    }

    public IReadOnlyCollection<IServiceRegistration> GetAll() => GetAll(null);

    public IReadOnlyCollection<IServiceRegistration> GetAll(Type serviceType)
    {
      var registrationsFound = new Dictionary<ServiceRegistrationKey,IServiceRegistration>();

      foreach(var provider in providers)
      {
        var registrationsAndKeys = GetNonConflictingRegistrations(registrationsFound.Keys, provider, serviceType);

        foreach(var regAndKey in registrationsAndKeys)
          registrationsFound.Add(regAndKey.Key, regAndKey.Value);
      }

      return registrationsFound.Values.ToArray();
    }

    IDictionary<ServiceRegistrationKey,IServiceRegistration> GetNonConflictingRegistrations(IEnumerable<ServiceRegistrationKey> alreadyFound,
                                                                                            IServiceRegistrationProvider provider,
                                                                                            Type serviceTypeFilter)
    {
      if(alreadyFound == null)
        throw new ArgumentNullException(nameof(alreadyFound));
      if(provider == null)
        throw new ArgumentNullException(nameof(provider));

      var candidates = (serviceTypeFilter != null)? provider.GetAll(serviceTypeFilter) : provider.GetAll();

      return (from registration in candidates
              let key = ServiceRegistrationKey.ForRegistration(registration)
              where !alreadyFound.Contains(key)
              select new { Registration = registration, Key = key })
        .ToDictionary(k => k.Key, v => v.Registration);
    }

    public StackOfRegistriesRegistrationProvider(IReadOnlyList<IServiceRegistrationProvider> providersOutermostFirst)
    {
      if(providersOutermostFirst == null)
        throw new ArgumentNullException(nameof(providersOutermostFirst));
      
      this.providers = providersOutermostFirst;
    }
  }
}
