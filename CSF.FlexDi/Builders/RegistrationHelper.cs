//
//    RegistrationHelper.cs
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
using CSF.FlexDi.Registration;
using CSF.FlexDi.Resolution;
using System.Reflection;

namespace CSF.FlexDi.Builders
{
  /// <summary>
  /// Implementation of the various registration helper interfaces.  This assists in adding new registrations to
  /// a container or registry instance.
  /// </summary>
  public class RegistrationHelper : IRegistrationHelper, IBulkRegistrationProvider
  {
    readonly ICollection<IServiceRegistration> registrations;
    readonly ISelectsConstructor constructorSelector;

    /// <summary>
    /// Registers a component via a factory function.  The provided delegate is expected to produce an instance of the
    /// specified component type when it is invoked.
    /// </summary>
    /// <returns>A builder instance.</returns>
    /// <param name="factory">A delegate which shall create the component instance.</param>
    /// <param name="serviceType">The type for which to register the service.  This must be assignable from the return value of the delegate.</param>
    public IRegistrationOptionsBuilderWithCacheability RegisterFactory(Delegate factory, Type serviceType)
    {
      if(factory == null)
        throw new ArgumentNullException(nameof(factory));
      if(serviceType == null)
        throw new ArgumentNullException(nameof(serviceType));

      var registration = new FactoryRegistration(factory) { ServiceType = serviceType };
      registrations.Add(registration);
      return new RegistrationBuilder(registration);
    }

    /// <summary>
    /// Registers a component via a factory function.  The provided delegate is expected to produce an instance of the
    /// specified component type when it is invoked.
    /// </summary>
    /// <returns>A builder instance.</returns>
    /// <param name="factory">A delegate which shall create the component instance.</param>
    /// <typeparam name="TService">The type for which to register the service.  This must be assignable from the return value of the delegate.</typeparam>
    public IRegistrationOptionsBuilderWithCacheability RegisterFactory<TService>(Delegate factory)
      where TService : class
      => RegisterFactory(factory, typeof(TService));

    /// <summary>
    /// Registers a component via a factory function.  There are many overloads of this method, taking a variable number
    /// of parameters.  Any parameters indicated will be resolved via dependency resolution.  The return value must
    /// be an instance of the component being registered.
    /// </summary>
    /// <returns>A builder instance.</returns>
    /// <param name="factory">A factory function which will be used to create the component.</param>
    /// <typeparam name="TReg">The implementation type of the component to be registered.</typeparam>
    public IAsBuilderWithCacheability RegisterFactory<TReg>(Func<TReg> factory)
      where TReg : class
      => RegisterTypedFactory<TReg>(factory);

    /// <summary>
    /// Registers a component via a factory function.  There are many overloads of this method, taking a variable number
    /// of parameters.  Any parameters indicated will be resolved via dependency resolution.  The return value must
    /// be an instance of the component being registered.
    /// </summary>
    /// <returns>A builder instance.</returns>
    /// <param name="factory">A factory function which will be used to create the component.</param>
    /// <typeparam name="TReg">The implementation type of the component to be registered.</typeparam>
    /// <typeparam name="T1">The type of a dependency required in the resolution of <typeparamref name="TReg" />.</typeparam>
    public IAsBuilderWithCacheability RegisterFactory<T1,TReg>(Func<T1,TReg> factory)
      where TReg : class
      => RegisterTypedFactory<TReg>(factory);

    /// <summary>
    /// Registers a component via a factory function.  There are many overloads of this method, taking a variable number
    /// of parameters.  Any parameters indicated will be resolved via dependency resolution.  The return value must
    /// be an instance of the component being registered.
    /// </summary>
    /// <returns>A builder instance.</returns>
    /// <param name="factory">A factory function which will be used to create the component.</param>
    /// <typeparam name="TReg">The implementation type of the component to be registered.</typeparam>
    /// <typeparam name="T1">The type of a dependency required in the resolution of <typeparamref name="TReg" />.</typeparam>
    /// <typeparam name="T2">The type of a dependency required in the resolution of <typeparamref name="TReg" />.</typeparam>
    public IAsBuilderWithCacheability RegisterFactory<T1,T2,TReg>(Func<T1,T2,TReg> factory)
      where TReg : class
      => RegisterTypedFactory<TReg>(factory);

