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
using System.Reflection;
using CSF.FlexDi.Builders;
using CSF.FlexDi.Registration;
using CSF.FlexDi.Resolution;

namespace CSF.FlexDi
{
    /// <summary>
    /// Default implementation of <see cref="IContainer"/> - this is the FlexDi container.
    /// </summary>
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

        /// <summary>
        /// Gets the cache of resolved services.
        /// </summary>
        /// <value>The cache.</value>
        public ICachesResolvedServiceInstances Cache => cache;

        /// <summary>
        /// Gets the registry (the service registrations).
        /// </summary>
        /// <value>The registry.</value>
        public IRegistersServices Registry => registry;

        /// <summary>
        /// Gets the options used to construct the current container instance.
        /// </summary>
        /// <value>The options.</value>
        public ContainerOptions Options => options;

        /// <summary>
        /// Gets a reference to the resolution information for the parent container (if one exists).
        /// </summary>
        /// <value>The parent resolution info.</value>
        public IProvidesResolutionInfo Parent => parentContainer as IProvidesResolutionInfo;

        /// <summary>
        /// Gets a service which is used to select constructors for instantiating new objects.
        /// </summary>
        /// <value>The constructor selector.</value>
        public ISelectsConstructor ConstructorSelector => constructorSelector;

        #endregion

        #region IResolvesServices implementation

        /// <summary>
        /// Resolves an instance of the specified type.
        /// </summary>
        /// <typeparam name="T">The component type to be resolved.</typeparam>
        public T Resolve<T>() => Resolve<T>(null);

        /// <summary>
        /// Resolves an instance of the specified type, using the given named registration.
        /// </summary>
        /// <param name="name">The registration name.</param>
        /// <typeparam name="T">The component type to be resolved.</typeparam>
        public T Resolve<T>(string name)
        {
            AssertNotDisposed();

            object output;
            if (!TryResolve(typeof(T), name, out output))
                ThrowResolutionFailureException(typeof(T));
            return (T)output;
        }

        /// <summary>
        /// Attempts to resolve an instance of the specified type, but does not raise an exception if resolution fails.
        /// </summary>
        /// <returns>
        /// <c>true</c>, if resolution was successful, <c>false</c> otherwise.</returns>
        /// <param name="output">The resolved component instance.</param>
        /// <typeparam name="T">The component type to be resolved.</typeparam>
        public bool TryResolve<T>(out T output) => TryResolve(null, out output);

        /// <summary>
        /// Attempts to resolve an instance of the specified type and using the given named registration, but
        /// does not raise an exception if resolution fails.
        /// </summary>
        /// <returns>
        /// <c>true</c>, if resolution was successful, <c>false</c> otherwise.</returns>
        /// <param name="output">The resolved component instance.</param>
        /// <param name="name">The registration name.</param>
        /// <typeparam name="T">The component type to be resolved.</typeparam>
        public bool TryResolve<T>(string name, out T output)
        {
            AssertNotDisposed();

            object resolved;
            var result = TryResolve(typeof(T), name, out resolved);
            if (!result)
            {
                output = default(T);
                return false;
            }

            output = (T)resolved;
            return true;
        }

        /// <summary>
        /// Attempts to resolve an instance of the specified type, but returns a <c>null</c> reference if resolution fails.
        /// </summary>
        /// <returns>The resolved component instance, or a <c>null</c> reference if resolution fauls.</returns>
        /// <typeparam name="T">The component type to be resolved.</typeparam>
        public T TryResolve<T>() where T : class => TryResolve<T>(null);

        /// <summary>
        /// Attempts to resolve an instance of the specified type and using the given named registration, but
        /// returns a <c>null</c> reference if resolution fails.
        /// </summary>
        /// <returns>The resolved component instance, or a <c>null</c> reference if resolution fauls.</returns>
        /// <param name="name">The registration name.</param>
        /// <typeparam name="T">The component type to be resolved.</typeparam>
        public T TryResolve<T>(string name) where T : class
        {
            T output;
            if (!TryResolve<T>(name, out output))
                return null;

            return output;
        }

