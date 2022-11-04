//
//    ContainerBuilder.cs
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
using CSF.FlexDi.Resolution;

namespace CSF.FlexDi.Builders
{
    /// <summary>
    /// A 'fluent' builder type for the creation of a <see cref="Container"/> instance.
    /// </summary>
    public class ContainerBuilder
    {
        bool created;
        ContainerOptions options = new ContainerOptions();

        /// <summary>
        /// Indicates that the created container should not use non-public constructors.
        /// </summary>
        /// <seealso cref="ContainerOptions.UseNonPublicConstructors"/>
        /// <returns>The builder instance.</returns>
        public ContainerBuilder DoNotUseNonPublicConstructors() => UseNonPublicConstructors(false);

        /// <summary>
        /// Indicates that the created container should use non-public constructors.
        /// </summary>
        /// <seealso cref="ContainerOptions.UseNonPublicConstructors"/>
        /// <returns>The builder instance.</returns>
        /// <param name="useNonPublicConstructors">Indicates the value which will be used for <see cref="ContainerOptions.UseNonPublicConstructors"/>.</param>
        public ContainerBuilder UseNonPublicConstructors(bool useNonPublicConstructors = true)
        {
            AssertNotCreated();
            options.UseNonPublicConstructors = useNonPublicConstructors;
            return this;
        }

        /// <summary>
        /// Indicates that the created container should not resolve unregistered types.
        /// </summary>
        /// <seealso cref="ContainerOptions.ResolveUnregisteredTypes"/>
        /// <returns>The builder instance.</returns>
        public ContainerBuilder DoNotResolveUnregisteredTypes() => ResolveUnregisteredTypes(false);

        /// <summary>
        /// Indicates that the created container should resolve unregistered types.
        /// </summary>
        /// <seealso cref="ContainerOptions.ResolveUnregisteredTypes"/>
        /// <returns>The builder instance.</returns>
        /// <param name="resolveUnregisteredTypes">Indicates the value which will be used for <see cref="ContainerOptions.ResolveUnregisteredTypes"/>.</param>
        public ContainerBuilder ResolveUnregisteredTypes(bool resolveUnregisteredTypes = true)
        {
            AssertNotCreated();
            options.ResolveUnregisteredTypes = resolveUnregisteredTypes;
            return this;
        }

        /// <summary>
        /// Indicates that the created container should not use an instance cache.
        /// </summary>
        /// <seealso cref="ContainerOptions.UseInstanceCache"/>
        /// <returns>The builder instance.</returns>
        public ContainerBuilder DoNotUseInstanceCache() => UseInstanceCache(false);

        /// <summary>
        /// Indicates that the created container should use an instance cache.
        /// </summary>
        /// <seealso cref="ContainerOptions.UseInstanceCache"/>
        /// <returns>The builder instance.</returns>
        /// <param name="useInstanceCache">Indicates the value which will be used for <see cref="ContainerOptions.UseInstanceCache"/>.</param>
        public ContainerBuilder UseInstanceCache(bool useInstanceCache = true)
        {
            AssertNotCreated();
            options.UseInstanceCache = useInstanceCache;
            return this;
        }

        /// <summary>
        /// Indicates that the created container should not self-register itself as a resolver.
        /// </summary>
        /// <seealso cref="ContainerOptions.SelfRegisterAResolver"/>
        /// <returns>The builder instance.</returns>
        public ContainerBuilder DoNotSelfRegisterAResolver() => SelfRegisterAResolver(false);

        /// <summary>
        /// Indicates that the created container should self-register itself as a resolver.
        /// </summary>
        /// <seealso cref="ContainerOptions.SelfRegisterAResolver"/>
        /// <returns>The builder instance.</returns>
        /// <param name="selfRegisterAResolver">Indicates the value which will be used for <see cref="ContainerOptions.SelfRegisterAResolver"/>.</param>
        public ContainerBuilder SelfRegisterAResolver(bool selfRegisterAResolver = true)
        {
            AssertNotCreated();
            options.SelfRegisterAResolver = selfRegisterAResolver;
            return this;
        }

        /// <summary>
        /// Indicates that the created container should not self-register itself as a registry.
        /// </summary>
        /// <seealso cref="ContainerOptions.SelfRegisterTheRegistry"/>
        /// <returns>The builder instance.</returns>
        public ContainerBuilder DoNotSelfRegisterTheRegistry() => SelfRegisterTheRegistry(false);

