using System;
using System.Collections.Generic;
using System.Linq;
using CSF.MicroDi.Builders;
using CSF.MicroDi.Registration;
using CSF.MicroDi.Resolution;

namespace CSF.MicroDi
{
  public class Container : IContainer
  {
    #region fields

    readonly IRegistersServices currentRegistry;
    readonly ICachesResolvedServiceInstances currentCache;
    readonly IRegistersServicesWithScope registry;
    readonly ICachesResolvedServiceInstancesWithScope cache;
    readonly IFulfilsResolutionRequests resolver;
    readonly IDisposesOfResolvedInstances disposer;

    #endregion

    #region IResolvesServices implementation

    public T Resolve<T>() => Resolve<T>(null);

    public T Resolve<T>(string name)
    {
      AssertNotDisposed();

      object output;
      if(!TryResolve(typeof(T), name, out output))
        throw new ResolutionException($"The service type `{typeof(T).FullName}' could be resolved");
      return (T) output;
    }

    public bool TryResolve<T>(out T output) => TryResolve(null, out output);

    public bool TryResolve<T>(string name, out T output)
    {
      AssertNotDisposed();

      object resolved;
      var result = TryResolve(typeof(T), name, out resolved);
      if(!result)
      {
        output = default(T);
        return false;
      }

      output = (T) resolved;
      return true;
    }

    public object Resolve(Type serviceType) => Resolve(serviceType, null);

    public object Resolve(Type serviceType, string name)
    {
      AssertNotDisposed();

      object output;
      if(!TryResolve(serviceType, name, out output))
        throw new ResolutionException($"The service type `{serviceType.FullName}' could be resolved");
      return output;
    }

    public bool TryResolve(Type serviceType, out object output) => TryResolve(serviceType, null, out output);

    public bool TryResolve(Type serviceType, string name, out object output)
    {
      AssertNotDisposed();

      var request = new ResolutionRequest(serviceType, name);
      return resolver.Resolve(request, out output);
    }

    public IReadOnlyCollection<T> ResolveAll<T>()
    {
      AssertNotDisposed();

      return ResolveAll(typeof(T)).Cast<T>().ToArray();
    }

    public IReadOnlyCollection<object> ResolveAll(Type serviceType)
    {
      AssertNotDisposed();

      return registry
        .GetAll(serviceType)
        .Select(x => Resolve(x.ServiceType, x.Name))
        .ToArray();
    }

    #endregion

    #region IContainer implementation

    public bool HasRegistration<T>(string name = null)
    {
      return HasRegistration(typeof(T), name);
    }

    public bool HasRegistration(Type serviceType, string name = null)
    {
      AssertNotDisposed();

      var request = new ResolutionRequest(serviceType, name);
      return registry.CanFulfilRequest(request);
    }

    public IReadOnlyCollection<IServiceRegistration> GetRegistrations()
    {
      AssertNotDisposed();

      return registry.GetAll();
    }

    public IReadOnlyCollection<IServiceRegistration> GetRegistrations(Type serviceType)
    {
      AssertNotDisposed();

      return registry.GetAll(serviceType);
    }

    #endregion

    #region IReceivesRegistrations implementation

    public void AddRegistrations(Action<IRegistrationHelper> registrationActions)
    {
      if(registrationActions == null)
        throw new ArgumentNullException(nameof(registrationActions));

      AssertNotDisposed();

      var helper = new RegistrationHelper();
      registrationActions(helper);

      AddRegistrations(helper.GetRegistrations());
    }

    public void AddRegistrations(IEnumerable<IServiceRegistration> registrations)
    {
      if(registrations == null)
        throw new ArgumentNullException(nameof(registrations));
      
      AssertNotDisposed();

      foreach(var registration in registrations)
        currentRegistry.Add(registration);
    }

    #endregion

    #region IDisposable implementation

    bool disposedValue;

    protected virtual void Dispose(bool disposing)
    {
      if(!disposedValue)
      {
        if(disposing)
          disposer.DisposeInstances(currentRegistry, currentCache);

        disposedValue = true;
      }
    }

    public void Dispose()
    {
      Dispose(true);
    }

    void AssertNotDisposed()
    {
      if(disposedValue)
        throw new ContainerDisposedException("The requested action is not valid for a container which has been disposed.");
    }

    #endregion

    #region constructors

    public Container(IRegistersServices initialRegistry = null,
                     ICachesResolvedServiceInstances initialCache = null,
                     IRegistersServicesWithScope scopedRegistry = null,
                     ICachesResolvedServiceInstancesWithScope scopedCache = null,
                     IFulfilsResolutionRequests resolver = null,
                     IDisposesOfResolvedInstances disposer = null)
    {
      disposedValue = false;

      currentRegistry = initialRegistry ?? new Registry();
      currentCache = initialCache ?? new ResolvedServiceCache();

      registry = scopedRegistry ?? new RegistryStack(currentRegistry);
      cache = scopedCache ?? new ResolvedServiceCacheStack(currentCache);

      this.resolver = resolver ?? new ObjectPoolingResolver(new Resolver(registry), cache: cache);
      this.disposer = disposer ?? new ServiceInstanceDisposer();
    }

    public Container(Container container,
                     IRegistersServices nextRegistry = null,
                     ICachesResolvedServiceInstances nextCache = null,
                     IFulfilsResolutionRequests resolver = null,
                     IDisposesOfResolvedInstances disposer = null)
    {
      if(registry == null)
        throw new ArgumentNullException(nameof(registry));
      if(cache == null)
        throw new ArgumentNullException(nameof(cache));

      disposedValue = false;

      currentRegistry = nextRegistry ?? new Registry();
      currentCache = nextCache ?? new ResolvedServiceCache();

      registry = container.registry.CreateChildScope(currentRegistry);
      cache = container.cache.CreateChildScope(currentCache);

      this.resolver = resolver ?? new ObjectPoolingResolver(new Resolver(registry), cache: cache);
      this.disposer = disposer ?? new ServiceInstanceDisposer();
    }

    #endregion
  }
}