        /// <summary>
        /// Attempts to resolve an instance of the specified type, but returns a <c>null</c> reference if resolution fails.
        /// </summary>
        /// <returns>The resolved component instance, or a <c>null</c> reference if resolution fauls.</returns>
        /// <param name="serviceType">The component type to be resolved.</param>
        public object TryResolve(Type serviceType) => TryResolve(serviceType, null);

        /// <summary>
        /// Attempts to resolve an instance of the specified type and using the given named registration, but
        /// returns a <c>null</c> reference if resolution fails.
        /// </summary>
        /// <returns>The resolved component instance, or a <c>null</c> reference if resolution fauls.</returns>
        /// <param name="name">The registration name.</param>
        /// <param name="serviceType">The component type to be resolved.</param>
        public object TryResolve(Type serviceType, string name)
        {
            if (serviceType == null)
                throw new ArgumentNullException(nameof(serviceType));
            if (serviceType.GetTypeInfo().IsValueType)
                throw new ArgumentException(Resources.ExceptionFormats.TypeToResolveMustBeNullableReferenceType, nameof(serviceType));

            object output;
            if (!TryResolve(serviceType, name, out output))
                return null;

            return output;
        }

        /// <summary>
        /// Resolves an instance of the specified type.
        /// </summary>
        /// <param name="serviceType">The component type to be resolved.</param>
        public object Resolve(Type serviceType) => Resolve(serviceType, null);

        /// <summary>
        /// Resolves an instance of the specified type, using the given named registration.
        /// </summary>
        /// <param name="serviceType">The component type to be resolved.</param>
        /// <param name="name">The registration name.</param>
        public object Resolve(Type serviceType, string name)
        {
            AssertNotDisposed();

            object output;
            if (!TryResolve(serviceType, name, out output))
                ThrowResolutionFailureException(serviceType);
            return output;
        }

        /// <summary>
        /// Attempts to resolve an instance of the specified type, but does not raise an exception if resolution fails.
        /// </summary>
        /// <returns>
        /// <c>true</c>, if resolution was successful, <c>false</c> otherwise.</returns>
        /// <param name="output">The resolved component instance.</param>
        /// <param name="serviceType">The component type to be resolved.</param>
        public bool TryResolve(Type serviceType, out object output) => TryResolve(serviceType, null, out output);

        /// <summary>
        /// Attempts to resolve an instance of the specified type and using the given named registration, but
        /// does not raise an exception if resolution fails.
        /// </summary>
        /// <returns>
        /// <c>true</c>, if resolution was successful, <c>false</c> otherwise.</returns>
        /// <param name="output">The resolved component instance.</param>
        /// <param name="name">The registration name.</param>
        /// <param name="serviceType">The component type to be resolved.</param>
        public bool TryResolve(Type serviceType, string name, out object output)
        {
            AssertNotDisposed();

            var request = new ResolutionRequest(serviceType, name);
            var result = TryResolve(request);

            if (!result.IsSuccess)
            {
                output = null;
                return false;
            }

            output = result.ResolvedObject;
            return true;
        }

        /// <summary>
        /// Attempts to resolve a component, as specified by a <see cref="T:CSF.FlexDi.Resolution.ResolutionRequest" /> instance.
        /// The result indicates whether resolution was successful or not, and if it is, contains a reference to the resolved
        /// component.
        /// </summary>
        /// <returns>A resolution result instance.</returns>
        /// <param name="request">A resolution request specifying what is to be resolved.</param>
        public ResolutionResult TryResolve(ResolutionRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return resolver.Resolve(request);
        }

        /// <summary>
        /// Resolves a component, as specified by a <see cref="T:CSF.FlexDi.Resolution.ResolutionRequest" /> instance.
        /// </summary>
        /// <param name="request">The resolved component instance.</param>
        public object Resolve(ResolutionRequest request)
        {
            var result = TryResolve(request);
            if (!result.IsSuccess)
                ThrowResolutionFailureException(request.ServiceType);