        /// <summary>
        /// Indicates that the created container should self-register itself as a registry
        /// </summary>
        /// <seealso cref="ContainerOptions.SelfRegisterTheRegistry"/>
        /// <returns>The builder instance.</returns>
        /// <param name="selfRegisterTheRegistry">Indicates the value which will be used for <see cref="ContainerOptions.SelfRegisterTheRegistry"/>.</param>
        public ContainerBuilder SelfRegisterTheRegistry(bool selfRegisterTheRegistry = true)
        {
            AssertNotCreated();
            options.SelfRegisterTheRegistry = selfRegisterTheRegistry;
            return this;
        }

        /// <summary>
        /// Indicates that the created container should not throw exceptions on circular dependencies.
        /// </summary>
        /// <seealso cref="ContainerOptions.ThrowOnCircularDependencies"/>
        /// <returns>The builder instance.</returns>
        public ContainerBuilder DoNotThrowOnCircularDependencies() => ThrowOnCircularDependencies(false);

        /// <summary>
        /// Indicates that the created container should throw exceptions on circular dependencies.
        /// </summary>
        /// <seealso cref="ContainerOptions.ThrowOnCircularDependencies"/>
        /// <returns>The builder instance.</returns>
        /// <param name="throwOnCircularDependencies">Indicates the value which will be used for <see cref="ContainerOptions.ThrowOnCircularDependencies"/>.</param>
        public ContainerBuilder ThrowOnCircularDependencies(bool throwOnCircularDependencies = true)
        {
            AssertNotCreated();
            options.ThrowOnCircularDependencies = throwOnCircularDependencies;
            return this;
        }

        /// <summary>
        /// Indicates that the created container should not support the resolution of named instance dictionaries.
        /// </summary>
        /// <seealso cref="ContainerOptions.SupportResolvingNamedInstanceDictionaries"/>
        /// <returns>The builder instance.</returns>
        public ContainerBuilder DoNotSupportResolvingNamedInstanceDictionaries() => SupportResolvingNamedInstanceDictionaries(false);

        /// <summary>
        /// Indicates that the created container should support the resolution of named instance dictionaries.
        /// </summary>
        /// <seealso cref="ContainerOptions.SupportResolvingNamedInstanceDictionaries"/>
        /// <returns>The builder instance.</returns>
        /// <param name="supportResolvingNamedInstanceDictionaries">Indicates the value which will be used for <see cref="ContainerOptions.SupportResolvingNamedInstanceDictionaries"/>.</param>
        public ContainerBuilder SupportResolvingNamedInstanceDictionaries(bool supportResolvingNamedInstanceDictionaries = true)
        {
            AssertNotCreated();
            options.SupportResolvingNamedInstanceDictionaries = supportResolvingNamedInstanceDictionaries;
            return this;
        }

        /// <summary>
        /// Indicates that the created container should not support the lazy resolution of components.
        /// </summary>
        /// <seealso cref="ContainerOptions.SupportResolvingLazyInstances"/>
        /// <returns>The builder instance.</returns>
        public ContainerBuilder DoNotSupportResolvingLazyInstances() => SupportResolvingLazyInstances(false);

        /// <summary>
        /// Indicates that the created container should support the lazy resolution of components.
        /// </summary>
        /// <seealso cref="ContainerOptions.SupportResolvingLazyInstances"/>
        /// <returns>The builder instance.</returns>
        /// <param name="supportResolvingLazyInstances">Indicates the value which will be used for <see cref="ContainerOptions.SupportResolvingLazyInstances"/>.</param>
        public ContainerBuilder SupportResolvingLazyInstances(bool supportResolvingLazyInstances = true)
        {
            AssertNotCreated();
            options.SupportResolvingLazyInstances = supportResolvingLazyInstances;
            return this;
        }

        /// <summary>
        /// Indicates that the created container should not make all resolution optional.
        /// </summary>
        /// <seealso cref="ContainerOptions.MakeAllResolutionOptional"/>
        /// <returns>The builder instance.</returns>
        public ContainerBuilder DoNotMakeAllResolutionOptional() => MakeAllResolutionOptional(false);

        /// <summary>
        /// Indicates that the created container should make all resolution optional.
        /// </summary>
        /// <seealso cref="ContainerOptions.MakeAllResolutionOptional"/>
        /// <returns>The builder instance.</returns>
        /// <param name="makeAllResolutionOptional">Indicates the value which will be used for <see cref="ContainerOptions.MakeAllResolutionOptional"/>.</param>
        public ContainerBuilder MakeAllResolutionOptional(bool makeAllResolutionOptional = true)
        {
            AssertNotCreated();
            options.MakeAllResolutionOptional = makeAllResolutionOptional;
            return this;
        }

