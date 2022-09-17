//
//    DynamicResolutionObjectContainerProxy.cs
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
using CSF.FlexDi;
using CSF.FlexDi.Resolution;

namespace BoDi.Internal
{
    /// <summary>
    /// A proxy type which wraps an <see cref="IObjectContainer"/> and provides the ability to detect dynamic circular
    /// dependencies by 'passing on' its own resolution path when it is used to resolve services.
    /// </summary>
    public class DynamicResolutionObjectContainerProxy : IObjectContainer, IProvidesFlexDiContainer
    {
        readonly ResolutionPath resolutionPath;
        readonly ObjectContainer proxiedContainer;
        readonly ExceptionTransformer exceptionTransformer;

        IContainer IProvidesFlexDiContainer.GetFlexDiContainer() => proxiedContainer.GetFlexDiContainer();

        #region IObjectContainer implementation

        /// <summary>
        /// Fired when a new object is created directly by the container. It is not invoked for resolving instance and factory registrations.
        /// </summary>
        public event Action<object> ObjectCreated;

        void InvokeObjectCreated(object obj)
        {
            ObjectCreated?.Invoke(obj);
        }

        /// <summary>
        /// Registers a type as the desired implementation type of an interface.
        /// </summary>
        /// <typeparam name="TType">Implementation type</typeparam>
        /// <typeparam name="TInterface">Interface will be resolved</typeparam>
        /// <param name="name">A name to register named instance, otherwise null.</param>
        /// <exception cref="T:BoDi.ObjectContainerException">If there was already a resolve for the <typeparamref name="TInterface" />.</exception>
        /// <remarks>
        /// <para>Previous registrations can be overridden before the first resolution for the <typeparamref name="TInterface" />.</para>
        /// </remarks>
        public void RegisterTypeAs<TType, TInterface>(string name = null) where TType : class, TInterface
        {
            proxiedContainer.RegisterTypeAs<TType, TInterface>(name);
        }

        /// <summary>
        /// Registers an instance
        /// </summary>
        /// <typeparam name="TInterface">Interface will be resolved</typeparam>
        /// <param name="instance">The instance implements the interface.</param>
        /// <param name="name">A name to register named instance, otherwise null.</param>
        /// <param name="dispose">Whether the instance should be disposed on container dispose, otherwise <c>false</c>.</param>
        /// <exception cref="T:System.ArgumentNullException">If <paramref name="instance" /> is null.</exception>
        /// <exception cref="T:BoDi.ObjectContainerException">If there was already a resolve for the <typeparamref name="TInterface" />.</exception>
        /// <remarks>
        /// <para>Previous registrations can be overridden before the first resolution for the <typeparamref name="TInterface" />.</para>
        /// <para>The instance will be registered in the object pool, so if a <see cref="M:BoDi.IObjectContainer.Resolve``1" /> (for another interface) would require an instance of the dynamic type of the <paramref name="instance" />, the <paramref name="instance" /> will be returned.</para>
        /// </remarks>
        public void RegisterInstanceAs<TInterface>(TInterface instance, string name = null, bool dispose = false) where TInterface : class
        {
            proxiedContainer.RegisterInstanceAs<TInterface>(instance, name, dispose);
        }

        /// <summary>
        /// Registers an instance
        /// </summary>
        /// <param name="instance">The instance implements the interface.</param>
        /// <param name="interfaceType">Interface will be resolved</param>
        /// <param name="name">A name to register named instance, otherwise null.</param>
        /// <param name="dispose">Whether the instance should be disposed on container dispose, otherwise <c>false</c>.</param>
        /// <exception cref="T:System.ArgumentNullException">If <paramref name="instance" /> is null.</exception>
        /// <exception cref="T:BoDi.ObjectContainerException">If there was already a resolve for the <paramref name="interfaceType" />.</exception>
        /// <remarks>
        /// <para>Previous registrations can be overridden before the first resolution for the <paramref name="interfaceType" />.</para>
        /// <para>The instance will be registered in the object pool, so if a <see cref="M:BoDi.IObjectContainer.Resolve``1" /> (for another interface) would require an instance of the dynamic type of the <paramref name="instance" />, the <paramref name="instance" /> will be returned.</para>
        /// </remarks>
        public void RegisterInstanceAs(object instance, Type interfaceType, string name = null, bool dispose = false)
        {
            proxiedContainer.RegisterInstanceAs(instance, interfaceType, name, dispose);
        }

        /// <summary>
        /// Registers an instance produced by <paramref name="factoryDelegate" />. The delegate will be called only once and the instance it returned will be returned in each resolution.
        /// </summary>
        /// <typeparam name="TInterface">Interface to register as.</typeparam>
        /// <param name="factoryDelegate">The function to run to obtain the instance.</param>
        /// <param name="name">A name to resolve named instance, otherwise null.</param>
        public void RegisterFactoryAs<TInterface>(Func<IObjectContainer, TInterface> factoryDelegate, string name = null)
        {
            proxiedContainer.RegisterFactoryAs<TInterface>(factoryDelegate, name);
        }

        /// <summary>
        /// Resolves an implementation object for an interface or type.
        /// </summary>
        /// <typeparam name="T">The interface or type.</typeparam>
        /// <returns>An object implementing <typeparamref name="T" />.</returns>
        /// <remarks>
        /// <para>The container pools the objects, so if the interface is resolved twice or the same type is registered for multiple interfaces, a single instance is created and returned.</para>
        /// </remarks>
        public T Resolve<T>()
        {
            return exceptionTransformer.TransformExceptions(() => {
                return (T) proxiedContainer
                    .GetFlexDiContainer()
                    .Resolve(new ResolutionRequest(typeof(T), resolutionPath));
            });
        }