    /// <summary>
    /// Registers a component via a factory function.  There are many overloads of this method, taking a variable number
    /// of parameters.  Any parameters indicated will be resolved via dependency resolution.  The return value must
    /// be an instance of the component being registered.
    /// </summary>
    /// <returns>A builder instance.</returns>
    /// <param name="factory">A factory function which will be used to create the component.</param>
    /// <typeparam name="TReg">The implementation type of the component to be registered.</typeparam>
    /// <typeparam name="T1">The type of a dependency required in the resolution of <typeparamref name="TReg" />.</typeparam>
    /// <typeparam name="T2">The type of a dependency required in the resolution of <typeparamref name="TReg" />.</typeparam>
    /// <typeparam name="T3">The type of a dependency required in the resolution of <typeparamref name="TReg" />.</typeparam>
    public IAsBuilderWithCacheability RegisterFactory<T1,T2,T3,TReg>(Func<T1,T2,T3,TReg> factory)
      where TReg : class
      => RegisterTypedFactory<TReg>(factory);

    /// <summary>
    /// Registers a component via a factory function.  There are many overloads of this method, taking a variable number
    /// of parameters.  Any parameters indicated will be resolved via dependency resolution.  The return value must
    /// be an instance of the component being registered.
    /// </summary>
    /// <returns>A builder instance.</returns>
    /// <param name="factory">A factory function which will be used to create the component.</param>
    /// <typeparam name="TReg">The implementation type of the component to be registered.</typeparam>
    /// <typeparam name="T1">The type of a dependency required in the resolution of <typeparamref name="TReg" />.</typeparam>
    /// <typeparam name="T2">The type of a dependency required in the resolution of <typeparamref name="TReg" />.</typeparam>
    /// <typeparam name="T3">The type of a dependency required in the resolution of <typeparamref name="TReg" />.</typeparam>
    /// <typeparam name="T4">The type of a dependency required in the resolution of <typeparamref name="TReg" />.</typeparam>
    public IAsBuilderWithCacheability RegisterFactory<T1,T2,T3,T4,TReg>(Func<T1,T2,T3,T4,TReg> factory)
      where TReg : class
      => RegisterTypedFactory<TReg>(factory);

    /// <summary>
    /// Registers a component via a factory function.  There are many overloads of this method, taking a variable number
    /// of parameters.  Any parameters indicated will be resolved via dependency resolution.  The return value must
    /// be an instance of the component being registered.
    /// </summary>
    /// <returns>A builder instance.</returns>
    /// <param name="factory">A factory function which will be used to create the component.</param>
    /// <typeparam name="TReg">The implementation type of the component to be registered.</typeparam>
    /// <typeparam name="T1">The type of a dependency required in the resolution of <typeparamref name="TReg" />.</typeparam>
    /// <typeparam name="T2">The type of a dependency required in the resolution of <typeparamref name="TReg" />.</typeparam>
    /// <typeparam name="T3">The type of a dependency required in the resolution of <typeparamref name="TReg" />.</typeparam>
    /// <typeparam name="T4">The type of a dependency required in the resolution of <typeparamref name="TReg" />.</typeparam>
    /// <typeparam name="T5">The type of a dependency required in the resolution of <typeparamref name="TReg" />.</typeparam>
    public IAsBuilderWithCacheability RegisterFactory<T1,T2,T3,T4,T5,TReg>(Func<T1,T2,T3,T4,T5,TReg> factory)
      where TReg : class
      => RegisterTypedFactory<TReg>(factory);

