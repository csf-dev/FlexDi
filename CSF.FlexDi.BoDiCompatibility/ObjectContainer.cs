//
//    ObjectContainer.cs
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
using BoDi.Internal;
using CSF.FlexDi;
using CSF.FlexDi.Registration;

namespace BoDi
{
    /// <summary>
    /// Concrete implementation of <see cref="IObjectContainer"/>.  This implementation, rather than providing all of
    /// the functionality itself, simply wraps a FlexDi <see cref="IContainer"/>.
    /// </summary>
    public class ObjectContainer : IObjectContainer, IProvidesFlexDiContainer
    {
        static readonly ExceptionTransformer exceptionTransformer;
        static readonly BoDiFlexDiContainerFactory containerFactory;

        /// <summary>
        /// The wrapped FlexDi container.
        /// </summary>
        protected IContainer container;
        bool isDisposed;

        /// <summary>
        /// Fired when a new object is created directly by the container. It is not invoked for resolving instance and factory registrations.
        /// </summary>
        public event Action<object> ObjectCreated;

        internal IContainer GetFlexDiContainer() => container;

        IContainer IProvidesFlexDiContainer.GetFlexDiContainer() => GetFlexDiContainer();

        /// <summary>
        /// Registers a type as the desired implementation type of an interface.
        /// </summary>
        /// <param name="implementationType">Implementation type.</param>
        /// <param name="name">A name to register named instance, otherwise null.</param>
        /// <exception cref="T:BoDi.ObjectContainerException">If there was already a resolve for the <typeparamref name="TInterface" />.</exception>
        /// <typeparam name="TInterface">Interface will be resolved.</typeparam>
        /// <remarks>
        /// <para>Previous registrations can be overridden before the first resolution for the <typeparamref name="TInterface" />.</para>
        /// </remarks>
        public void RegisterTypeAs<TInterface>(Type implementationType, string name = null) where TInterface : class
        {
            RegisterTypeAs(implementationType, typeof(TInterface), name);
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
            RegisterTypeAs(typeof(TType), typeof(TInterface), name);
        }

        /// <summary>
        /// Registers the type as.
        /// </summary>
        /// <param name="implementationType">Implementation type.</param>
        /// <param name="interfaceType">Interface will be resolved.</param>
        /// <exception cref="T:BoDi.ObjectContainerException">If there was already a resolve for the interface type.</exception>
        /// <remarks>
        /// <para>Previous registrations can be overridden before the first resolution for the interface type.</para>
        /// </remarks>
        public void RegisterTypeAs(Type implementationType, Type interfaceType)
        {
            RegisterTypeAs(implementationType, interfaceType, null);
        }

        void RegisterTypeAs(Type implementationType, Type interfaceType, string name)
        {
            exceptionTransformer.TransformExceptions(() => {
                container.AddRegistrations(x => {
                    x.RegisterType(implementationType)
           .As(interfaceType)
           .WithName(name);
                });
            });
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
            exceptionTransformer.TransformExceptions(() => {
                container.AddRegistrations(x => {
                    x.RegisterInstance(instance)
           .As(interfaceType)
           .WithName(name)
           .DisposeWithContainer(dispose);
                });
            });
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
            RegisterInstanceAs(instance, typeof(TInterface), name, dispose);
        }

        /// <summary>
        /// Registers an instance produced by <paramref name="factoryDelegate" />. The delegate will be called only once and the instance it returned will be returned in each resolution.
        /// </summary>
        /// <param name="factoryDelegate">The function to run to obtain the instance.</param>
        /// <param name="name">A name to resolve named instance, otherwise null.</param>
        /// <typeparam name="TInterface">Interface to register as.</typeparam>
        public void RegisterFactoryAs<TInterface>(Func<TInterface> factoryDelegate, string name = null)
        {
            RegisterFactoryAs(factoryDelegate, typeof(TInterface), name);
        }

