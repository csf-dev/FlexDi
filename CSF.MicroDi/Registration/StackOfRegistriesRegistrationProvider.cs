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
