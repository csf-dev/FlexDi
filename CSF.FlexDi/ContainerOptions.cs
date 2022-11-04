//
//    ContainerOptions.cs
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
namespace CSF.FlexDi
{
  /// <summary>
  /// Represents the available options which may be configured when creating a <see cref="Container"/> instance.
  /// These influence the behaviour of that container.
  /// </summary>
  public class ContainerOptions
  {
    static readonly ContainerOptions defaultOptions;

    /// <summary>
    /// Gets a value indicating whether the container should make use of non-public constructors when resolving
    /// instances registered using type registrations.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Components that are registered as types (instead of factories or instances) must be instantiated dynamically.
    /// In constructing an instance, the FlexDi container needs to choose a constructor to execute.
    /// This setting controls whether or not non-public constructors are considered during that selection process.
    /// </para>
    /// <para>
    /// The default value for this option is <c>false</c>.
    /// </para>
    /// </remarks>
    /// <value><c>true</c> if the container is permitted to use non-public constructors; otherwise, <c>false</c>.</value>
    public bool UseNonPublicConstructors { get; private set; }

    /// <summary>
    /// Gets a value indicating whether or not the container should attempt to fulfil resolution requests for types
    /// which have not been registered first.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If this setting is <c>false</c> then all components which are resolved (including their dependencies) must
    /// first be registered with the container.  If a component or dependency has not been registered then a
    /// <see cref="Resolution.ResolutionException"/> will be raised.
    /// </para>
    /// <para>
    /// If, on the other hand, this setting is <c>true</c>, then an attempt will be made to resolve the components
    /// regardless of there being no registration.  This resolution attempt will be performed as if there had been a simple
    /// type registration for that component.  In this manner, interfaces and abstract types cannot be resolved in this
    /// manner, but classes may be.
    /// </para>
    /// <para>
    /// If registration-less resolution fails, then a resolution exception will be raised.
    /// </para>
    /// <para>
    /// The default value for this option is <c>false</c>.
    /// </para>
    /// </remarks>
    /// <value><c>true</c> if the container should resolve unregistered types; otherwise, <c>false</c>.</value>
    public bool ResolveUnregisteredTypes { get; private set; }

    /// <summary>
    /// Gets a value that indicates whether or not a container will cache component instances which have been created,
    /// and - on subsequent attempts to resolve the same component - will return that cached instance.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Usually, you will want this setting enabled; if it is set to <c>false</c> then every time a resolution occurs
    /// for a factory or type registration, a new component instance will be created.
    /// </para>
    /// <para>
    /// When this setting is true, subsequent resolutions of the same component type will return the already-resolved
    /// instance.  This setting must also be true if the container is to dispose of created components (which are
    /// <c>IDisposable</c> when the container is disposed.
    /// </para>
    /// <para>
    /// The default value for this option is <c>true</c>.
    /// </para>
    /// </remarks>
    /// <value><c>true</c> if the instance cache is enabled; otherwise, <c>false</c>.</value>
    public bool UseInstanceCache { get; private set; }

    /// <summary>
    /// Gets a value indicating whether or not the container should detect and raise an exception when a circular
    /// dependency is encountered during component resolution.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Usually, you will want this setting enabled.  If it is disabled then a circular dependency during resolution
    /// will usually result in a stack overflow.  When this is enabled, circular dependencies are detected and will
    /// result in the raising of a <see cref="CircularDependencyException"/> being raised.
    /// </para>
    /// <para>
    /// You may wish to disable this if you are replacing some or all of the resolution mechanism with your own
    /// implementation, but please exercise caution if you are.
    /// </para>
    /// <para>
    /// The default value for this option is <c>true</c>.
    /// </para>
    /// </remarks>
    /// <value><c>true</c> if the resolution process should throw on circular dependencies; otherwise, <c>false</c>.</value>
    public bool ThrowOnCircularDependencies { get; private set; }

