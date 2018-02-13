//
//    IResolvesServices.cs
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
using CSF.FlexDi.Resolution;

namespace CSF.FlexDi
{
  /// <summary>
  /// A type which can resolve service instances.
  /// </summary>
  public interface IResolvesServices
  {
    /// <summary>
    /// Resolves an instance of the specified type.
    /// </summary>
    /// <typeparam name="T">The component type to be resolved.</typeparam>
    T Resolve<T>();

    /// <summary>
    /// Resolves an instance of the specified type, using the given named registration.
    /// </summary>
    /// <param name="name">The registration name.</param>
    /// <typeparam name="T">The component type to be resolved.</typeparam>
    T Resolve<T>(string name);

    /// <summary>
    /// Resolves an instance of the specified type.
    /// </summary>
    /// <param name="serviceType">The component type to be resolved.</param>
    object Resolve(Type serviceType);

    /// <summary>
    /// Resolves an instance of the specified type, using the given named registration.
    /// </summary>
    /// <param name="serviceType">The component type to be resolved.</param>
    /// <param name="name">The registration name.</param>
    object Resolve(Type serviceType, string name);

    /// <summary>
    /// Attempts to resolve an instance of the specified type, but does not raise an exception if resolution fails.
    /// </summary>
    /// <returns><c>true</c>, if resolution was successful, <c>false</c> otherwise.</returns>
    /// <param name="output">The resolved component instance.</param>
    /// <typeparam name="T">The component type to be resolved.</typeparam>
    bool TryResolve<T>(out T output);

    /// <summary>
    /// Attempts to resolve an instance of the specified type and using the given named registration, but
    /// does not raise an exception if resolution fails.
    /// </summary>
    /// <returns><c>true</c>, if resolution was successful, <c>false</c> otherwise.</returns>
    /// <param name="output">The resolved component instance.</param>
    /// <param name="name">The registration name.</param>
    /// <typeparam name="T">The component type to be resolved.</typeparam>
    bool TryResolve<T>(string name, out T output);

    /// <summary>
    /// Attempts to resolve an instance of the specified type, but does not raise an exception if resolution fails.
    /// </summary>
    /// <returns><c>true</c>, if resolution was successful, <c>false</c> otherwise.</returns>
    /// <param name="output">The resolved component instance.</param>
    /// <param name="serviceType">The component type to be resolved.</param>
    bool TryResolve(Type serviceType, out object output);

    /// <summary>
    /// Attempts to resolve an instance of the specified type and using the given named registration, but
    /// does not raise an exception if resolution fails.
    /// </summary>
    /// <returns><c>true</c>, if resolution was successful, <c>false</c> otherwise.</returns>
    /// <param name="output">The resolved component instance.</param>
    /// <param name="name">The registration name.</param>
    /// <param name="serviceType">The component type to be resolved.</param>
    bool TryResolve(Type serviceType, string name, out object output);

    /// <summary>
    /// Attempts to resolve an instance of the specified type, but returns a <c>null</c> reference if resolution fails.
    /// </summary>
    /// <returns>The resolved component instance, or a <c>null</c> reference if resolution fauls.</returns>
    /// <typeparam name="T">The component type to be resolved.</typeparam>
    T TryResolve<T>() where T : class;

    /// <summary>
    /// Attempts to resolve an instance of the specified type and using the given named registration, but
    /// returns a <c>null</c> reference if resolution fails.
    /// </summary>
    /// <returns>The resolved component instance, or a <c>null</c> reference if resolution fauls.</returns>
    /// <param name="name">The registration name.</param>
    /// <typeparam name="T">The component type to be resolved.</typeparam>
    T TryResolve<T>(string name) where T : class;

    /// <summary>
    /// Attempts to resolve an instance of the specified type, but returns a <c>null</c> reference if resolution fails.
    /// </summary>
    /// <returns>The resolved component instance, or a <c>null</c> reference if resolution fauls.</returns>
    /// <param name="serviceType">The component type to be resolved.</param>
    object TryResolve(Type serviceType);

    /// <summary>
    /// Attempts to resolve an instance of the specified type and using the given named registration, but
    /// returns a <c>null</c> reference if resolution fails.
    /// </summary>
    /// <returns>The resolved component instance, or a <c>null</c> reference if resolution fauls.</returns>
    /// <param name="name">The registration name.</param>
    /// <param name="serviceType">The component type to be resolved.</param>
    object TryResolve(Type serviceType, string name);

    /// <summary>
    /// Attempts to resolve a component, as specified by a <see cref="ResolutionRequest"/> instance.
    /// The result indicates whether resolution was successful or not, and if it is, contains a reference to the resolved
    /// component.
    /// </summary>
    /// <returns>A resolution result instance.</returns>
    /// <param name="request">A resolution request specifying what is to be resolved.</param>
    ResolutionResult TryResolve(ResolutionRequest request);

    /// <summary>
    /// Resolves a component, as specified by a <see cref="ResolutionRequest"/> instance.
    /// </summary>
    /// <param name="request">The resolved component instance.</param>
    object Resolve(ResolutionRequest request);

    /// <summary>
    /// Creates a collection which contains resolved instances of all of the components registered for a given type.
    /// </summary>
    /// <returns>A collection of resolved components.</returns>
    /// <typeparam name="T">The type of the components to be resolved.</typeparam>
    IReadOnlyCollection<T> ResolveAll<T>();
    IReadOnlyCollection<object> ResolveAll(Type serviceType);
  }
}
