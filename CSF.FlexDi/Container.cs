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
        readonly IFulfilsResolutionRequests resolver;
        readonly IServiceRegistrationProvider registryStack;
        readonly IContainer parentContainer;

        bool disposedValue = false;

        /// <inheritdoc/>
        public ICachesResolvedServiceInstances Cache { get; }

        /// <inheritdoc/>
        public IRegistersServices Registry { get; }

        /// <inheritdoc/>
        public ContainerOptions Options { get; }

        /// <inheritdoc/>
        public IProvidesResolutionInfo Parent => parentContainer as IProvidesResolutionInfo;

        /// <inheritdoc/>
        public ISelectsConstructor ConstructorSelector { get; }

        /// <inheritdoc/>
        public ResolutionResult TryResolve(ResolutionRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            AssertNotDisposed();

            return resolver.Resolve(request);
        }

        /// <inheritdoc/>
        public bool HasRegistration(Type serviceType, string name = null)
        {
            AssertNotDisposed();

            return registryStack.HasRegistration(new ServiceRegistrationKey(serviceType, name));
        }

        /// <inheritdoc/>
        public IReadOnlyCollection<IServiceRegistration> GetRegistrations(Type serviceType = null)
        {
            AssertNotDisposed();

            return registryStack.GetAll(serviceType);
        }

        /// <inheritdoc/>
        public IContainer CreateChildContainer() => new Container(options: Options, parentContainer: this);

        /// <inheritdoc/>
        public event EventHandler<ServiceResolutionEventArgs> ServiceResolved;

        void InvokeServiceResolved(object sender, ServiceResolutionEventArgs args)
        {
            ServiceResolved?.Invoke(this, args);
        }

        /// <inheritdoc/>
        public void AddRegistrations(IEnumerable<IServiceRegistration> registrations)
        {
            if (registrations == null)
                throw new ArgumentNullException(nameof(registrations));

            AssertNotDisposed();

            foreach (var registration in registrations)
            {
                DoNotPermitReRegisteringAServiceWhichIsAlreadyCached(registration);
                Registry.Add(registration);
            }
        }

        void DoNotPermitReRegisteringAServiceWhichIsAlreadyCached(IServiceRegistration registration)
        {
            var key = ServiceRegistrationKey.FromRegistration(registration);
            if (Cache.Has(key))
            {
                var message = String.Format(Resources.ExceptionFormats.CannotReRegisterAfterResolution, registration);
                throw new ServiceReRegisteredAfterResolutionException(message);
            }
        }

        /// <inheritdoc/>
        public bool IsResolvedInstanceCached(Type type, string name = null)
            => Cache.Has(new ServiceRegistrationKey(type, name));

        /// <summary>
        /// Releases all resource used by the <see cref="Container"/> object.
        /// </summary>
        /// <param name="disposing">If set to <c>true</c> then this method call represents an explicit disposal.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Options.Disposer.DisposeInstances(Registry, Cache);
                }

                disposedValue = true;
            }
        }

        /// <summary>
        /// Releases all resource used by the <see cref="Container"/> object.
        /// </summary>
        /// <remarks>Call <see cref="Dispose()"/> when you are finished using the <see cref="Container"/>. The
        /// <see cref="Dispose()"/> method leaves the <see cref="Container"/> in an unusable state. After calling
        /// <see cref="Dispose()"/>, you must release all references to the <see cref="Container"/> so the
        /// garbage collector can reclaim the memory that the <see cref="Container"/> was occupying.</remarks>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void AssertNotDisposed()
        {
            if (disposedValue)
                throw new ContainerDisposedException(Resources.ExceptionFormats.ContainerIsDisposed);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Container"/> class.
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
        /// <param name="options">A set of container options.</param>
        /// <param name="parentContainer">An optional parent container - indicating that this container is the child of another.</param>
        public Container(ContainerOptions options = null,
                         IContainer parentContainer = null)
        {
            this.parentContainer = parentContainer;
            Options = options ?? ((parentContainer is IProvidesResolutionInfo resolutionInfoProvider) ? resolutionInfoProvider.Options : new ContainerOptions());

            ConstructorSelector = Options.GetConstructorSelector();
            Cache = Options.CacheFactory.GetCache();
            Registry = Options.RegistryFactory.GetRegistry();
            registryStack = RegistryStackFactory.CreateRegistryStack(this);
            
            resolver = Options.Resolver ?? Options.ResolverFactory.CreateResolver(this);
            if (this.resolver == null)
            {
                var message = String.Format(Resources.ExceptionFormats.ResolverFactoryMustNotReturnNull,
                                            nameof(ICreatesResolvers),
                                            nameof(IResolver));
                throw new ArgumentException(message, nameof(options));
            }
            this.resolver.ServiceResolved += InvokeServiceResolved;

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

        /// <summary>
        /// Convenience method which creates and returns an instance of <see cref="ContainerBuilder"/> with which to
        /// customise and then create a container.
        /// </summary>
        /// <returns>The container builder.</returns>
        public static ContainerBuilder CreateBuilder() => new ContainerBuilder();
    }
}
