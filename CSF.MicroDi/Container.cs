//
//    Container.cs
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
    readonly IServiceRegistrationProvider registryStack;
    readonly IDisposesOfResolvedInstances disposer;
    readonly ContainerOptions options;
    readonly IContainer parentContainer;
    readonly ISelectsConstructor constructorSelector;

    #endregion

    #region IProvidesResolutionInfo implementation

    public ICachesResolvedServiceInstances Cache => cache;

    public IRegistersServices Registry => registry;

    public ContainerOptions Options => options;

    public IProvidesResolutionInfo Parent => parentContainer as IProvidesResolutionInfo;

    public ISelectsConstructor ConstructorSelector => constructorSelector;

    #endregion

    #region IResolvesServices implementation

    public T Resolve<T>() => Resolve<T>(null);

    public T Resolve<T>(string name)
    {
      AssertNotDisposed();

      object output;
      if(!TryResolve(typeof(T), name, out output))
        throw new ResolutionException($"The service type `{typeof(T).FullName}' could not be resolved");
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
      var result = TryResolve(request);

      if(!result.IsSuccess)
      {
        output = null;
        return false;
      }

      output = result.ResolvedObject;
      return true;
    }

    public ResolutionResult TryResolve(ResolutionRequest request)
    {
      if(request == null)
        throw new ArgumentNullException(nameof(request));

      return resolver.Resolve(request);
    }

    public object Resolve(ResolutionRequest request)
    {
      var result = TryResolve(request);
      if(!result.IsSuccess)
        throw new ResolutionException($"The service type `{request.ServiceType.FullName}' could be resolved");

      return result.ResolvedObject;
    }

    public IReadOnlyCollection<T> ResolveAll<T>()
    {
      AssertNotDisposed();

      return ResolveAll(typeof(T)).Cast<T>().ToArray();
    }

    public IReadOnlyCollection<object> ResolveAll(Type serviceType)
    {
      AssertNotDisposed();

      return GetRegistrations(serviceType)
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
      return registryStack.CanFulfilRequest(request);
    }

    public IReadOnlyCollection<IServiceRegistration> GetRegistrations()
    {
      AssertNotDisposed();

      return registryStack.GetAll();
    }

    public IReadOnlyCollection<IServiceRegistration> GetRegistrations(Type serviceType)
    {
      AssertNotDisposed();

      return registryStack.GetAll(serviceType);
    }

    public IContainer CreateChildContainer()
    {
      return new Container(options: Options, parentContainer: this);
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

      var helper = new RegistrationHelper(constructorSelector);
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
                     IContainer parentContainer = null,
                     ICreatesResolvers resolverFactory = null)
    {
      disposedValue = false;

      this.parentContainer = parentContainer;

      this.options = GetContainerOptions(options, parentContainer);
      constructorSelector = new ConstructorWithMostParametersSelector(this.options.UseNonPublicConstructors);

      this.registry = registry ?? new Registry();
      this.cache = cache ?? new ResolvedServiceCache();
      this.disposer = disposer ?? new ServiceInstanceDisposer();
      this.registryStack = new RegistryStackFactory().CreateRegistryStack(this);
      this.resolver = resolver ?? GetResolver(resolverFactory);

      this.resolver.ServiceResolved += InvokeServiceResolved;

      PerformSelfRegistrations();
    }

    ContainerOptions GetContainerOptions(ContainerOptions explicitOptions,
                                         IContainer parent)
    {
      if(explicitOptions != null)
        return explicitOptions;

      var providesInfo = parent as IProvidesResolutionInfo;
      if(providesInfo != null)
        return providesInfo.Options;

      return ContainerOptions.Default;
    }

    IResolver GetResolver(ICreatesResolvers resolverFactory)
    {
      var factory = resolverFactory ?? new ResolverFactory();
      var output = factory.CreateResolver(this);

      if(output == null)
        throw new ArgumentException($"The implementation of {nameof(ICreatesResolvers)} must not return a null instance of {nameof(IResolver)}.", nameof(resolverFactory));
      
      return output;
    }

    void PerformSelfRegistrations()
    {
      if(Options.SelfRegisterAResolver)
        SelfRegisterAResolver();
      
      if(Options.SelfRegisterTheRegistry)
        SelfRegisterTheRegistry();
    }

    void SelfRegisterAResolver()
    {
      var resolverRegistration = new InstanceRegistration(this) {
        DisposeWithContainer = false,
        ServiceType = typeof(IResolvesServices),
      };
      Registry.Add(resolverRegistration);

      var containerRegistration = new InstanceRegistration(this) {
        DisposeWithContainer = false,
        ServiceType = typeof(IContainer),
      };
      Registry.Add(containerRegistration);
    }

    void SelfRegisterTheRegistry()
    {
      var registration = new InstanceRegistration(this) {
        DisposeWithContainer = false,
        ServiceType = typeof(IReceivesRegistrations),
      };
      Registry.Add(registration);
    }

    #endregion

    #region static methods

    public static ContainerBuilder CreateBuilder() => new ContainerBuilder();

    #endregion
  }
}
