using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using CSF.MicroDi.Registration;

namespace CSF.MicroDi.Resolution
{
  public class ResolvedServiceCache : ICachesResolvedServiceInstances
  {
    readonly ConcurrentDictionary<ServiceCacheKey,object> instances;
    readonly object syncRoot;
    static readonly CacheKeySpecificityComparer specificityComparer;

    public void Add(IServiceRegistration registration, object instance)
    {
      if(registration == null)
        throw new ArgumentNullException(nameof(registration));
      var key = ServiceRegistrationKey.ForRegistration(registration);

      var cacheKey = ServiceCacheKey.FromRegistrationKeyAndInstance(key, instance);
      instances.TryAdd(cacheKey, instance);
    }

    public bool Has(IServiceRegistration registration)
    {
      if(registration == null)
        throw new ArgumentNullException(nameof(registration));
      var key = ServiceRegistrationKey.ForRegistration(registration);
      return Has(key);
    }

    public bool Has(ServiceRegistrationKey key)
    {
      if(key == null)
        throw new ArgumentNullException(nameof(key));
      return GetCandidateCacheKeys(key).Any();
    }

    public bool TryGet(IServiceRegistration registration, out object instance)
    {
      if(registration == null)
        throw new ArgumentNullException(nameof(registration));
      var key = ServiceRegistrationKey.ForRegistration(registration);

      var cacheKey = GetCandidateCacheKeys(key).FirstOrDefault();
      if(cacheKey == null)
      {
        instance = null;
        return false;
      }

      return instances.TryGetValue(cacheKey, out instance);
    }

    IReadOnlyList<ServiceCacheKey> GetCandidateCacheKeys(ServiceRegistrationKey registrationKey)
    {
      var cacheKey = ServiceCacheKey.FromRegistrationKey(registrationKey);
      var output = new List<ServiceCacheKey>();

      lock(syncRoot)
      {
        if(instances.ContainsKey(cacheKey))
          output.Add(cacheKey);

        var otherMatchingKeys = instances.Keys
          .Where(x => cacheKey.ImplementationType.IsAssignableFrom(x.ImplementationType));
        output.AddRange(otherMatchingKeys);

        return output.OrderByDescending(x => x, specificityComparer).ToArray();
      }
    }

    public ResolvedServiceCache()
    {
      instances = new ConcurrentDictionary<ServiceCacheKey, object>();
      syncRoot = new object();
    }

    static ResolvedServiceCache()
    {
      specificityComparer = new CacheKeySpecificityComparer();
    }
  }
}