    /// <summary>
    /// Gets a value indicating whether or not the container will support the resolution of a 'named instance dictionary'.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A named instance dictionary has one of the two following signatures: <c>IDictionary&lt;string,TComponent&gt;</c>
    /// or <c>IDictionary&lt;TEnum,TComponent&gt;</c>.  In both cases, <c>TComponent</c> refers to a component type.
    /// In the second case only, <c>TEnum</c> refers to an enumeration type.
    /// </para>
    /// <para>
    /// The 'key' (either string or enumeration constant) to these dictionaries refers to the registration name, under
    /// which a component was registered.  The value is the resolved component instance, matching that registration name.
    /// In the case where an enumeration is used - the enumeration constant must match the string registration name (albeit
    /// this match is case insensitive).
    /// </para>
    /// <para>
    /// Resolving a named instance dictionary is essentially "give me all of the TComponent that I registered,
    /// indexed by the names under which they were registered".
    /// </para>
    /// <para>
    /// If this setting is enabled then named instance dictionaries may be resolved for any registered type.  If it is
    /// disabled then this feature will be deactivated.
    /// </para>
    /// <para>
    /// The default value for this option is <c>false</c>.
    /// </para>
    /// </remarks>
    /// <value><c>true</c> if the container should support resolving named instance dictionaries; otherwise, <c>false</c>.</value>
    public bool SupportResolvingNamedInstanceDictionaries { get; private set; }

    /// <summary>
    /// Gets a value indicating whether or not the container will self-register itself as an instance of
    /// <see cref="IResolvesServices"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If this option is enabled then when a container instance is created, as part of its constructor, it will
    /// create and add a registration for itself, as an instance registration, for the component type
    /// <see cref="IResolvesServices"/>.  This is a convenience to allow dynamic resolution without needing to manually
    /// register the container.
    /// </para>
    /// <para>
    /// If this option is disabled then that self-registration will not occur.
    /// </para>
    /// <para>
    /// The default value for this option is <c>true</c>.
    /// </para>
    /// </remarks>
    /// <value><c>true</c> if the container should self-register as a resolver; otherwise, <c>false</c>.</value>
    public bool SelfRegisterAResolver { get; private set; }

    /// <summary>
    /// Gets a value indicating whether or not the container will self-register itself as an instance of
    /// <see cref="IReceivesRegistrations"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If this option is enabled then when a container instance is created, as part of its constructor, it will
    /// create and add a registration for itself, as an instance registration, for the component type
    /// <see cref="IReceivesRegistrations"/>.  This is a convenience to allow dynamic addition of further registrations,
    /// without needing to manually register the container.
    /// </para>
    /// <para>
    /// If this option is disabled then that self-registration will not occur.
    /// </para>
    /// <para>
    /// The default value for this option is <c>false</c>.
    /// </para>
    /// </remarks>
    /// <value><c>true</c> if the container should self-register as a registry; otherwise, <c>false</c>.</value>
    public bool SelfRegisterTheRegistry { get; private set; }

    /// <summary>
    /// Gets a value indicating whether or not the container will support resolving a lazy component instance:
    /// <c>Lazy&lt;TComponent&gt;</c>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If this option is enabled then it will possible to resolve "lazy component" instances which are not fully
    /// resolved until they are first used (via the <c>Lazy&lt;T&gt;.Value</c> property).  Lazy component instances
    /// are resolved just like any other, by listing them as a constructor paramation (AKA constructor-injection)
    /// or by attempting to resolve them directly.
    /// </para>
    /// <para>
    /// If this option is disabled then components may not be resolved lazily in this manner.
    /// </para>
    /// <para>
    /// The default value for this option is <c>true</c>.
    /// </para>
    /// </remarks>
    /// <value><c>true</c> if the container will support resolving lazy instances; otherwise, <c>false</c>.</value>
    public bool SupportResolvingLazyInstances { get; private set; }