            return result.ResolvedObject;
        }

        /// <summary>
        /// Creates a collection which contains resolved instances of all of the components registered for a given type.
        /// </summary>
        /// <returns>A collection of resolved components.</returns>
        /// <typeparam name="T">The type of the components to be resolved.</typeparam>
        public IReadOnlyCollection<T> ResolveAll<T>()
        {
            AssertNotDisposed();

            return ResolveAll(typeof(T)).Cast<T>().ToArray();
        }

        /// <summary>
        /// Creates a collection which contains resolved instances of all of the components registered for a given type.
        /// </summary>
        /// <returns>A collection of resolved components.</returns>
        /// <param name="serviceType">The type of the components to be resolved.</param>
        public IReadOnlyCollection<object> ResolveAll(Type serviceType)
        {
            AssertNotDisposed();

            return GetRegistrations(serviceType)
              .Select(x => Resolve(x.ServiceType, x.Name))
              .ToArray();
        }

        void ThrowResolutionFailureException(Type componentType)
        {
            var message = String.Format(Resources.ExceptionFormats.CannotResolveComponentType, componentType.FullName);
            throw new ResolutionException(message);
        }

        #endregion

        #region IContainer implementation

        /// <summary>
        /// Gets a value indicating whether or not the container has a registration for the specified component type.
        /// </summary>
        /// <returns>
        /// <c>true</c>, if the container has a registration for the component type, <c>false</c> otherwise.</returns>
        /// <param name="name">An optional registration name.</param>
        /// <typeparam name="T">The component type for which to check.</typeparam>
        public bool HasRegistration<T>(string name = null)
        {
            return HasRegistration(typeof(T), name);
        }

        /// <summary>
        /// Gets a value indicating whether or not the container has a registration for the specified component type.
        /// </summary>
        /// <returns>
        /// <c>true</c>, if the container has a registration for the component type, <c>false</c> otherwise.</returns>
        /// <param name="name">An optional registration name.</param>
        /// <param name="serviceType">The component type for which to check.</param>
        public bool HasRegistration(Type serviceType, string name = null)
        {
            AssertNotDisposed();

            var request = new ResolutionRequest(serviceType, name);
            return registryStack.CanFulfilRequest(request);
        }

        /// <summary>
        /// Gets a collection of all of the registrations held within the current container instance.
        /// </summary>
        /// <returns>The registrations.</returns>
        public IReadOnlyCollection<IServiceRegistration> GetRegistrations()
        {
            AssertNotDisposed();

            return registryStack.GetAll();
        }

        /// <summary>
        /// Gets a collection of all of the registrations, matching a specified component type, held within
        /// the current container instance.
        /// </summary>
        /// <returns>The registrations.</returns>
        /// <param name="serviceType">The component type for which to get the registrations.</param>
        public IReadOnlyCollection<IServiceRegistration> GetRegistrations(Type serviceType)
        {
            AssertNotDisposed();

            return registryStack.GetAll(serviceType);
        }

        /// <summary>
        /// Creates a new <see cref="T:CSF.FlexDi.IContainer" /> which is a child of the current container.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Child container instances 'inherit' all of the registrations from their parent container instance.  That is - if
        /// a parent has IFoo registered and its child resolves IFoo, the resolution will be fulfilled by the parent.
        /// </para>
        /// <para>
        /// Child containers may also add new registrations and may even override the registrations upon their parent, by
        /// re-registering the same component type (and registration name if applicable).  When any component is resolved, it
        /// is always resolved from the container at which it was registered.  When a container is diposed, it will only
        /// dispose resolved instances which that container itself had created (thus it will only dispose instances which were
        /// registered with that container).
        /// </para>
        /// <para>
        /// Leveraging these behaviours, it is suitable to use child containers to control the lifetime of components which
        /// should have a shorter life-span (between creation and disposal) than others.
        /// </para>
        /// <para>
        /// For example, in a testing scenario, a single container instance may be used as the ultimate "parent" for the test
        /// run.  However each test scenario may use this method to create a child container which has a lifetime of only
        /// that test scenario.  The child container may be used to resolve all of the components registered with its parent,
        /// and when the child container is diposed those components from the parent container will 'live on' without being
        /// disposed.  However, any new registrations added to the child container will result in components which are (by
        /// default) disposed when the child container is disposed; IE: At the end of the test scenario.
        /// </para>
        /// </remarks>
        /// <returns>The child container.</returns>
        public IContainer CreateChildContainer()
        {
            return new Container(options: Options, parentContainer: this);
        }