        /// <summary>
        /// Indicates that the container should be created using a custom implementation of <see cref="ICreatesResolvers"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is an advanced customisation, as it allows direct control over the resolution process used by
        /// FlexDi.  This includes the ability to override almost all of the options set by this builder and the
        /// <see cref="ContainerOptions"/> type.
        /// </para>
        /// <para>
        /// You should look at the existing <see cref="ResolverFactory"/> if you wish to create your own resolver, and
        /// definitely consider subclassing the existing resolver or at least <see cref="ResolverFactoryBase"/>.
        /// </para>
        /// <para>
        /// Those existing resolver factory types make use of the proxy &amp; chain of responsibility patterns in order
        /// to perform resolution.  Most of the individual <see cref="IResolver"/> implementations proxy other implementations
        /// and modify the results of the overall operation.
        /// </para>
        /// </remarks>
        /// <returns>The builder instance.</returns>
        /// <param name="resolverFactory">A custom resolver factory implementation.</param>
        public ContainerBuilder UseCustomResolverFactory(ICreatesResolvers resolverFactory)
        {
            AssertNotCreated();
            options.ResolverFactory = resolverFactory;
            return this;
        }

        /// <summary>
        /// Indicates that the container should be created using a custom implementation of <see cref="ICreatesRegistry"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The container defaults to an instance of <see cref="Registration.RegistryFactory"/>, but it may be substituted with an
        /// alternative implementation if desired.
        /// </para>
        /// </remarks>
        public ContainerBuilder UseCustomRegistryFactory(ICreatesRegistry registryFactory)
        {
            AssertNotCreated();
            options.RegistryFactory = registryFactory;
            return this;
        }

        /// <summary>
        /// Indicates that the container should be created using a custom implementation of <see cref="ICreatesInstanceCache"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The container defaults to an instance of <see cref="InstanceCacheFactory"/>, but it may be substituted with an
        /// alternative implementation if desired.
        /// </para>
        /// </remarks>
        public ContainerBuilder UseCustomCacheFactory(ICreatesInstanceCache cache)
        {
            AssertNotCreated();
            options.CacheFactory = cache;
            return this;
        }

        /// <summary>
        /// Indicates that the container should be created using a custom implementation of <see cref="IFulfilsResolutionRequests"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The container defaults to <see langword="null" />, which indicates that the <see cref="ContainerOptions.ResolverFactory"/>
        /// will be used to create an instance of a resolver.  If this is set to any non-null value then the
        /// resolver factory will not be used and this instance will be used instead.
        /// </para>
        /// </remarks>
        public ContainerBuilder UseCustomResolver(IFulfilsResolutionRequests resolver)
        {
            AssertNotCreated();
            options.Resolver = resolver;
            return this;
        }

        /// <summary>
        /// Indicates that the container should be created using a custom implementation of <see cref="IDisposesOfResolvedInstances"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The container defaults to an instance of <see cref="ServiceInstanceDisposer"/>, but it may be substituted with an
        /// alternative implementation if desired.
        /// </para>
        /// </remarks>
        public ContainerBuilder UseCustomDisposer(IDisposesOfResolvedInstances disposer)
        {
            AssertNotCreated();
            options.Disposer = disposer;
            return this;
        }

        /// <summary>
        /// Indicates that the container should be created using a custom implementation of <see cref="ISelectsConstructor"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The container defaults to <see langword="null" />, which indicates that an instance of <see cref="ConstructorWithMostParametersSelector"/>
        /// should be used, respecting the option <see cref="ContainerOptions.UseNonPublicConstructors"/>.
        /// If this is set to any other value, then that implementation will be used, and the <see cref="ContainerOptions.UseNonPublicConstructors"/>
        /// setting will not be honoured.
        /// </para>
        /// </remarks>
        public ContainerBuilder UseCustomConstructorSelector(ISelectsConstructor constructorSelector)
        {
            AssertNotCreated();
            options.ConstructorSelector = constructorSelector;
            return this;
        }

        /// <summary>
        /// Builds and returns an instance of <see cref="ContainerOptions"/> from the current state of this builder instance.
        /// </summary>
        /// <returns>The container options.</returns>
        public ContainerOptions BuildOptions()
        {
            AssertNotCreated();
            created = true;
            return options;
        }

        /// <summary>
        /// Builds and returns a <see cref="Container"/> instance from the current state of this builder instance.
        /// </summary>
        public IContainer Build() => BuildOptions().GetContainer();

        void AssertNotCreated()
        {
            if(created)
            {
                var message = String.Format(Resources.ExceptionFormats.ContainerOptionsBuilderCannotBeUsedMoreThanOnce, nameof(ContainerBuilder));
                throw new InvalidOperationException(message);
            }
        }
    }
}