    /// <summary>
    /// Registers a component via a factory function.  There are many overloads of this method, taking a variable number
    /// of parameters.  Any parameters indicated will be resolved via dependency resolution.  The return value must
    /// be an instance of the component being registered.
    /// </summary>
    /// <returns>A builder instance.</returns>
    /// <param name="factory">A factory function which will be used to create the component.</param>
    /// <typeparam name="TReg">The implementation type of the component to be registered.</typeparam>
    /// <typeparam name="T1">The type of a dependency required in the resolution of <typeparamref name="TReg" />.</typeparam>
    /// <typeparam name="T2">The type of a dependency required in the resolution of <typeparamref name="TReg" />.</typeparam>
    /// <typeparam name="T3">The type of a dependency required in the resolution of <typeparamref name="TReg" />.</typeparam>
    /// <typeparam name="T4">The type of a dependency required in the resolution of <typeparamref name="TReg" />.</typeparam>
    /// <typeparam name="T5">The type of a dependency required in the resolution of <typeparamref name="TReg" />.</typeparam>
    /// <typeparam name="T6">The type of a dependency required in the resolution of <typeparamref name="TReg" />.</typeparam>
    public IAsBuilderWithCacheability RegisterFactory<T1,T2,T3,T4,T5,T6,TReg>(Func<T1,T2,T3,T4,T5,T6,TReg> factory)
      where TReg : class
      => RegisterTypedFactory<TReg>(factory);

    /// <summary>
    /// Registers a component via a factory function.  There are many overloads of this method, taking a variable number
    /// of parameters.  Any parameters indicated will be resolved via dependency resolution.  The return value must
    /// be an instance of the component being registered.
    /// </summary>
    /// <returns>A builder instance.</returns>
    /// <param name="factory">A factory function which will be used to create the component.</param>
    /// <typeparam name="TReg">The implementation type of the component to be registered.</typeparam>
    /// <typeparam name="T1">The type of a dependency required in the resolution of <typeparamref name="TReg" />.</typeparam>
    /// <typeparam name="T2">The type of a dependency required in the resolution of <typeparamref name="TReg" />.</typeparam>
    /// <typeparam name="T3">The type of a dependency required in the resolution of <typeparamref name="TReg" />.</typeparam>
    /// <typeparam name="T4">The type of a dependency required in the resolution of <typeparamref name="TReg" />.</typeparam>
    /// <typeparam name="T5">The type of a dependency required in the resolution of <typeparamref name="TReg" />.</typeparam>
    /// <typeparam name="T6">The type of a dependency required in the resolution of <typeparamref name="TReg" />.</typeparam>
    /// <typeparam name="T7">The type of a dependency required in the resolution of <typeparamref name="TReg" />.</typeparam>
    public IAsBuilderWithCacheability RegisterFactory<T1,T2,T3,T4,T5,T6,T7,TReg>(Func<T1,T2,T3,T4,T5,T6,T7,TReg> factory)
      where TReg : class
      => RegisterTypedFactory<TReg>(factory);

    /// <summary>
    /// Registers a component via a factory function.  There are many overloads of this method, taking a variable number
    /// of parameters.  Any parameters indicated will be resolved via dependency resolution.  The return value must
    /// be an instance of the component being registered.
    /// </summary>
    /// <returns>A builder instance.</returns>
    /// <param name="factory">A factory function which will be used to create the component.</param>
    /// <typeparam name="TReg">The implementation type of the component to be registered.</typeparam>
    /// <typeparam name="T1">The type of a dependency required in the resolution of <typeparamref name="TReg" />.</typeparam>
    /// <typeparam name="T2">The type of a dependency required in the resolution of <typeparamref name="TReg" />.</typeparam>
    /// <typeparam name="T3">The type of a dependency required in the resolution of <typeparamref name="TReg" />.</typeparam>
    /// <typeparam name="T4">The type of a dependency required in the resolution of <typeparamref name="TReg" />.</typeparam>
    /// <typeparam name="T5">The type of a dependency required in the resolution of <typeparamref name="TReg" />.</typeparam>
    /// <typeparam name="T6">The type of a dependency required in the resolution of <typeparamref name="TReg" />.</typeparam>
    /// <typeparam name="T7">The type of a dependency required in the resolution of <typeparamref name="TReg" />.</typeparam>
    /// <typeparam name="T8">The type of a dependency required in the resolution of <typeparamref name="TReg" />.</typeparam>
    public IAsBuilderWithCacheability RegisterFactory<T1,T2,T3,T4,T5,T6,T7,T8,TReg>(Func<T1,T2,T3,T4,T5,T6,T7,T8,TReg> factory)
      where TReg : class
      => RegisterTypedFactory<TReg>(factory);