        /// <summary>
        /// Registers an instance produced by <paramref name="factoryDelegate" />. The delegate will be called only once and the instance it returned will be returned in each resolution.
        /// </summary>
        /// <typeparam name="TInterface">Interface to register as.</typeparam>
        /// <param name="factoryDelegate">The function to run to obtain the instance.</param>
        /// <param name="name">A name to resolve named instance, otherwise null.</param>
        public void RegisterFactoryAs<TInterface>(Func<IObjectContainer, TInterface> factoryDelegate, string name = null)
        {
            RegisterFactoryAs(factoryDelegate, typeof(TInterface), name);
        }

        /// <summary>
        /// Registers an instance produced by <paramref name="factoryDelegate" />. The delegate will be called only once and the instance it returned will be returned in each resolution.
        /// </summary>
        /// <param name="factoryDelegate">The function to run to obtain the instance.</param>
        /// <param name="name">A name to resolve named instance, otherwise null.</param>
        /// <typeparam name="TInterface">Interface to register as.</typeparam>
        public void RegisterFactoryAs<TInterface>(Delegate factoryDelegate, string name = null)
        {
            RegisterFactoryAs(factoryDelegate, typeof(TInterface), name);
        }

        /// <summary>
        /// Registers an instance produced by <paramref name="factoryDelegate" />. The delegate will be called only once and the instance it returned will be returned in each resolution.
        /// </summary>
        /// <param name="factoryDelegate">The function to run to obtain the instance.</param>
        /// <param name="interfaceType">Interface to register as.</param>
        /// <param name="name">A name to resolve named instance, otherwise null.</param>
        public void RegisterFactoryAs(Delegate factoryDelegate, Type interfaceType, string name = null)
        {
            exceptionTransformer.TransformExceptions(() => {
                container.AddRegistrations(x => {
                    x.RegisterFactory(factoryDelegate, interfaceType)
           .WithName(name);
                });
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
            return IsRegistered<T>(null);
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
            return exceptionTransformer.TransformExceptions(() => {
                return container.HasRegistration<T>(name);
            });
        }

        #if !BODI_LIMITEDRUNTIME && !BODI_DISABLECONFIGFILESUPPORT

        /// <summary>
        /// Adds registrations contained within the application/web configuration file.
        /// </summary>
        public void RegisterFromConfiguration()
        {
            var section = (BoDiConfigurationSection) System.Configuration.ConfigurationManager.GetSection("boDi");
            if (section == null)
                return;

            RegisterFromConfiguration(section.Registrations);
        }

        /// <summary>
        /// Adds registrations contained within the application/web configuration file.
        /// </summary>
        /// <param name="containerRegistrationCollection">Container registration collection.</param>
        /// <remarks>
        /// <para>
        /// All of the elements within the configuration collection are read and interpreted as type registrations.
        /// </para>
        /// </remarks>
        public void RegisterFromConfiguration(ContainerRegistrationCollection containerRegistrationCollection)
        {
            if (containerRegistrationCollection == null)
                return;

            foreach (ContainerRegistrationConfigElement registrationConfigElement in containerRegistrationCollection)
            {
                RegisterFromConfiguration(registrationConfigElement);
            }
        }

        void RegisterFromConfiguration(ContainerRegistrationConfigElement registrationConfigElement)
        {
            var interfaceType = Type.GetType(registrationConfigElement.Interface, true);
            var implementationType = Type.GetType(registrationConfigElement.Implementation, true);
            var name = string.IsNullOrEmpty(registrationConfigElement.Name) ? null : registrationConfigElement.Name;

            RegisterTypeAs(implementationType, interfaceType, name);
        }

        #endif

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
            return Resolve<T>(null);
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
                return container.Resolve<T>(name);
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
                return container.Resolve(typeToResolve, name);
            });
        }

        /// <summary>
        /// Resolved all of the instances of <typeparamref name="T"/> which are available to the current instance.
        /// </summary>
        /// <returns>The resolved instances.</returns>
        /// <typeparam name="T">The type to resolve.</typeparam>
        public IEnumerable<T> ResolveAll<T>() where T : class
        {
            return exceptionTransformer.TransformExceptions(() => {
                return container.ResolveAll<T>();
            });
        }

        IEnumerable<T> IObjectContainer.ResolveAll<T>()
        {
            return exceptionTransformer.TransformExceptions(() => {
                return ResolveAll<T>();
            });
        }

