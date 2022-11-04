//
//    RegistrationBuilder.cs
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

namespace CSF.FlexDi.Builders
{
    /// <summary>
    /// Implementation of the various registration builder interfaces.  This assists in the building of a single
    /// component/service registration.
    /// </summary>
    public class RegistrationBuilder : IAsBuilder,
                                       IAsBuilderWithCacheability,
                                       IRegistrationOptionsBuilder,
                                       IRegistrationOptionsBuilderWithCacheability
    {
        readonly ServiceRegistration registration;

        /// <summary>
        /// Indicates that the component will be registered 'as' the specified type.
        /// </summary>
        /// <param name="serviceType">The service type for which to register the component.</param>
        /// <returns>A builder with which to specify registration options.</returns>
        public IRegistrationOptionsBuilder As(Type serviceType)
        {
            if(serviceType == null)
                throw new ArgumentNullException(nameof(serviceType));
            registration.ServiceType = serviceType;
            return this;
        }

        /// <summary>
        /// Indicates that the component will be registered 'as' the specified generic type parameter.
        /// </summary>
        /// <typeparam name="T">The service type for which to register the component.</typeparam>
        /// <returns>A builder with which to specify registration options.</returns>
        public IRegistrationOptionsBuilder As<T>()
            where T : class
            => As(typeof(T));

        /// <summary>
        /// Indicates that the component will be registered as its own type (and not under a more general type).
        /// </summary>
        /// <returns>A builder with which to specify registration options.</returns>
        public IRegistrationOptionsBuilderWithCacheability AsOwnType()
        {
            if(!(registration is TypedRegistration typedRegistration))
            {
                var message = String.Format(Resources.ExceptionFormats.AsOwnTypeOnlyForTypedRegistrations,
                                            nameof(TypedRegistration));
                throw new InvalidOperationException(message);
            }

            registration.ServiceType = typedRegistration.ImplementationType;
            return this;
        }

        /// <summary>
        /// Indicates that instances created from the current registration should not be cached in the container's
        /// instance cache.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This overrides the setting in <see cref="ContainerOptions.UseInstanceCache"/>, such that the current registration
        /// will always bypass the cache, even when it is otherwise-enabled.
        /// </para>
        /// <para>
        /// Setting the registration not-cacheable means that every time a component is resolved, a new instance will
        /// be created (clearly, this is applicable only to Type and Factory registrations).
        /// </para>
        /// <para>
        /// If the registration is marked as non-cacheable, then <see cref="DoNotDisposeWithContainer"/> and
        /// <see cref="DisposeWithContainer"/> will have no effect upon the registration.  If the instances are not cached
        /// then they will never be disposed.
        /// </para>
        /// </remarks>
        /// <returns>The builder instance.</returns>
        public IRegistrationOptionsBuilderWithCacheability NotCacheable()
        {
            return Cacheable(false);
        }

        /// <summary>
        /// Indicates that instances created from the current registration should be cached in the container's
        /// instance cache (assuming it is enabled).
        /// </summary>
        /// <remarks>
        /// <para>
        /// By default, instances are cached within the container's instance cache - assuming that option
        /// <see cref="ContainerOptions.UseInstanceCache"/> is enabled (which it is by default).
        /// </para>
        /// </remarks>
        /// <param name="cacheable">If set to <c>true</c> then the current registration is cacheable.</param>
        public IRegistrationOptionsBuilderWithCacheability Cacheable(bool cacheable = true)
        {
            registration.Cacheable = cacheable;
            if(!registration.Cacheable) registration.DisposeWithContainer = false;
            return this;
        }

        /// <summary>
        /// Indicates that the current registration is to be registered under a specified name.
        /// </summary>
        /// <returns>The builder instance.</returns>
        /// <param name="name">The name of the registration.</param>
        public IRegistrationOptionsBuilder WithName(string name)
        {
            registration.Name = name;
            return this;
        }

        /// <summary>
        /// Indicates that an instance of the component which is created by a container should not be disposed along with that
        /// container (if the component implements <c>IDisposable</c>).  This overrides the default behaviour.
        /// </summary>
        /// <remarks>
        /// <para>
        /// By default, when a container is disposed, any components which were created by that container and contained
        /// within its cache are disposed along with the container.  This depends upon the option
        /// <see cref="ContainerOptions.UseInstanceCache"/> being enabled (it is by default).
        /// </para>
        /// <para>
        /// Disabling this option means that the automatic disposal does not occur, meaning that the client
        /// developer must take responsibility for performing the disposal of any components which had been resolved
        /// from that container.
        /// </para>
        /// </remarks>
        /// <returns>The builder instance.</returns>
        public IRegistrationOptionsBuilder DoNotDisposeWithContainer()
        {
            registration.DisposeWithContainer = false;
            return this;
        }

        /// <summary>
        /// Indicates that this component should be disposed along with the container which resolves it.
        /// </summary>
        /// <seealso cref="DoNotDisposeWithContainer"/>
        /// <returns>The builder instance.</returns>
        /// <param name="disposeWithContainer">If set to <c>true</c> then the component will be disposed with the container.</param>
        public IRegistrationOptionsBuilder DisposeWithContainer(bool disposeWithContainer = true)
        {
            registration.DisposeWithContainer = disposeWithContainer;
            return this;
        }

        IRegistrationOptionsBuilderWithCacheability IAsBuilderWithCacheability.As(Type serviceType)
        {
            if(serviceType == null)
                throw new ArgumentNullException(nameof(serviceType));
            registration.ServiceType = serviceType;
            return this;
        }

        IRegistrationOptionsBuilderWithCacheability IAsBuilderWithCacheability.As<T>()
        {
            registration.ServiceType = typeof(T);
            return this;
        }

        IRegistrationOptionsBuilderWithCacheability IRegistrationOptionsBuilderWithCacheability.WithName(string name)
        {
            registration.Name = name;
            return this;
        }

        IRegistrationOptionsBuilderWithCacheability IRegistrationOptionsBuilderWithCacheability.DoNotDisposeWithContainer()
        {
            registration.DisposeWithContainer = false;
            return this;
        }

        IRegistrationOptionsBuilderWithCacheability IRegistrationOptionsBuilderWithCacheability.DisposeWithContainer(bool disposeWithContainer)
        {
            registration.DisposeWithContainer = disposeWithContainer;
            return this;
        }

        IRegistrationOptionsBuilder IAsBuilder.As<T>()
        {
            registration.ServiceType = typeof(T);
            return this;
        }

        IRegistrationOptionsBuilder IAsBuilder.As(Type serviceType)
        {
            if(serviceType == null)
                throw new ArgumentNullException(nameof(serviceType));
            registration.ServiceType = serviceType;
            return this;
        }

        IRegistrationOptionsBuilder IAsBuilder.AsOwnType()
        {
            // Intentional no-op, we already have the correct type
            return this;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationBuilder"/> class.
        /// </summary>
        /// <param name="registration">The registration which is being built.</param>
        public RegistrationBuilder(ServiceRegistration registration)
        {
            if(registration == null)
                throw new ArgumentNullException(nameof(registration));

            this.registration = registration;
        }
    }
}
