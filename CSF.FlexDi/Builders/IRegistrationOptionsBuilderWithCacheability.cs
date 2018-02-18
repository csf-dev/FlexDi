//
//    IRegistrationOptionsBuilderWithMultiplicity.cs
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
namespace CSF.FlexDi.Builders
{
  /// <summary>
  /// A type which assists in the selection of optional aspects of creating a registration.
  /// </summary>
  public interface IRegistrationOptionsBuilderWithCacheability
  {
    /// <summary>
    /// Indicates that the current registration is to be registered under a specified name.
    /// </summary>
    /// <returns>The builder instance.</returns>
    /// <param name="name">The name of the registration.</param>
    IRegistrationOptionsBuilderWithCacheability WithName(string name);

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
    IRegistrationOptionsBuilderWithCacheability NotCacheable();

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
    IRegistrationOptionsBuilderWithCacheability Cacheable(bool cacheable = true);

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
    IRegistrationOptionsBuilderWithCacheability DoNotDisposeWithContainer();

    /// <summary>
    /// Indicates that this component should be disposed along with the container which resolves it.
    /// </summary>
    /// <seealso cref="DoNotDisposeWithContainer"/>
    /// <returns>The builder instance.</returns>
    /// <param name="disposeWithContainer">If set to <c>true</c> then the component will be disposed with the container.</param>
    IRegistrationOptionsBuilderWithCacheability DisposeWithContainer(bool disposeWithContainer = true);
  }
}