        void OnServiceResolved(object sender, ServiceResolutionEventArgs args)
        {
            if(!(args.Registration is TypeRegistration))
                return;
            
            OnObjectCreated(args.Instance);
        }


        /// <summary>
        /// Invokes the <see cref="ObjectCreated"/> event.
        /// </summary>
        /// <param name="obj">The object which was created by the container.</param>
        protected virtual void OnObjectCreated(object obj)
        {
            var eventHandler = ObjectCreated;
            if (eventHandler != null)
                eventHandler(obj);
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:BoDi.ObjectContainer"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:BoDi.ObjectContainer"/>.</returns>
        public override string ToString()
        {
            return string.Join(Environment.NewLine, RegistrationFormatter.Format(container.GetRegistrations()));
        }

        /// <summary>
        /// Releases all resource used by the <see cref="T:BoDi.ObjectContainer"/> object.
        /// </summary>
        /// <remarks>Call <see cref="Dispose()"/> when you are finished using the <see cref="T:BoDi.ObjectContainer"/>. The
        /// <see cref="Dispose()"/> method leaves the <see cref="T:BoDi.ObjectContainer"/> in an unusable state. After calling
        /// <see cref="Dispose()"/>, you must release all references to the <see cref="T:BoDi.ObjectContainer"/> so the
        /// garbage collector can reclaim the memory that the <see cref="T:BoDi.ObjectContainer"/> was occupying.</remarks>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases all resource used by the <see cref="T:BoDi.ObjectContainer"/> object.
        /// </summary>
        /// <remarks>Call <see cref="Dispose(bool)"/> when you are finished using the <see cref="T:BoDi.ObjectContainer"/>. The
        /// <see cref="Dispose(bool)"/> method leaves the <see cref="T:BoDi.ObjectContainer"/> in an unusable state. After calling
        /// <see cref="Dispose(bool)"/>, you must release all references to the <see cref="T:BoDi.ObjectContainer"/> so the
        /// garbage collector can reclaim the memory that the <see cref="T:BoDi.ObjectContainer"/> was occupying.</remarks>
        /// <param name="disposing">Indicates whether or not explicit disposal is occurring.</param>
        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                if(isDisposed) return;

                container.Dispose();
                isDisposed = true;
            }
        }

        IProvidesFlexDiContainer GetFlexDiContainerProvider(IObjectContainer baseContainer)
        {
            if(baseContainer == null)
                return null;
            
            try
            {
                return (IProvidesFlexDiContainer) baseContainer;
            }
            catch(InvalidCastException ex)
            {
                var message = String.Format(Resources.ExceptionFormats.BaseContainerMustProvideFlexDiContainer,
                                                                        nameof(IProvidesFlexDiContainer));
                throw new ArgumentException(message, nameof(baseContainer), ex);
            }
        }

        IContainer CreateFlexDiContainer(IProvidesFlexDiContainer flexDiProvider)
        {
            if(flexDiProvider != null)
                return new Container(parentContainer: flexDiProvider.GetFlexDiContainer());

            return containerFactory.GetContainer();
        }

        IContainer GetFlexDiContainer(IProvidesFlexDiContainer flexDiProvider)
        {
            var output = CreateFlexDiContainer(flexDiProvider);
            output.ServiceResolved += OnServiceResolved;
            return output;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:BoDi.ObjectContainer"/> class.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The <paramref name="baseContainer"/>, if provided, must be a FlexDi-based BoDi container.  It must not be
        /// a plain/original BoDi container.  This is not supported.
        /// </para>
        /// </remarks>
        /// <param name="baseContainer">An optional BoDi container which shall serve as a parent container.</param>
        public ObjectContainer(IObjectContainer baseContainer = null) 
        {
            var flexDiProvider = GetFlexDiContainerProvider(baseContainer);
            container = GetFlexDiContainer(flexDiProvider);
            RegisterInstanceAs<IObjectContainer>(this);
        }

        /// <summary>
        /// Initializes the <see cref="T:BoDi.ObjectContainer"/> class.
        /// </summary>
        static ObjectContainer()
        {
            exceptionTransformer = new ExceptionTransformer();
            containerFactory = new BoDiFlexDiContainerFactory();
        }
    }
}
