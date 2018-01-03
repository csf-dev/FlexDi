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
using CSF.MicroDi.Registration;
using CSF.MicroDi.Resolution;

namespace CSF.MicroDi.Builders
{
  public class RegistrationHelper : IRegistrationHelper, IBulkRegistrationProvider
  {
    readonly ICollection<IServiceRegistration> registrations;
    readonly bool useNonPublicConstructors;

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

    public IRegistrationOptionsBuilderWithCacheability RegisterFactory<TService>(Delegate factory)
      where TService : class
      => RegisterFactory(factory, typeof(TService));

    public IAsBuilderWithCacheability RegisterFactory<T>(Func<T> factory) where T : class
    {
      if(factory == null)
        throw new ArgumentNullException(nameof(factory));

      var registration = new FactoryRegistration<T>(factory) { ServiceType = typeof(T) };
      registrations.Add(registration);
      return new RegistrationBuilder(registration);
    }

    public IAsBuilder RegisterInstance(object instance)
    {
      if(instance == null)
        throw new ArgumentNullException(nameof(instance));

      var registration = new InstanceRegistration(instance) { ServiceType = instance.GetType() };
      registrations.Add(registration);
      return new RegistrationBuilder(registration);
    }

    public IAsBuilderWithCacheability RegisterType(Type concreteType)
    {
      if(concreteType == null)
        throw new ArgumentNullException(nameof(concreteType));

      ServiceRegistration registration;
      var ctorSelector = GetConstructorSelector();

      if(concreteType.IsGenericTypeDefinition)
      {
        registration = new OpenGenericTypeRegistration(concreteType, ctorSelector)
        { ServiceType = concreteType };
      }
      else
      {
        registration = new TypeRegistration(concreteType, ctorSelector)
        { ServiceType = concreteType };
      }

      registrations.Add(registration);
      return new RegistrationBuilder(registration);
    }

    public IAsBuilderWithCacheability RegisterType<T>()
      where T : class
      => RegisterType(typeof(T));

    public IReadOnlyCollection<IServiceRegistration> GetRegistrations() => registrations.ToArray();

    ISelectsConstructor GetConstructorSelector()
      => new ConstructorWithMostParametersSelector(useNonPublicConstructors);

    public RegistrationHelper(bool useNonPublicConstructors = false)
    {
      registrations = new List<IServiceRegistration>();

      this.useNonPublicConstructors = useNonPublicConstructors;
    }
  }
}
