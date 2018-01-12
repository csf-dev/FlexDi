//
//    ResolvedServiceCache.cs
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

      var cacheKeys = ServiceCacheKey.CreateFromRegistrationKeyAndInstance(key, instance);
      foreach(var cacheKey in cacheKeys)
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
      return TryGet(key, out instance);
    }

    public bool TryGet(Type serviceType, string name, out object instance)
    {
      if(serviceType == null)
        throw new ArgumentNullException(nameof(serviceType));
      
      var key = new ServiceRegistrationKey(serviceType, name);
      return TryGet(key, out instance);
    }

    bool TryGet(ServiceRegistrationKey key, out object instance)
    {
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
      var cacheKeys = ServiceCacheKey.CreateFromRegistrationKey(registrationKey);
      var output = new List<ServiceCacheKey>();

      lock(syncRoot)
      {
        foreach(var cacheKey in cacheKeys)
        {
          if(instances.ContainsKey(cacheKey))
            output.Add(cacheKey);

          if(registrationKey.Name == null)
          {
            var otherMatchingKeys = instances.Keys
              .Where(x => cacheKey.ImplementationType.IsAssignableFrom(x.ImplementationType));
            output.AddRange(otherMatchingKeys);
          }
        }

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
