using System;
using System.Collections.Generic;
using System.Linq;
using CSF.MicroDi.Registration;

namespace CSF.MicroDi.Resolution
{
  public class ResolvedServiceCacheStack : ICachesResolvedServiceInstancesWithScope
  {
    readonly Stack<CacheAndRegistrationProviderPair> providers;

    protected ICachesResolvedServiceInstances GetProviderForNewItem(IServiceRegistration registration)
    {
      var providerMatchingRegistrationKey = providers
        .FirstOrDefault(x => IsForMatchingRegistrationProvider(x, registration));

      if(providerMatchingRegistrationKey != null)
        return providerMatchingRegistrationKey.Cache;

      return providers.Peek().Cache;
    }

    protected bool IsForMatchingRegistrationProvider(CacheAndRegistrationProviderPair registrationAndCache,
                                                     IServiceRegistration regProviderToMatch)
    {
      if(regProviderToMatch == null)
        throw new ArgumentNullException(nameof(regProviderToMatch));
      if(registrationAndCache == null) return false;

      var regProvider = registrationAndCache.RegistrationProvider;
      return regProvider.HasRegistration(regProviderToMatch);
    }

    public void Add(IServiceRegistration registration, object instance)
      => GetProviderForNewItem(registration).Add(registration, instance);

    public bool Has(IServiceRegistration registration)
    {
      if(registration == null)
        throw new ArgumentNullException(nameof(registration));

      return providers.Any(x => IsForMatchingRegistrationProvider(x, registration));
    }

    public bool Has(ServiceRegistrationKey key)
    {
      if(key == null)
        throw new ArgumentNullException(nameof(key));
      return providers.Any(x => x.Cache.Has(key));
    }

    public bool TryGet(IServiceRegistration registration, out object instance)
    {
      if(registration == null)
        throw new ArgumentNullException(nameof(registration));

      var matchingProvider = providers
        .FirstOrDefault(x => IsForMatchingRegistrationProvider(x, registration));
      if(matchingProvider == null)
      {
        instance = null;
        return false;
      }

      return matchingProvider.Cache.TryGet(registration, out instance);
    }

    public ICachesResolvedServiceInstancesWithScope CreateChildScope(ICachesResolvedServiceInstances provider,
                                                                     IServiceRegistrationProvider registry)
    {
      return new ResolvedServiceCacheStack(provider, registry, providers.ToArray());
    }

    public ResolvedServiceCacheStack(ICachesResolvedServiceInstances firstProvider, IServiceRegistrationProvider registry)
      : this(firstProvider, registry, Enumerable.Empty<CacheAndRegistrationProviderPair>().ToArray()) {}

    public ResolvedServiceCacheStack(ICachesResolvedServiceInstances provider,
                                     IServiceRegistrationProvider registry,
                                     IReadOnlyList<CacheAndRegistrationProviderPair> otherProviders)
    {
      if(provider == null)
        throw new ArgumentNullException(nameof(provider));
      if(registry == null)
        throw new ArgumentNullException(nameof(registry));
      if(otherProviders == null)
        throw new ArgumentNullException(nameof(otherProviders));

      providers = new Stack<CacheAndRegistrationProviderPair>(otherProviders);
      providers.Push(new CacheAndRegistrationProviderPair(registry, provider));
    }
  }
}