    /// <summary>
    /// Gets a value indicating whether or not all resolution (container-wide) shall be considered optional.
    /// </summary>
    /// <remarks>
    /// <para>
    /// By default, unless an overload of <see cref="ContainerResolutionExtensions.TryResolve(IResolvesServices)"/> is used, resolution is mandatory.
    /// That is, either an object instance will be resolved successfully, or <see cref="Resolution.ResolutionException"/>
    /// will be raised.  There are no 'silent failures' to resolve a component.
    /// </para>
    /// <para>
    /// If this option is enabled then every resolution will be treated like <c>TryResolve</c>, and if the resolution
    /// fails, a <c>null</c> reference will be returned as the result of resolution instead.
    /// </para>
    /// <para>
    /// Please exercise caution and think twice before enabling this option, because it can mask deeper errors in your
    /// registrations/application by silently returning a null instance, which will later on be hard to debug.
    /// </para>
    /// <para>
    /// The default value for this option is <c>false</c>.
    /// </para>
    /// </remarks>
    /// <value><c>true</c> if all resolution should be optional; otherwise, <c>false</c>.</value>
    public bool MakeAllResolutionOptional { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ContainerOptions"/> class.
    /// </summary>
    /// <param name="useNonPublicConstructors">Corresponds to the <see cref="UseNonPublicConstructors"/> option.</param>
    /// <param name="resolveUnregisteredTypes">Corresponds to the <see cref="ResolveUnregisteredTypes"/> option.</param>
    /// <param name="useInstanceCache">Corresponds to the <see cref="UseInstanceCache"/> option.</param>
    /// <param name="throwOnCircularDependencies">Corresponds to the <see cref="ThrowOnCircularDependencies"/> option.</param>
    /// <param name="supportResolvingNamedInstanceDictionaries">Corresponds to the <see cref="SupportResolvingNamedInstanceDictionaries"/> option.</param>
    /// <param name="selfRegisterAResolver">Corresponds to the <see cref="SelfRegisterAResolver"/> option.</param>
    /// <param name="selfRegisterTheRegistry">Corresponds to the <see cref="SelfRegisterTheRegistry"/> option.</param>
    /// <param name="supportResolvingLazyInstances">Corresponds to the <see cref="SupportResolvingLazyInstances"/> option.</param>
    /// <param name="makeAllResolutionOptional">Corresponds to the <see cref="MakeAllResolutionOptional"/> option.</param>
    public ContainerOptions(bool useNonPublicConstructors = false,
                            bool resolveUnregisteredTypes = false,
                            bool useInstanceCache = true,
                            bool throwOnCircularDependencies = true,
                            bool supportResolvingNamedInstanceDictionaries = false,
                            bool selfRegisterAResolver = true,
                            bool selfRegisterTheRegistry = false,
                            bool supportResolvingLazyInstances = true,
                            bool makeAllResolutionOptional = false)
    {
      MakeAllResolutionOptional = makeAllResolutionOptional;
      SelfRegisterTheRegistry = selfRegisterTheRegistry;
      SelfRegisterAResolver = selfRegisterAResolver;
      UseNonPublicConstructors = useNonPublicConstructors;
      ResolveUnregisteredTypes = resolveUnregisteredTypes;
      UseInstanceCache = useInstanceCache;
      ThrowOnCircularDependencies = throwOnCircularDependencies;
      SupportResolvingNamedInstanceDictionaries = supportResolvingNamedInstanceDictionaries;
      SupportResolvingLazyInstances = supportResolvingLazyInstances;
    }

    /// <summary>
    /// Initializes the <see cref="ContainerOptions"/> class.
    /// </summary>
    static ContainerOptions()
    {
      defaultOptions = new ContainerOptions();
    }

    /// <summary>
    /// Gets a special singleton instance which represents the hard-coded default options, used when no options are
    /// specified.
    /// </summary>
    /// <value>The default options.</value>
    public static ContainerOptions Default => defaultOptions;
  }
}
