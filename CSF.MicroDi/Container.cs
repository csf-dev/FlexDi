using System;
using System.Collections.Generic;
using System.Linq;
using CSF.MicroDi.Builders;
using CSF.MicroDi.Registration;
using CSF.MicroDi.Resolution;

namespace CSF.MicroDi
{
  public class Container : IContainer, IProvidesResolutionInfo
  {
    #region fields

    readonly IFulfilsResolutionRequests resolver;
    readonly ICachesResolvedServiceInstances cache;
    readonly IRegistersServices registry;
    readonly IDisposesOfResolvedInstances disposer;
    readonly ContainerOptions options;
    readonly IContainer parentContainer;

    #endregion

    #region IProvidesResolutionInfo implementation

    public ICachesResolvedServiceInstances Cache => cache;

    public IRegistersServices Registry => registry;

    public ContainerOptions Options => options;

    public IProvidesResolutionInfo Parent => parentContainer as IProvidesResolutionInfo;

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
      var result = resolver.Resolve(request);

      if(!result.IsSuccess)
      {
        output = null;
        return false;
      }

      output = result.ResolvedObject;
      return true;
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

    public event EventHandler<ServiceResolutionEventArgs> ServiceResolved;

    void InvokeServiceResolved(object sender, ServiceResolutionEventArgs args)
    {
      ServiceResolved?.Invoke(this, args);
    }

    #endregion

    #region IReceivesRegistrations implementation

    public void AddRegistrations(Action<IRegistrationHelper> registrationActions)
    {
      if(registrationActions == null)
        throw new ArgumentNullException(nameof(registrationActions));

      AssertNotDisposed();

      var helper = new RegistrationHelper(options.UseNonPublicConstructors);
      registrationActions(helper);

      AddRegistrations(helper.GetRegistrations());
    }

    public void AddRegistrations(IEnumerable<IServiceRegistration> registrations)
    {
      if(registrations == null)
        throw new ArgumentNullException(nameof(registrations));
      
      AssertNotDisposed();

      foreach(var registration in registrations)
      {
        DoNotPermitReRegisteringAServiceWhichIsAlreadyCached(registration);
        registry.Add(registration);
      }
    }

    void DoNotPermitReRegisteringAServiceWhichIsAlreadyCached(IServiceRegistration registration)
    {
      var key = ServiceRegistrationKey.ForRegistration(registration);
      if(cache.Has(key))
        throw new ServiceReRegisteredAfterResolutionException($"Cannot re-register a service after it has already been resolved from the container and cached.{Environment.NewLine}Invalid registration: {registration.ToString()}");
    }

    #endregion

    #region IDisposable implementation

    bool disposedValue;

    protected virtual void Dispose(bool disposing)
    {
      if(!disposedValue)
      {
        if(disposing)
          disposer.DisposeInstances(registry, cache);

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

    public Container(IRegistersServices registry = null,
                     ICachesResolvedServiceInstances cache = null,
                     IFulfilsResolutionRequests resolver = null,
                     IDisposesOfResolvedInstances disposer = null,
                     ContainerOptions options = null,
                     IContainer parentContainer = null)
    {
      disposedValue = false;

      this.options = options ?? ContainerOptions.Default;
      this.parentContainer = parentContainer;

      this.registry = registry ?? new Registry();
      this.cache = cache ?? new ResolvedServiceCache();
      this.disposer = disposer ?? new ServiceInstanceDisposer();
      this.resolver = resolver ?? new ResolverFactory().CreateResolver(this);

      this.resolver.ServiceResolved += InvokeServiceResolved;
    }

    #endregion

    #region static methods

    public static IContainerBuilder CreateBuilder() => new ContainerBuilder();

    #endregion
  }
}