        /// <summary>
        /// Event which occurs when a service is resolved.
        /// </summary>
        public event EventHandler<ServiceResolutionEventArgs> ServiceResolved;

        void InvokeServiceResolved(object sender, ServiceResolutionEventArgs args)
        {
            ServiceResolved?.Invoke(this, args);
        }

        #endregion

        #region IReceivesRegistrations implementation

        /// <summary>
        /// Adds new component registrations by use of a helper type.  Registrations are added within a callback which
        /// uses functionality from the helper.
        /// </summary>
        /// <seealso cref="T:CSF.FlexDi.Builders.IRegistrationHelper" />
        /// <param name="registrationActions">A callback which may use the functionality of the helper type.</param>
        public void AddRegistrations(Action<IRegistrationHelper> registrationActions)
        {
            if (registrationActions == null)
                throw new ArgumentNullException(nameof(registrationActions));

            AssertNotDisposed();

            var helper = new RegistrationHelper(constructorSelector);
            registrationActions(helper);

            AddRegistrations(helper.GetRegistrations());
        }

        /// <summary>
        /// Adds a collection of registration instances directly.
        /// </summary>
        /// <param name="registrations">A collection of registrations.</param>
        public void AddRegistrations(IEnumerable<IServiceRegistration> registrations)
        {
            if (registrations == null)
                throw new ArgumentNullException(nameof(registrations));

            AssertNotDisposed();

            foreach (var registration in registrations)
            {
                DoNotPermitReRegisteringAServiceWhichIsAlreadyCached(registration);
                registry.Add(registration);
            }
        }

        void DoNotPermitReRegisteringAServiceWhichIsAlreadyCached(IServiceRegistration registration)
        {
            var key = ServiceRegistrationKey.ForRegistration(registration);
            if (cache.Has(key))
            {
                var message = String.Format(Resources.ExceptionFormats.CannotReRegisterAfterResolution, registration);
                throw new ServiceReRegisteredAfterResolutionException(message);
            }
        }

        #endregion

        #region IInspectsServiceCache implementation

        /// <summary>
        /// Gets a value indicating whether a matching service instance is available in the resolver's cache
        /// or not.
        /// </summary>
        /// <remarks>
        /// <para>
        /// When this returns <c>true</c>, it means that a concrete instance of the service has already been
        /// resolved at least once, or was initially registered as an instance.  If the service were to be
        /// resolved using <see cref="T:CSF.FlexDi.IResolvesServices"/> then it would be served from the cache without
        /// creating a new instance.
        /// </para>
        /// <para>
        /// If this method returns <c>false</c> then, if the service were to be resolved, it would result in
        /// the creation of a new object.
        /// </para>
        /// </remarks>
        /// <returns>
        /// <c>true</c>, if an object of the matching <paramref name="type"/> (and optionally registration <paramref name="name"/>) is
        /// available in the cache, <c>false</c> otherwise.</returns>
        /// <param name="type">The service type.</param>
        /// <param name="name">An optional registration name.</param>
        public bool IsResolvedInstanceCached(Type type, string name = null)
            => Cache.Has(new ServiceRegistrationKey(type, name));

        #endregion

        #region IDisposable implementation

        bool disposedValue;

