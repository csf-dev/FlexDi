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
using CSF.FlexDi.Resolution;

namespace CSF.FlexDi.Registration
{
  /// <summary>
  /// An implementation of <see cref="IServiceRegistrationProvider"/> which provides access to a 'stack' of
  /// instances of <see cref="IServiceRegistrationProvider"/>.  It fulfils the functionality of the provider
  /// by querying across that whole contained collection.
  /// </summary>
  public class StackOfRegistriesRegistrationProvider : IServiceRegistrationProvider
  {
    readonly IReadOnlyList<IServiceRegistrationProvider> providers;

    /// <summary>
    /// Gets a value which indicates whether or not the current provider can fulfil the given resolution request.
    /// </summary>
    /// <returns>
    /// <c>true</c>, if the request can be fulfilled, <c>false</c> otherwise.</returns>
    /// <param name="request">A resolution request.</param>
    public bool CanFulfilRequest(ResolutionRequest request)
    {
      if(request == null)
        throw new ArgumentNullException(nameof(request));

      return providers.Any(x => x.HasRegistration(request));
    }

    /// <summary>
    /// Gets a value which indicates whether or not the current provider has a matching registrations.
    /// </summary>
    /// <returns>
    /// <c>true</c>, if a matching registration is available, <c>false</c> otherwise.</returns>
    /// <param name="key">A registration key.</param>
    public bool HasRegistration(ServiceRegistrationKey key)
    {
      if(key == null)
        throw new ArgumentNullException(nameof(key));
      return providers.Any(x => x.HasRegistration(key));
    }

    /// <summary>
    /// Gets a registration.
    /// </summary>
    /// <param name="request">A resolution request.</param>
    public IServiceRegistration Get(ResolutionRequest request)
    {
      if(request == null)
        throw new ArgumentNullException(nameof(request));

      return providers
        .FirstOrDefault(x => x.HasRegistration(request))
        ?.Get(request);
    }

    /// <inheritdoc />
    public IReadOnlyCollection<IServiceRegistration> GetAll(Type serviceType = null)
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

      if(candidates == null)
        candidates = new IServiceRegistration[0];

      return (from registration in candidates
              let key = ServiceRegistrationKey.FromRegistration(registration)
              where !alreadyFound.Contains(key)
              select new { Registration = registration, Key = key })
        .ToDictionary(k => k.Key, v => v.Registration);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="StackOfRegistriesRegistrationProvider"/> class.
    /// </summary>
    /// <param name="providersInnermostFirst">An ordered collection of providers, the innermost (most deeply nested) should come first.</param>
    public StackOfRegistriesRegistrationProvider(IReadOnlyList<IServiceRegistrationProvider> providersInnermostFirst)
    {
      if(providersInnermostFirst == null)
        throw new ArgumentNullException(nameof(providersInnermostFirst));
      
      this.providers = providersInnermostFirst;
    }
  }
}