        /// <summary>
        /// Resolves an implementation object for an interface or type.
        /// </summary>
        /// <param name="name">A name to resolve named instance, otherwise null.</param>
        /// <typeparam name="T">The interface or type.</typeparam>
        /// <returns>An object implementing <typeparamref name="T" />.</returns>
        /// <remarks>
        /// <para>The container pools the objects, so if the interface is resolved twice or the same type is registered for multiple interfaces, a single instance is created and returned.</para>
        /// </remarks>
        public T Resolve<T>(string name)
        {
            return exceptionTransformer.TransformExceptions(() => {
                return (T) proxiedContainer
                    .GetFlexDiContainer()
                    .Resolve(new ResolutionRequest(typeof(T), name, resolutionPath));
            });
        }

        /// <summary>
        /// Resolves an implementation object for an interface or type.
        /// </summary>
        /// <param name="typeToResolve">The interface or type.</param>
        /// <param name="name">A name to resolve named instance, otherwise null.</param>
        /// <returns>An object implementing <paramref name="typeToResolve" />.</returns>
        /// <remarks>
        /// <para>The container pools the objects, so if the interface is resolved twice or the same type is registered for multiple interfaces, a single instance is created and returned.</para>
        /// </remarks>
        public object Resolve(Type typeToResolve, string name = null)
        {
            return exceptionTransformer.TransformExceptions(() => {
                return proxiedContainer
                    .GetFlexDiContainer()
                    .Resolve(new ResolutionRequest(typeToResolve, name, resolutionPath));
            });
        }

        /// <summary>
        /// Resolves all implementations of an interface or type.
        /// </summary>
        /// <typeparam name="T">The interface or type.</typeparam>
        /// <returns>An object implementing <typeparamref name="T" />.</returns>
        public IEnumerable<T> ResolveAll<T>() where T : class
        {
            return exceptionTransformer.TransformExceptions(() => {
                return proxiedContainer
                    .GetFlexDiContainer()
                    .GetRegistrations(typeof(T))
                    .Select(x => Resolve(x.ServiceType, x.Name))
                    .Cast<T>()
                    .ToArray();
            });
        }

        /// <summary>
        /// Determines whether the interface or type is registered.
        /// </summary>
        /// <typeparam name="T">The interface or type.</typeparam>
        /// <returns>
        /// <c>true</c> if the interface or type is registered; otherwise <c>false</c>.</returns>
        public bool IsRegistered<T>()
        {
            return proxiedContainer.IsRegistered<T>();
        }

        /// <summary>
        /// Determines whether the interface or type is registered with the specified name.
        /// </summary>
        /// <typeparam name="T">The interface or type.</typeparam>
        /// <param name="name">The name.</param>
        /// <returns>
        /// <c>true</c> if the interface or type is registered; otherwise <c>false</c>.</returns>
        public bool IsRegistered<T>(string name)
        {
            return proxiedContainer.IsRegistered<T>(name);
        }

        /// <summary>
        /// Releases all resource used by the <see cref="T:BoDi.Internal.DynamicResolutionObjectContainerProxy"/> object.
        /// </summary>
        /// <remarks>Call <see cref="Dispose()"/> when you are finished using the
        /// <see cref="T:BoDi.Internal.DynamicResolutionObjectContainerProxy"/>. The <see cref="Dispose()"/> method leaves the
        /// <see cref="T:BoDi.Internal.DynamicResolutionObjectContainerProxy"/> in an unusable state. After calling
        /// <see cref="Dispose()"/>, you must release all references to the
        /// <see cref="T:BoDi.Internal.DynamicResolutionObjectContainerProxy"/> so the garbage collector can reclaim the
        /// memory that the <see cref="T:BoDi.Internal.DynamicResolutionObjectContainerProxy"/> was occupying.</remarks>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Releases all resource used by the <see cref="T:BoDi.Internal.DynamicResolutionObjectContainerProxy"/> object.
        /// </summary>
        /// <remarks>Call <see cref="Dispose(bool)"/> when you are finished using the
        /// <see cref="T:BoDi.Internal.DynamicResolutionObjectContainerProxy"/>. The <see cref="Dispose(bool)"/> method leaves the
        /// <see cref="T:BoDi.Internal.DynamicResolutionObjectContainerProxy"/> in an unusable state. After calling
        /// <see cref="Dispose(bool)"/>, you must release all references to the
        /// <see cref="T:BoDi.Internal.DynamicResolutionObjectContainerProxy"/> so the garbage collector can reclaim the
        /// memory that the <see cref="T:BoDi.Internal.DynamicResolutionObjectContainerProxy"/> was occupying.</remarks>
        /// <param name="disposing">Indicates whether or not explicit disposal is occurring.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                proxiedContainer.Dispose();
                GC.SuppressFinalize(this);
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="T:BoDi.Internal.DynamicResolutionObjectContainerProxy"/> class.
        /// </summary>
        /// <param name="proxiedContainer">The proxied container.</param>
        /// <param name="resolutionPath">The resolution path up to the point at which this instance was resolved.</param>
        public DynamicResolutionObjectContainerProxy(ObjectContainer proxiedContainer, ResolutionPath resolutionPath)
        {
            if(proxiedContainer == null)
                throw new ArgumentNullException(nameof(proxiedContainer));

            this.proxiedContainer = proxiedContainer;
            this.resolutionPath = resolutionPath ?? new ResolutionPath();
            exceptionTransformer = new ExceptionTransformer();

            proxiedContainer.ObjectCreated += InvokeObjectCreated;
        }
    }
}
