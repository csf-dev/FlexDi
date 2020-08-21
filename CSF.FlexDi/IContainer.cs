//
//    IContainer.cs
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
using CSF.FlexDi.Registration;

namespace CSF.FlexDi
{
  /// <summary>
  /// An interface for the FlexDi container itself.  Where possible, avoid using this in your own code.
  /// </summary>
  /// <remarks>
  /// <para>
  /// In general, it is bad practice to reference this 'large' interface in your own code.  Obviously the ideal
  /// strategy is to use dependency injection directly. If you must register a container interface (to dynamically
  /// resolve components or add registerations) then prefer one of either <see cref="IResolvesServices"/> or
  /// <see cref="IReceivesRegistrations"/>.
  /// </para>
  /// </remarks>
  public interface IContainer : IResolvesServices, IReceivesRegistrations, IDisposable, IInspectsServiceCache
    {
    /// <summary>
    /// Gets a value indicating whether or not the container has a registration for the specified component type.
    /// </summary>
    /// <returns><c>true</c>, if the container has a registration for the component type, <c>false</c> otherwise.</returns>
    /// <param name="name">An optional registration name.</param>
    /// <typeparam name="T">The component type for which to check.</typeparam>
    bool HasRegistration<T>(string name = null);

    /// <summary>
    /// Gets a value indicating whether or not the container has a registration for the specified component type.
    /// </summary>
    /// <returns><c>true</c>, if the container has a registration for the component type, <c>false</c> otherwise.</returns>
    /// <param name="name">An optional registration name.</param>
    /// <param name="serviceType">The component type for which to check.</param>
    bool HasRegistration(Type serviceType, string name = null);

    /// <summary>
    /// Gets a collection of all of the registrations held within the current container instance.
    /// </summary>
    /// <returns>The registrations.</returns>
    IReadOnlyCollection<IServiceRegistration> GetRegistrations();

    /// <summary>
    /// Gets a collection of all of the registrations, matching a specified component type, held within
    /// the current container instance.
    /// </summary>
    /// <returns>The registrations.</returns>
    /// <param name="serviceType">The component type for which to get the registrations.</param>
    IReadOnlyCollection<IServiceRegistration> GetRegistrations(Type serviceType);

    /// <summary>
    /// Occurs when any component is resolved.
    /// </summary>
    event EventHandler<ServiceResolutionEventArgs> ServiceResolved;

    /// <summary>
    /// Creates a new <see cref="IContainer"/> which is a child of the current container.
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
    IContainer CreateChildContainer();
  }
}
