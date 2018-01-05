//
//    CachingResolverProxy.cs
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
using CSF.MicroDi.Registration;

namespace CSF.MicroDi.Resolution.Proxies
{
  public class CachingResolverProxy : ProxyingResolver
  {
    readonly ICachesResolvedServiceInstances cache;

    public override ResolutionResult Resolve(ResolutionRequest request)
    {
      if(request == null)
        throw new ArgumentNullException(nameof(request));

      var registration = GetRegistration(request);
      var output = ResolveFromCache(registration, request);

      if(output.IsSuccess)
        return output;

      output = ProxiedResolver.Resolve(request);
      if(output.IsSuccess)
      {
        AddToCacheIfApplicable(registration, output.ResolvedObject);
      }

      return output;
    }

    protected virtual ResolutionResult ResolveFromCache(IServiceRegistration registration, ResolutionRequest request)
    {
      if(registration == null)
        return ResolutionResult.Failure(request.ResolutionPath);

      object resolved;

      if(TryGetFromCache(registration, out resolved))
        return ResolutionResult.Success(request.ResolutionPath, resolved);

      return ResolutionResult.Failure(request.ResolutionPath);
    }

    protected virtual bool TryGetFromCache(IServiceRegistration registration, out object cachedInstance)
    {
      if(registration == null)
        throw new ArgumentNullException(nameof(registration));


      if(!registration.Cacheable)
      {
        cachedInstance = null;
        return false;
      }

      return cache.TryGet(registration, out cachedInstance);
    }

    protected virtual void AddToCacheIfApplicable(IServiceRegistration registration, object instance)
    {
      if(registration == null)
        throw new ArgumentNullException(nameof(registration));
      
      if(!registration.Cacheable)
        return;

      cache.Add(registration, instance);
    }

    public CachingResolverProxy(IResolver proxiedResolver,
                                ICachesResolvedServiceInstances cache = null) : base(proxiedResolver)
    {
      this.cache = cache ?? new ResolvedServiceCache();
    }
  }
}
