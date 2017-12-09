using System;
using CSF.MicroDi.Registration;

namespace CSF.MicroDi.Resolution
{
  public class ObjectPoolingResolver : IResolver
  {
    readonly ICachesResolvedServiceInstancesWithScope cache;
    readonly IResolver resolver;
    readonly ResolutionPath resolutionPath;
    readonly CircularDependencyDetector circularDependencyDetector;

    public ResolutionPath ResolutionPath => resolutionPath;

    public virtual bool Resolve(ResolutionRequest request, out object output)
      => Resolve(request, out output, this);

    public virtual bool Resolve(ResolutionRequest request, out object output, IResolver outermostResolver)
    {
      if(request == null)
        throw new ArgumentNullException(nameof(request));

      var registration = GetRegistration(request);

      circularDependencyDetector.ThrowOnCircularDependency(registration, outermostResolver.ResolutionPath);

      if(TryGetFromCache(registration, out output))
        return true;

      if(!resolver.Resolve(request, out output, outermostResolver))
      {
        output = null;
        return false;
      }

      AddToCacheIfApplicable(registration, output);
      return true;
    }

    public virtual IServiceRegistration GetRegistration(ResolutionRequest request)
      => resolver.GetRegistration(request);

    public event EventHandler<ServiceResolutionEventArgs> ServiceResolved;

    protected virtual void InvokeServiceResolved(ServiceResolutionEventArgs args)
    {
      ServiceResolved?.Invoke(this, args);
    }

    protected virtual void OnServiceResolved(object sender, ServiceResolutionEventArgs args)
    {
      InvokeServiceResolved(args);
    }

    void InvokeServiceResolved(object sender, ServiceResolutionEventArgs args)
    {
      ServiceResolved?.Invoke(sender, args);
    }

    public IResolver CreateChild(IServiceRegistration registration)
    {
      if(registration == null)
        throw new ArgumentNullException(nameof(registration));

      var path = resolutionPath.CreateChild(registration);
      var child = new ObjectPoolingResolver(resolver, cache, path);
      child.ServiceResolved += OnServiceResolved;
      return child;
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
      resolver.ServiceResolved += InvokeServiceResolved;

      resolutionPath = new ResolutionPath();
      circularDependencyDetector = new CircularDependencyDetector();
    }

    ObjectPoolingResolver(IResolver resolver,
                          ICachesResolvedServiceInstancesWithScope cache,
                          ResolutionPath resolutionPath) : this(resolver, cache)
    {
      if(resolutionPath == null)
        throw new ArgumentNullException(nameof(resolutionPath));
      
      this.resolutionPath = resolutionPath;
    }
  }
}