    IAsBuilderWithCacheability RegisterTypedFactory<TService>(Delegate factory) where TService : class
    {
      if(factory == null)
        throw new ArgumentNullException(nameof(factory));

      var registration = new FactoryRegistration<TService>(factory) { ServiceType = typeof(TService) };
      registrations.Add(registration);
      return new RegistrationBuilder(registration);
    }

    /// <summary>
    /// Registers a component via a factory function.  The parameter to this factory function is an instance of
    /// <see cref="T:CSF.FlexDi.IResolvesServices" /> which may be used to dynamically resolve any other dependencies for the
    /// creation of the component.
    /// </summary>
    /// <returns>A builder instance.</returns>
    /// <param name="factory">A factory function which will be used to create the component.</param>
    /// <typeparam name="T">The implementation type of the component to be registered.</typeparam>
    public IAsBuilderWithCacheability RegisterDynamicFactory<T>(Func<IResolvesServices,T> factory)
      where T : class
      => RegisterTypedFactory<T>(factory);

    /// <summary>
    /// Registers a component instance.  In this case the container will not create the instance dynamically, it will
    /// simply use the instance provided.
    /// </summary>
    /// <returns>A builder instance.</returns>
    /// <param name="instance">The created component instance.</param>
    /// <typeparam name="T">The implementation type of the component to be registered, typically inferred from the provided instance.</typeparam>
    public IAsBuilder RegisterInstance<T>(T instance) where T : class
    {
      if(instance == null)
        throw new ArgumentNullException(nameof(instance));

      var registration = new InstanceRegistration(instance) { ServiceType = typeof(T) };
      registrations.Add(registration);
      return new RegistrationBuilder(registration);
    }

    /// <summary>
    /// Registers a component by its type.  The type used here is the type which will be instantiated when the component
    /// is resolved: The implementation of your component.
    /// </summary>
    /// <returns>A builder instance.</returns>
    /// <param name="concreteType">The implementation type of the component to be registered.</param>
    public IAsBuilderWithCacheability RegisterType(Type concreteType)
    {
      if(concreteType == null)
        throw new ArgumentNullException(nameof(concreteType));

      ServiceRegistration registration;

      if(concreteType.GetTypeInfo().IsGenericTypeDefinition)
      {
        registration = new OpenGenericTypeRegistration(concreteType, constructorSelector)
        { ServiceType = concreteType };
      }
      else
      {
        registration = new TypeRegistration(concreteType, constructorSelector)
        { ServiceType = concreteType };
      }

      registrations.Add(registration);
      return new RegistrationBuilder(registration);
    }

    /// <summary>
    /// Registers a component by its type.  The type used here is the type which will be instantiated when the component
    /// is resolved: The implementation of your component.
    /// </summary>
    /// <returns>A builder instance.</returns>
    /// <typeparam name="T">The implementation type of the component to be registered.</typeparam>
    public IAsBuilderWithCacheability RegisterType<T>()
      where T : class
      => RegisterType(typeof(T));

    /// <summary>
    /// Gets all of the registrations available to the current provider instance.
    /// </summary>
    /// <returns>The registrations.</returns>
    public IReadOnlyCollection<IServiceRegistration> GetRegistrations() => registrations.ToArray();

    /// <summary>
    /// Initializes a new instance of the <see cref="RegistrationHelper"/> class.
    /// </summary>
    /// <param name="constructorSelector">Constructor selector.</param>
    public RegistrationHelper(ISelectsConstructor constructorSelector)
    {
      if(constructorSelector == null)
        throw new ArgumentNullException(nameof(constructorSelector));

      this.constructorSelector = constructorSelector;

      registrations = new List<IServiceRegistration>();
    }
  }
}
