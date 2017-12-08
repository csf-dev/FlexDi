using System;
using System.Collections.Concurrent;
using CSF.MicroDi.Registration;

namespace CSF.MicroDi.Resolution
{
  public class ResolvedServiceCache : ICachesResolvedServiceInstances
  {
    readonly ConcurrentDictionary<ServiceRegistrationKey,object> instances;

    public void Add(ServiceRegistrationKey key, object instance)
    {
      if(key == null)
        throw new ArgumentNullException(nameof(key));

      instances.TryAdd(key, instance);
    }

    public bool Has(ServiceRegistrationKey key)
    {
      if(key == null)
        throw new ArgumentNullException(nameof(key));
      
      return instances.ContainsKey(key);
    }

    public bool TryGet(ServiceRegistrationKey key, out object instance)
    {
      if(key == null)
        throw new ArgumentNullException(nameof(key));

      return instances.TryGetValue(key, out instance);
    }

    public ResolvedServiceCache()
    {
      instances = new ConcurrentDictionary<ServiceRegistrationKey, object>();
    }
  }
}
