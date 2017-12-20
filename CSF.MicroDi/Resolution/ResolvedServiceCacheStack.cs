using System;
using System.Collections.Generic;
using System.Linq;
using CSF.MicroDi.Registration;

namespace CSF.MicroDi.Resolution
{
  public class ResolvedServiceCacheStack : ICachesResolvedServiceInstancesWithScope
  {
    readonly Stack<ICachesResolvedServiceInstances> providers;
    readonly IDictionary<IServiceRegistrationProvider,ICachesResolvedServiceInstances> providerPerRegistry;

    protected ICachesResolvedServiceInstances GetProvider(ServiceRegistrationKey key)
    {
      var providerMatchingRegistrationKey = providerPerRegistry.FirstOrDefault(x => x.Key.HasRegistration(key)).Value;

      if(providerMatchingRegistrationKey != null)
        return providerMatchingRegistrationKey;

      return providers.First();
    }

    public void Add(ServiceRegistrationKey key, object instance)
      => GetProvider(key).Add(key, instance);

    public bool Has(ServiceRegistrationKey key)
    {
      if(key == null)
        throw new ArgumentNullException(nameof(key));

      return providers.Any(x => x.Has(key));
    }

    public bool TryGet(ServiceRegistrationKey key, out object instance)
    {
      if(key == null)
        throw new ArgumentNullException(nameof(key));

      var matchingProvider = providers.FirstOrDefault(x => x.Has(key));
      if(matchingProvider == null)
      {
        instance = null;
        return false;
      }

      return matchingProvider.TryGet(key, out instance);
    }

    public ICachesResolvedServiceInstancesWithScope CreateChildScope(ICachesResolvedServiceInstances provider,
                                                                     IServiceRegistrationProvider registry)
    {
      return new ResolvedServiceCacheStack(provider, registry, providers, providerPerRegistry);
    }

    public ResolvedServiceCacheStack(ICachesResolvedServiceInstances firstProvider, IServiceRegistrationProvider registry)
      : this(firstProvider, registry, Enumerable.Empty<ICachesResolvedServiceInstances>(), new Dictionary<IServiceRegistrationProvider,ICachesResolvedServiceInstances>()) {}

    public ResolvedServiceCacheStack(ICachesResolvedServiceInstances provider,
                                     IServiceRegistrationProvider registry,
                                     IEnumerable<ICachesResolvedServiceInstances> otherProviders,
                                     IDictionary<IServiceRegistrationProvider,ICachesResolvedServiceInstances> providersByRegistration)
    {
      if(provider == null)
        throw new ArgumentNullException(nameof(provider));
      if(registry == null)
        throw new ArgumentNullException(nameof(registry));
      if(otherProviders == null)
        throw new ArgumentNullException(nameof(otherProviders));
      if(providersByRegistration == null)
        throw new ArgumentNullException(nameof(providersByRegistration));
      

      providers = new Stack<ICachesResolvedServiceInstances>(otherProviders);
      providers.Push(provider);

      providerPerRegistry = new Dictionary<IServiceRegistrationProvider,ICachesResolvedServiceInstances>(providersByRegistration);
      providerPerRegistry.Add(registry, provider);
    }
  }
}
