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
using CSF.FlexDi.Resolution;

namespace CSF.FlexDi.Builders
{
  /// <summary>
  /// A 'fluent' builder type for the creation of a <see cref="Container"/> instance.
  /// </summary>
  public class ContainerBuilder
  {
    bool
      useNonPublicConstructors,
      resolveUnregisteredTypes,
      useInstanceCache,
      throwOnCircularDependencies,
      supportResolvingNamedInstanceDictionaries,
      selfRegisterAResolver,
      selfRegisterTheRegistry,
      supportResolvingLazyInstances,
      makeAllResolutionOptional;
    ICreatesResolvers resolverFactory;

    /// <summary>
    /// Indicates that the created container should not use non-public constructors.
    /// </summary>
    /// <seealso cref="ContainerOptions.UseNonPublicConstructors"/>
    /// <returns>The builder instance.</returns>
    public ContainerBuilder DoNotUseNonPublicConstructors()
    {
      useNonPublicConstructors = false;
      return this;
    }

    /// <summary>
    /// Indicates that the created container should use non-public constructors.
    /// </summary>
    /// <seealso cref="ContainerOptions.UseNonPublicConstructors"/>
    /// <returns>The builder instance.</returns>
    /// <param name="useNonPublicConstructors">Indicates the value which will be used for <see cref="ContainerOptions.UseNonPublicConstructors"/>.</param>
    public ContainerBuilder UseNonPublicConstructors(bool useNonPublicConstructors = true)
    {
      this.useNonPublicConstructors = useNonPublicConstructors;
      return this;
    }

    /// <summary>
    /// Indicates that the created container should not resolve unregistered types.
    /// </summary>
    /// <seealso cref="ContainerOptions.ResolveUnregisteredTypes"/>
    /// <returns>The builder instance.</returns>
    public ContainerBuilder DoNotResolveUnregisteredTypes()
    {
      resolveUnregisteredTypes = false;
      return this;
    }

    /// <summary>
    /// Indicates that the created container should resolve unregistered types.
    /// </summary>
    /// <seealso cref="ContainerOptions.ResolveUnregisteredTypes"/>
    /// <returns>The builder instance.</returns>
    /// <param name="resolveUnregisteredTypes">Indicates the value which will be used for <see cref="ContainerOptions.ResolveUnregisteredTypes"/>.</param>
    public ContainerBuilder ResolveUnregisteredTypes(bool resolveUnregisteredTypes = true)
    {
      this.resolveUnregisteredTypes = resolveUnregisteredTypes;
      return this;
    }

    /// <summary>
    /// Indicates that the created container should not use an instance cache.
    /// </summary>
    /// <seealso cref="ContainerOptions.UseInstanceCache"/>
    /// <returns>The builder instance.</returns>
    public ContainerBuilder DoNotUseInstanceCache()
    {
      useInstanceCache = false;
      return this;
    }

    /// <summary>
    /// Indicates that the created container should use an instance cache.
    /// </summary>
    /// <seealso cref="ContainerOptions.UseInstanceCache"/>
    /// <returns>The builder instance.</returns>
    /// <param name="useInstanceCache">Indicates the value which will be used for <see cref="ContainerOptions.UseInstanceCache"/>.</param>
    public ContainerBuilder UseInstanceCache(bool useInstanceCache = true)
    {
      this.useInstanceCache = useInstanceCache;
      return this;
    }

    /// <summary>
    /// Indicates that the created container should not self-register itself as a resolver.
    /// </summary>
    /// <seealso cref="ContainerOptions.SelfRegisterAResolver"/>
    /// <returns>The builder instance.</returns>
    public ContainerBuilder DoNotSelfRegisterAResolver()
    {
      return SelfRegisterAResolver(false);
    }

    /// <summary>
    /// Indicates that the created container should self-register itself as a resolver.
    /// </summary>
    /// <seealso cref="ContainerOptions.SelfRegisterAResolver"/>
    /// <returns>The builder instance.</returns>
    /// <param name="selfRegisterTheRegistry">Indicates the value which will be used for <see cref="ContainerOptions.SelfRegisterAResolver"/>.</param>
    public ContainerBuilder SelfRegisterAResolver(bool selfRegisterTheRegistry = true)
    {
      this.selfRegisterTheRegistry = selfRegisterTheRegistry;
      return this;
    }

    /// <summary>
    /// Indicates that the created container should not self-register itself as a registry.
    /// </summary>
    /// <seealso cref="ContainerOptions.SelfRegisterTheRegistry"/>
    /// <returns>The builder instance.</returns>
    public ContainerBuilder DoNotSelfRegisterTheRegistry()
    {
      return SelfRegisterTheRegistry(false);
    }

    /// <summary>
    /// Indicates that the created container should self-register itself as a registry
    /// </summary>
    /// <seealso cref="ContainerOptions.SelfRegisterTheRegistry"/>
    /// <returns>The builder instance.</returns>
    /// <param name="selfRegisterTheRegistry">Indicates the value which will be used for <see cref="ContainerOptions.SelfRegisterTheRegistry"/>.</param>
    public ContainerBuilder SelfRegisterTheRegistry(bool selfRegisterTheRegistry = true)
    {
      this.selfRegisterTheRegistry = selfRegisterTheRegistry;
      return this;
    }

    /// <summary>
    /// Indicates that the created container should not throw exceptions on circular dependencies.
    /// </summary>
    /// <seealso cref="ContainerOptions.ThrowOnCircularDependencies"/>
    /// <returns>The builder instance.</returns>
    public ContainerBuilder DoNotThrowOnCircularDependencies()
    {
      throwOnCircularDependencies = false;
      return this;
    }

