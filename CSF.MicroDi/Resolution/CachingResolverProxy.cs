using System;
using CSF.MicroDi.Registration;

namespace CSF.MicroDi.Resolution
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


      if(registration.Multiplicity != Multiplicity.Shared)
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
      
      if(registration.Multiplicity != Multiplicity.Shared)
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