        /// <summary>
        /// Releases all resource used by the <see cref="T:CSF.FlexDi.Container"/> object.
        /// </summary>
        /// <param name="disposing">If set to <c>true</c> then this method call represents an explicit disposal.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    disposer.DisposeInstances(registry, cache);
                    GC.SuppressFinalize(this);
                }

                disposedValue = true;
            }
        }

        /// <summary>
        /// Releases all resource used by the <see cref="T:CSF.FlexDi.Container"/> object.
        /// </summary>
        /// <remarks>Call <see cref="Dispose()"/> when you are finished using the <see cref="T:CSF.FlexDi.Container"/>. The
        /// <see cref="Dispose()"/> method leaves the <see cref="T:CSF.FlexDi.Container"/> in an unusable state. After calling
        /// <see cref="Dispose()"/>, you must release all references to the <see cref="T:CSF.FlexDi.Container"/> so the
        /// garbage collector can reclaim the memory that the <see cref="T:CSF.FlexDi.Container"/> was occupying.</remarks>
        public void Dispose()
        {
            Dispose(true);
        }

        void AssertNotDisposed()
        {
            if (disposedValue)
                throw new ContainerDisposedException(Resources.ExceptionFormats.ContainerIsDisposed);
        }

        #endregion

        #region constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CSF.FlexDi.Container"/> class.
        /// </summary>
        /// <remarks>
        /// <para>
        /// All of the parameters to this method are optional.  Any which are not provided will be fulfilled using
        /// default implementations.
        /// </para>
        /// <para>
        /// Of the parameters which might be used, the most useful is likely to be <paramref name="options"/> and
        /// <paramref name="parentContainer"/>.
        /// </para>
        /// </remarks>
        /// <param name="registry">An optional service registry instance.</param>
        /// <param name="cache">An optional service cache instance.</param>
        /// <param name="resolver">An optional resolver instance.</param>
        /// <param name="disposer">An optional service disposer instance.</param>
        /// <param name="options">A set of container options.</param>
        /// <param name="parentContainer">An optional parent container - indicating that this container is the child of another.</param>
        /// <param name="resolverFactory">An optional resolver factory instance.</param>
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
            if (explicitOptions != null)
                return explicitOptions;

            var providesInfo = parent as IProvidesResolutionInfo;
            if (providesInfo != null)
                return providesInfo.Options;

            return ContainerOptions.Default;
        }

        IResolver GetResolver(ICreatesResolvers resolverFactory)
        {
            var factory = resolverFactory ?? new ResolverFactory();
            var output = factory.CreateResolver(this);

            if (output == null)
            {
                var message = String.Format(Resources.ExceptionFormats.ResolverFactoryMustNotReturnNull,
                                            nameof(ICreatesResolvers),
                                            nameof(IResolver));
                throw new ArgumentException(message, nameof(resolverFactory));
            }

            return output;
        }

        void PerformSelfRegistrations()
        {
            if (Options.SelfRegisterAResolver)
                SelfRegisterAResolver();

            if (Options.SelfRegisterTheRegistry)
                SelfRegisterTheRegistry();
        }

        void SelfRegisterAResolver()
        {
            var resolverRegistration = new InstanceRegistration(this)
            {
                DisposeWithContainer = false,
                ServiceType = typeof(IResolvesServices),
            };
            Registry.Add(resolverRegistration);

            var containerRegistration = new InstanceRegistration(this)
            {
                DisposeWithContainer = false,
                ServiceType = typeof(IContainer),
            };
            Registry.Add(containerRegistration);
        }

        void SelfRegisterTheRegistry()
        {
            var registration = new InstanceRegistration(this)
            {
                DisposeWithContainer = false,
                ServiceType = typeof(IReceivesRegistrations),
            };
            Registry.Add(registration);
        }

        #endregion

        #region static methods

        /// <summary>
        /// Convenience method which creates and returns an instance of <see cref="ContainerBuilder"/> with which to
        /// customise and then create a container.
        /// </summary>
        /// <returns>The container builder.</returns>
        public static ContainerBuilder CreateBuilder() => new ContainerBuilder();

        #endregion
    }
}
