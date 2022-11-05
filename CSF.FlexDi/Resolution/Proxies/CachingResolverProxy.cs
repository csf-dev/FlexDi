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
using CSF.FlexDi.Registration;

namespace CSF.FlexDi.Resolution.Proxies
{
  /// <summary>
  /// A proxying resolver which makes use of a service cache to store resolved instances, and then fulfils
  /// resolution requests using instances stored in the cache where available.
  /// </summary>
  public class CachingResolverProxy : ProxyingResolver
  {
    readonly ICachesResolvedServiceInstances cache;

    /// <summary>
    /// Resolves the given resolution request and returns the result.
    /// </summary>
    /// <param name="request">Request.</param>
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

    /// <summary>
    /// Attempts to fulfil the given resolution request, using the given registration, from the cache.
    /// </summary>
    /// <returns>A resolution result.</returns>
    /// <param name="registration">Registration.</param>
    /// <param name="request">Request.</param>
    protected virtual ResolutionResult ResolveFromCache(IServiceRegistration registration, ResolutionRequest request)
    {
      if(registration == null)
        return ResolutionResult.Failure(request.ResolutionPath);

      object resolved;

      if(TryGetFromCache(registration, out resolved))
        return ResolutionResult.Success(request.ResolutionPath, resolved);

      return ResolutionResult.Failure(request.ResolutionPath);
    }

    /// <summary>
    /// Attempts to get a service/component instance from the cache, where it matches the given registration.
    /// </summary>
    /// <returns><c>true</c>, if the component was resolved from the cache, <c>false</c> otherwise.</returns>
    /// <param name="registration">Registration.</param>
    /// <param name="cachedInstance">Cached instance.</param>
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

    /// <summary>
    /// Adds a service/component instance to the cache where it is applicable to do so.
    /// </summary>
    /// <param name="registration">Registration.</param>
    /// <param name="instance">Instance.</param>
    protected virtual void AddToCacheIfApplicable(IServiceRegistration registration, object instance)
    {
      if(registration == null)
        throw new ArgumentNullException(nameof(registration));
      
      if(!registration.Cacheable)
        return;

      cache.Add(registration, instance);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.FlexDi.Resolution.Proxies.CachingResolverProxy"/> class.
    /// </summary>
    /// <param name="proxiedResolver">Proxied resolver.</param>
    /// <param name="cache">Cache.</param>
    public CachingResolverProxy(IResolver proxiedResolver,
                                ICachesResolvedServiceInstances cache) : base(proxiedResolver)
    {
      if(cache == null)
        throw new ArgumentNullException(nameof(cache));
      
      this.cache = cache;
    }
  }
}