    /// <summary>
    /// Indicates that the created container should throw exceptions on circular dependencies.
    /// </summary>
    /// <seealso cref="ContainerOptions.ThrowOnCircularDependencies"/>
    /// <returns>The builder instance.</returns>
    /// <param name="throwOnCircularDependencies">Indicates the value which will be used for <see cref="ContainerOptions.ThrowOnCircularDependencies"/>.</param>
    public ContainerBuilder ThrowOnCircularDependencies(bool throwOnCircularDependencies = true)
    {
      this.throwOnCircularDependencies = throwOnCircularDependencies;
      return this;
    }

    /// <summary>
    /// Indicates that the created container should not support the resolution of named instance dictionaries.
    /// </summary>
    /// <seealso cref="ContainerOptions.SupportResolvingNamedInstanceDictionaries"/>
    /// <returns>The builder instance.</returns>
    public ContainerBuilder DoNotSupportResolvingNamedInstanceDictionaries()
    {
      supportResolvingNamedInstanceDictionaries = false;
      return this;
    }

    /// <summary>
    /// Indicates that the created container should support the resolution of named instance dictionaries.
    /// </summary>
    /// <seealso cref="ContainerOptions.SupportResolvingNamedInstanceDictionaries"/>
    /// <returns>The builder instance.</returns>
    /// <param name="supportResolvingNamedInstanceDictionaries">Indicates the value which will be used for <see cref="ContainerOptions.SupportResolvingNamedInstanceDictionaries"/>.</param>
    public ContainerBuilder SupportResolvingNamedInstanceDictionaries(bool supportResolvingNamedInstanceDictionaries = true)
    {
      this.supportResolvingNamedInstanceDictionaries = supportResolvingNamedInstanceDictionaries;
      return this;
    }

    /// <summary>
    /// Indicates that the created container should not support the lazy resolution of components.
    /// </summary>
    /// <seealso cref="ContainerOptions.SupportResolvingLazyInstances"/>
    /// <returns>The builder instance.</returns>
    public ContainerBuilder DoNotSupportResolvingLazyInstances()
    {
      supportResolvingLazyInstances = false;
      return this;
    }

    /// <summary>
    /// Indicates that the created container should support the lazy resolution of components.
    /// </summary>
    /// <seealso cref="ContainerOptions.SupportResolvingLazyInstances"/>
    /// <returns>The builder instance.</returns>
    /// <param name="supportResolvingLazyInstances">Indicates the value which will be used for <see cref="ContainerOptions.SupportResolvingLazyInstances"/>.</param>
    public ContainerBuilder SupportResolvingLazyInstances(bool supportResolvingLazyInstances = true)
    {
      this.supportResolvingLazyInstances = supportResolvingLazyInstances;
      return this;
    }

    /// <summary>
    /// Indicates that the created container should not make all resolution optional.
    /// </summary>
    /// <seealso cref="ContainerOptions.MakeAllResolutionOptional"/>
    /// <returns>The builder instance.</returns>
    public ContainerBuilder DoNotMakeAllResolutionOptional()
    {
      makeAllResolutionOptional = false;
      return this;
    }

    /// <summary>
    /// Indicates that the created container should make all resolution optional.
    /// </summary>
    /// <seealso cref="ContainerOptions.MakeAllResolutionOptional"/>
    /// <returns>The builder instance.</returns>
    /// <param name="makeAllResolutionOptional">Indicates the value which will be used for <see cref="ContainerOptions.MakeAllResolutionOptional"/>.</param>
    public ContainerBuilder MakeAllResolutionOptional(bool makeAllResolutionOptional = true)
    {
      this.makeAllResolutionOptional = makeAllResolutionOptional;
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
      if(resolverFactory == null)
        throw new ArgumentNullException(nameof(resolverFactory));
      
      this.resolverFactory = resolverFactory;
      return this;
    }

    /// <summary>
    /// Builds and returns a <see cref="Container"/> instance from the current state of this builder instance.
    /// </summary>
    public Container Build()
    {
      return new Container(options: GetContainerOptions(),
                           resolverFactory: resolverFactory);
    }

    ContainerOptions GetContainerOptions()
    {
      return new ContainerOptions(useNonPublicConstructors,
                                  resolveUnregisteredTypes,
                                  useInstanceCache,
                                  throwOnCircularDependencies,
                                  supportResolvingNamedInstanceDictionaries,
                                  selfRegisterAResolver,
                                  selfRegisterTheRegistry,
                                  supportResolvingLazyInstances,
                                  makeAllResolutionOptional);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ContainerBuilder"/> class.
    /// </summary>
    public ContainerBuilder()
    {
      useNonPublicConstructors = ContainerOptions.Default.UseNonPublicConstructors;
      resolveUnregisteredTypes = ContainerOptions.Default.ResolveUnregisteredTypes;
      useInstanceCache = ContainerOptions.Default.UseInstanceCache;
      throwOnCircularDependencies = ContainerOptions.Default.ThrowOnCircularDependencies;
      supportResolvingNamedInstanceDictionaries = ContainerOptions.Default.SupportResolvingNamedInstanceDictionaries;
      selfRegisterAResolver = ContainerOptions.Default.SelfRegisterAResolver;
      selfRegisterTheRegistry = ContainerOptions.Default.SelfRegisterTheRegistry;
      supportResolvingLazyInstances = ContainerOptions.Default.SupportResolvingLazyInstances;
      makeAllResolutionOptional = ContainerOptions.Default.MakeAllResolutionOptional;
    }
  }
}
