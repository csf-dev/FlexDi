//
//    IObjectContainer.cs
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

namespace BoDi
{
  /// <summary>
  /// The is the BoDi container service.  It provides functionality to register and resolve services/components.
  /// </summary>
  public interface IObjectContainer: IDisposable
  {
    /// <summary>
    /// Fired when a new object is created directly by the container. It is not invoked for resolving instance and factory registrations.
    /// </summary>
    event Action<object> ObjectCreated;

    /// <summary>
    /// Registers a type as the desired implementation type of an interface.
    /// </summary>
    /// <typeparam name="TType">Implementation type</typeparam>
    /// <typeparam name="TInterface">Interface will be resolved</typeparam>
    /// <param name="name">A name to register named instance, otherwise null.</param>
    /// <exception cref="ObjectContainerException">If there was already a resolve for the <typeparamref name="TInterface"/>.</exception>
    /// <remarks>
    ///     <para>Previous registrations can be overridden before the first resolution for the <typeparamref name="TInterface"/>.</para>
    /// </remarks>
    void RegisterTypeAs<TType, TInterface>(string name = null) where TType : class, TInterface;

    /// <summary>
    /// Registers an instance 
    /// </summary>
    /// <typeparam name="TInterface">Interface will be resolved</typeparam>
    /// <param name="instance">The instance implements the interface.</param>
    /// <param name="name">A name to register named instance, otherwise null.</param>
    /// <param name="dispose">Whether the instance should be disposed on container dispose, otherwise <c>false</c>.</param>
    /// <exception cref="ArgumentNullException">If <paramref name="instance"/> is null.</exception>
    /// <exception cref="ObjectContainerException">If there was already a resolve for the <typeparamref name="TInterface"/>.</exception>
    /// <remarks>
    ///     <para>Previous registrations can be overridden before the first resolution for the <typeparamref name="TInterface"/>.</para>
    ///     <para>The instance will be registered in the object pool, so if a <see cref="Resolve{T}()"/> (for another interface) would require an instance of the dynamic type of the <paramref name="instance"/>, the <paramref name="instance"/> will be returned.</para>
    /// </remarks>
    void RegisterInstanceAs<TInterface>(TInterface instance, string name = null, bool dispose = false) where TInterface : class;

    /// <summary>
    /// Registers an instance 
    /// </summary>
    /// <param name="instance">The instance implements the interface.</param>
    /// <param name="interfaceType">Interface will be resolved</param>
    /// <param name="name">A name to register named instance, otherwise null.</param>
    /// <param name="dispose">Whether the instance should be disposed on container dispose, otherwise <c>false</c>.</param>
    /// <exception cref="ArgumentNullException">If <paramref name="instance"/> is null.</exception>
    /// <exception cref="ObjectContainerException">If there was already a resolve for the <paramref name="interfaceType"/>.</exception>
    /// <remarks>
    ///     <para>Previous registrations can be overridden before the first resolution for the <paramref name="interfaceType"/>.</para>
    ///     <para>The instance will be registered in the object pool, so if a <see cref="Resolve{T}()"/> (for another interface) would require an instance of the dynamic type of the <paramref name="instance"/>, the <paramref name="instance"/> will be returned.</para>
    /// </remarks>
    void RegisterInstanceAs(object instance, Type interfaceType, string name = null, bool dispose = false);

    /// <summary>
    /// Registers an instance produced by <paramref name="factoryDelegate"/>. The delegate will be called only once and the instance it returned will be returned in each resolution.
    /// </summary>
    /// <typeparam name="TInterface">Interface to register as.</typeparam>
    /// <param name="factoryDelegate">The function to run to obtain the instance.</param>
    /// <param name="name">A name to resolve named instance, otherwise null.</param>
    void RegisterFactoryAs<TInterface>(Func<IObjectContainer, TInterface> factoryDelegate, string name = null);

    /// <summary>
    /// Resolves an implementation object for an interface or type.
    /// </summary>
    /// <typeparam name="T">The interface or type.</typeparam>
    /// <returns>An object implementing <typeparamref name="T"/>.</returns>
    /// <remarks>
    ///     <para>The container pools the objects, so if the interface is resolved twice or the same type is registered for multiple interfaces, a single instance is created and returned.</para>
    /// </remarks>
    T Resolve<T>();

    /// <summary>
    /// Resolves an implementation object for an interface or type.
    /// </summary>
    /// <param name="name">A name to resolve named instance, otherwise null.</param>
    /// <typeparam name="T">The interface or type.</typeparam>
    /// <returns>An object implementing <typeparamref name="T"/>.</returns>
    /// <remarks>
    ///     <para>The container pools the objects, so if the interface is resolved twice or the same type is registered for multiple interfaces, a single instance is created and returned.</para>
    /// </remarks>
    T Resolve<T>(string name);

    /// <summary>
    /// Resolves an implementation object for an interface or type.
    /// </summary>
    /// <param name="typeToResolve">The interface or type.</param>
    /// <param name="name">A name to resolve named instance, otherwise null.</param>
    /// <returns>An object implementing <paramref name="typeToResolve"/>.</returns>
    /// <remarks>
    ///     <para>The container pools the objects, so if the interface is resolved twice or the same type is registered for multiple interfaces, a single instance is created and returned.</para>
    /// </remarks>
    object Resolve(Type typeToResolve, string name = null);

    /// <summary>
    /// Resolves all implementations of an interface or type.
    /// </summary>
    /// <typeparam name="T">The interface or type.</typeparam>
    /// <returns>An object implementing <typeparamref name="T"/>.</returns>
    IEnumerable<T> ResolveAll<T>() where T : class;

    /// <summary>
    /// Determines whether the interface or type is registered.
    /// </summary>
    /// <typeparam name="T">The interface or type.</typeparam>
    /// <returns><c>true</c> if the interface or type is registered; otherwise <c>false</c>.</returns>
    bool IsRegistered<T>();

    /// <summary>
    /// Determines whether the interface or type is registered with the specified name.
    /// </summary>
    /// <typeparam name="T">The interface or type.</typeparam>
    /// <param name="name">The name.</param>
    /// <returns><c>true</c> if the interface or type is registered; otherwise <c>false</c>.</returns>
    bool IsRegistered<T>(string name);
  }
}
