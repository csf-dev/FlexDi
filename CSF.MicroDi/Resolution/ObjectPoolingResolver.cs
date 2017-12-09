using System;
using CSF.MicroDi.Registration;

namespace CSF.MicroDi.Resolution
{
  public class ObjectPoolingResolver : IResolver
  {
    readonly ICachesResolvedServiceInstancesWithScope cache;
    readonly IResolver resolver;

    public virtual object Resolve(IFactoryAdapter factory) => resolver.Resolve(factory);

    public virtual bool Resolve(ResolutionRequest request, out object output)
    {
      if(request == null)
        throw new ArgumentNullException(nameof(request));

      var registration = GetRegistration(request);

      if(TryGetFromCache(registration, out output))
        return true;

      if(!resolver.Resolve(request, out output))
      {
        output = null;
        return false;
      }

      AddToCacheIfApplicable(registration, output);
      return true;
    }

    public virtual IServiceRegistration GetRegistration(ResolutionRequest request)
      => resolver.GetRegistration(request);

    protected virtual bool TryGetFromCache(IServiceRegistration registration, out object cachedInstance)
    {
      if(registration == null)
        throw new ArgumentNullException(nameof(registration));


      if(registration.Multiplicity != Multiplicity.Shared)
      {
        cachedInstance = null;
        return false;
      }

      var key = ServiceRegistrationKey.ForRegistration(registration);
      return cache.TryGet(key, out cachedInstance);
    }

    protected virtual void AddToCacheIfApplicable(IServiceRegistration registration, object instance)
    {
      if(registration == null)
        throw new ArgumentNullException(nameof(registration));
      
      if(registration.Multiplicity != Multiplicity.Shared)
        return;

      var key = ServiceRegistrationKey.ForRegistration(registration);
      cache.Add(key, instance);
    }

    public ObjectPoolingResolver(IResolver resolver,
                                 ICachesResolvedServiceInstancesWithScope cache = null)
    {
      if(resolver == null)
        throw new ArgumentNullException(nameof(resolver));

      this.resolver = resolver;
      this.cache = cache ?? new ResolvedServiceCacheStack(new ResolvedServiceCache());
    }
  }
}
