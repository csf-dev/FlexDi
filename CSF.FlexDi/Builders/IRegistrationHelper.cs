//
//    IRegistrationHelper.cs
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
    /// A service which assists in the creation of component/service registrations.  This fluent/builder interface
    /// guides you through the requirements of creating registrations.
    /// </summary>
    public interface IRegistrationHelper
    {
        /// <summary>
        /// Registers a component by its type.  The type used here is the type which will be instantiated when the component
        /// is resolved: The implementation of your component.
        /// </summary>
        /// <returns>A builder instance.</returns>
        /// <typeparam name="T">The implementation type of the component to be registered.</typeparam>
        IAsBuilderWithCacheability RegisterType<T>() where T : class;

        /// <summary>
        /// Registers a component by its type.  The type used here is the type which will be instantiated when the component
        /// is resolved: The implementation of your component.
        /// </summary>
        /// <returns>A builder instance.</returns>
        /// <param name="concreteType">The implementation type of the component to be registered.</param>
        IAsBuilderWithCacheability RegisterType(Type concreteType);

        /// <summary>
        /// Registers a component via a factory function.  There are many overloads of this method, taking a variable number
        /// of parameters.  Any parameters indicated will be resolved via dependency resolution.  The return value must
        /// be an instance of the component being registered.
        /// </summary>
        /// <returns>A builder instance.</returns>
        /// <param name="factory">A factory function which will be used to create the component.</param>
        /// <typeparam name="TReg">The implementation type of the component to be registered.</typeparam>
        IAsBuilderWithCacheability RegisterFactory<TReg>(Func<TReg> factory) where TReg : class;

        /// <summary>
        /// Registers a component via a factory function.  There are many overloads of this method, taking a variable number
        /// of parameters.  Any parameters indicated will be resolved via dependency resolution.  The return value must
        /// be an instance of the component being registered.
        /// </summary>
        /// <returns>A builder instance.</returns>
        /// <param name="factory">A factory function which will be used to create the component.</param>
        /// <typeparam name="TReg">The implementation type of the component to be registered.</typeparam>
        /// <typeparam name="T1">The type of a dependency required in the resolution of <typeparamref name="TReg"/>.</typeparam>
        IAsBuilderWithCacheability RegisterFactory<T1,TReg>(Func<T1,TReg> factory) where TReg : class;

        /// <summary>
        /// Registers a component via a factory function.  There are many overloads of this method, taking a variable number
        /// of parameters.  Any parameters indicated will be resolved via dependency resolution.  The return value must
        /// be an instance of the component being registered.
        /// </summary>
        /// <returns>A builder instance.</returns>
        /// <param name="factory">A factory function which will be used to create the component.</param>
        /// <typeparam name="TReg">The implementation type of the component to be registered.</typeparam>
        /// <typeparam name="T1">The type of a dependency required in the resolution of <typeparamref name="TReg"/>.</typeparam>
        /// <typeparam name="T2">The type of a dependency required in the resolution of <typeparamref name="TReg"/>.</typeparam>
        IAsBuilderWithCacheability RegisterFactory<T1,T2,TReg>(Func<T1,T2,TReg> factory) where TReg : class;

        /// <summary>
        /// Registers a component via a factory function.  There are many overloads of this method, taking a variable number
        /// of parameters.  Any parameters indicated will be resolved via dependency resolution.  The return value must
        /// be an instance of the component being registered.
        /// </summary>
        /// <returns>A builder instance.</returns>
        /// <param name="factory">A factory function which will be used to create the component.</param>
        /// <typeparam name="TReg">The implementation type of the component to be registered.</typeparam>
        /// <typeparam name="T1">The type of a dependency required in the resolution of <typeparamref name="TReg"/>.</typeparam>
        /// <typeparam name="T2">The type of a dependency required in the resolution of <typeparamref name="TReg"/>.</typeparam>
        /// <typeparam name="T3">The type of a dependency required in the resolution of <typeparamref name="TReg"/>.</typeparam>
        IAsBuilderWithCacheability RegisterFactory<T1,T2,T3,TReg>(Func<T1,T2,T3,TReg> factory) where TReg : class;

        /// <summary>
        /// Registers a component via a factory function.  There are many overloads of this method, taking a variable number
        /// of parameters.  Any parameters indicated will be resolved via dependency resolution.  The return value must
        /// be an instance of the component being registered.
        /// </summary>
        /// <returns>A builder instance.</returns>
        /// <param name="factory">A factory function which will be used to create the component.</param>
        /// <typeparam name="TReg">The implementation type of the component to be registered.</typeparam>
        /// <typeparam name="T1">The type of a dependency required in the resolution of <typeparamref name="TReg"/>.</typeparam>
        /// <typeparam name="T2">The type of a dependency required in the resolution of <typeparamref name="TReg"/>.</typeparam>
        /// <typeparam name="T3">The type of a dependency required in the resolution of <typeparamref name="TReg"/>.</typeparam>
        /// <typeparam name="T4">The type of a dependency required in the resolution of <typeparamref name="TReg"/>.</typeparam>
        IAsBuilderWithCacheability RegisterFactory<T1,T2,T3,T4,TReg>(Func<T1,T2,T3,T4,TReg> factory) where TReg : class;

        /// <summary>
        /// Registers a component via a factory function.  There are many overloads of this method, taking a variable number
        /// of parameters.  Any parameters indicated will be resolved via dependency resolution.  The return value must
        /// be an instance of the component being registered.
        /// </summary>
        /// <returns>A builder instance.</returns>
        /// <param name="factory">A factory function which will be used to create the component.</param>
        /// <typeparam name="TReg">The implementation type of the component to be registered.</typeparam>
        /// <typeparam name="T1">The type of a dependency required in the resolution of <typeparamref name="TReg"/>.</typeparam>
        /// <typeparam name="T2">The type of a dependency required in the resolution of <typeparamref name="TReg"/>.</typeparam>
        /// <typeparam name="T3">The type of a dependency required in the resolution of <typeparamref name="TReg"/>.</typeparam>
        /// <typeparam name="T4">The type of a dependency required in the resolution of <typeparamref name="TReg"/>.</typeparam>
        /// <typeparam name="T5">The type of a dependency required in the resolution of <typeparamref name="TReg"/>.</typeparam>
        IAsBuilderWithCacheability RegisterFactory<T1,T2,T3,T4,T5,TReg>(Func<T1,T2,T3,T4,T5,TReg> factory) where TReg : class;

        /// <summary>
        /// Registers a component via a factory function.  There are many overloads of this method, taking a variable number
        /// of parameters.  Any parameters indicated will be resolved via dependency resolution.  The return value must
        /// be an instance of the component being registered.
        /// </summary>
        /// <returns>A builder instance.</returns>
        /// <param name="factory">A factory function which will be used to create the component.</param>
        /// <typeparam name="TReg">The implementation type of the component to be registered.</typeparam>
        /// <typeparam name="T1">The type of a dependency required in the resolution of <typeparamref name="TReg"/>.</typeparam>
        /// <typeparam name="T2">The type of a dependency required in the resolution of <typeparamref name="TReg"/>.</typeparam>
        /// <typeparam name="T3">The type of a dependency required in the resolution of <typeparamref name="TReg"/>.</typeparam>
        /// <typeparam name="T4">The type of a dependency required in the resolution of <typeparamref name="TReg"/>.</typeparam>
        /// <typeparam name="T5">The type of a dependency required in the resolution of <typeparamref name="TReg"/>.</typeparam>
        /// <typeparam name="T6">The type of a dependency required in the resolution of <typeparamref name="TReg"/>.</typeparam>
        IAsBuilderWithCacheability RegisterFactory<T1,T2,T3,T4,T5,T6,TReg>(Func<T1,T2,T3,T4,T5,T6,TReg> factory) where TReg : class;

        /// <summary>
        /// Registers a component via a factory function.  There are many overloads of this method, taking a variable number
        /// of parameters.  Any parameters indicated will be resolved via dependency resolution.  The return value must
        /// be an instance of the component being registered.
        /// </summary>
        /// <returns>A builder instance.</returns>
        /// <param name="factory">A factory function which will be used to create the component.</param>
        /// <typeparam name="TReg">The implementation type of the component to be registered.</typeparam>
        /// <typeparam name="T1">The type of a dependency required in the resolution of <typeparamref name="TReg"/>.</typeparam>
        /// <typeparam name="T2">The type of a dependency required in the resolution of <typeparamref name="TReg"/>.</typeparam>
        /// <typeparam name="T3">The type of a dependency required in the resolution of <typeparamref name="TReg"/>.</typeparam>
        /// <typeparam name="T4">The type of a dependency required in the resolution of <typeparamref name="TReg"/>.</typeparam>
        /// <typeparam name="T5">The type of a dependency required in the resolution of <typeparamref name="TReg"/>.</typeparam>
        /// <typeparam name="T6">The type of a dependency required in the resolution of <typeparamref name="TReg"/>.</typeparam>
        /// <typeparam name="T7">The type of a dependency required in the resolution of <typeparamref name="TReg"/>.</typeparam>
        IAsBuilderWithCacheability RegisterFactory<T1,T2,T3,T4,T5,T6,T7,TReg>(Func<T1,T2,T3,T4,T5,T6,T7,TReg> factory) where TReg : class;

        /// <summary>
        /// Registers a component via a factory function.  There are many overloads of this method, taking a variable number
        /// of parameters.  Any parameters indicated will be resolved via dependency resolution.  The return value must
        /// be an instance of the component being registered.
        /// </summary>
        /// <returns>A builder instance.</returns>
        /// <param name="factory">A factory function which will be used to create the component.</param>
        /// <typeparam name="TReg">The implementation type of the component to be registered.</typeparam>
        /// <typeparam name="T1">The type of a dependency required in the resolution of <typeparamref name="TReg"/>.</typeparam>
        /// <typeparam name="T2">The type of a dependency required in the resolution of <typeparamref name="TReg"/>.</typeparam>
        /// <typeparam name="T3">The type of a dependency required in the resolution of <typeparamref name="TReg"/>.</typeparam>
        /// <typeparam name="T4">The type of a dependency required in the resolution of <typeparamref name="TReg"/>.</typeparam>
        /// <typeparam name="T5">The type of a dependency required in the resolution of <typeparamref name="TReg"/>.</typeparam>
        /// <typeparam name="T6">The type of a dependency required in the resolution of <typeparamref name="TReg"/>.</typeparam>
        /// <typeparam name="T7">The type of a dependency required in the resolution of <typeparamref name="TReg"/>.</typeparam>
        /// <typeparam name="T8">The type of a dependency required in the resolution of <typeparamref name="TReg"/>.</typeparam>
        IAsBuilderWithCacheability RegisterFactory<T1,T2,T3,T4,T5,T6,T7,T8,TReg>(Func<T1,T2,T3,T4,T5,T6,T7,T8,TReg> factory) where TReg : class;

        /// <summary>
        /// Registers a component via a factory function.  The provided delegate is expected to produce an instance of the
        /// specified component type when it is invoked.
        /// </summary>
        /// <returns>A builder instance.</returns>
        /// <param name="factory">A delegate which shall create the component instance.</param>
        /// <param name="serviceType">The type for which to register the service.  This must be assignable from the return value of the delegate.</param>
        IRegistrationOptionsBuilderWithCacheability RegisterFactory(Delegate factory, Type serviceType);

        /// <summary>
        /// Registers a component via a factory function.  The provided delegate is expected to produce an instance of the
        /// specified component type when it is invoked.
        /// </summary>
        /// <returns>A builder instance.</returns>
        /// <param name="factory">A delegate which shall create the component instance.</param>
        /// <typeparam name="TService">The type for which to register the service.  This must be assignable from the return value of the delegate.</typeparam>
        IRegistrationOptionsBuilderWithCacheability RegisterFactory<TService>(Delegate factory) where TService : class;

        /// <summary>
        /// Registers a component via a factory function.  The parameter to this factory function is an instance of
        /// <see cref="IResolvesServices"/> which may be used to dynamically resolve any other dependencies for the
        /// creation of the component.
        /// </summary>
        /// <returns>A builder instance.</returns>
        /// <param name="factory">A factory function which will be used to create the component.</param>
        /// <typeparam name="T">The implementation type of the component to be registered.</typeparam>
        IAsBuilderWithCacheability RegisterDynamicFactory<T>(Func<IResolvesServices,T> factory) where T : class;

        /// <summary>
        /// Registers a component instance.  In this case the container will not create the instance dynamically, it will
        /// simply use the instance provided.
        /// </summary>
        /// <returns>A builder instance.</returns>
        /// <param name="instance">The created component instance.</param>
        /// <typeparam name="T">The implementation type of the component to be registered, typically inferred from the provided instance.</typeparam>
        IAsBuilder RegisterInstance<T>(T instance) where T : class;
    }
}
