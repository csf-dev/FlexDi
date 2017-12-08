using System;
using System.Collections.Generic;
using System.Linq;
using CSF.MicroDi.Registration;

namespace CSF.MicroDi.Resolution
{
  public class ResolvedServiceCacheStack : ICachesResolvedServiceInstancesWithScope
  {
    readonly Stack<ICachesResolvedServiceInstances> providers;

    protected ICachesResolvedServiceInstances CurrentProvider => providers.Peek();

    public void Add(ServiceRegistrationKey key, object instance)
    {
      CurrentProvider.Add(key, instance);
    }

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

    public ICachesResolvedServiceInstancesWithScope CreateChildScope(ICachesResolvedServiceInstances provider)
    {
      return new ResolvedServiceCacheStack(provider, providers);
    }

    public ResolvedServiceCacheStack(ICachesResolvedServiceInstances firstProvider)
      : this(firstProvider, Enumerable.Empty<ICachesResolvedServiceInstances>()) {}

    public ResolvedServiceCacheStack(ICachesResolvedServiceInstances provider,
                                     IEnumerable<ICachesResolvedServiceInstances> otherProviders)
    {
      if(provider == null)
        throw new ArgumentNullException(nameof(provider));
      if(otherProviders == null)
        throw new ArgumentNullException(nameof(otherProviders));

      providers = new Stack<ICachesResolvedServiceInstances>(otherProviders);
      providers.Push(provider);
    }
  }
}
