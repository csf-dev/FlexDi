//
//    RegistrationHelperTests.cs
//
//    Copyright 2018  Craig Fowler
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
using NUnit.Framework;
using CSF.MicroDi.Tests.Autofixture;
using CSF.MicroDi.Builders;
using CSF.MicroDi.Tests.Stubs;
using System.Linq;
using CSF.MicroDi.Registration;
using CSF.MicroDi.Resolution;
using Ploeh.AutoFixture.NUnit3;
using Moq;

namespace CSF.MicroDi.Tests.Builders
{
  [TestFixture,Parallelizable(ParallelScope.All)]
  public class RegistrationHelperTests
  {
    [Test,AutoMoqData]
    public void RegisterFactory_can_create_a_generic_factory_registration([DefaultRegHelper] RegistrationHelper sut)
    {
      // Act
      sut.RegisterFactory(() => new SampleServiceImplementationOne());

      // Assert
      var registrations = sut.GetRegistrations();
      Assert.That(registrations, Has.Some.InstanceOf<FactoryRegistration<SampleServiceImplementationOne>>());
      var matchingReg = registrations.FirstOrDefault(x => x is FactoryRegistration<SampleServiceImplementationOne>);
      Assert.That(matchingReg.ServiceType, Is.EqualTo(typeof(SampleServiceImplementationOne)));
    }

    [Test,AutoMoqData]
    public void RegisterFactory_can_create_a_nongeneric_factory_registration([DefaultRegHelper] RegistrationHelper sut)
    {
      // Act
      sut.RegisterFactory(new Func<SampleServiceImplementationOne>(() => new SampleServiceImplementationOne()),
                          typeof(ISampleService));

      // Assert
      var registrations = sut.GetRegistrations();
      Assert.That(registrations, Has.Some.InstanceOf<FactoryRegistration>());
      var matchingReg = registrations.FirstOrDefault(x => x is FactoryRegistration);
      Assert.That(matchingReg.ServiceType, Is.EqualTo(typeof(ISampleService)));
    }

    [Test,AutoMoqData]
    public void RegisterType_generic_can_create_a_type_registration([DefaultRegHelper] RegistrationHelper sut)
    {
      // Act
      sut.RegisterType<SampleServiceImplementationOne>();

      // Assert
      var registrations = sut.GetRegistrations();
      Assert.That(registrations, Has.Some.InstanceOf<TypeRegistration>());
      var matchingReg = registrations.FirstOrDefault(x => x is TypeRegistration);
      Assert.That(matchingReg.ServiceType, Is.EqualTo(typeof(SampleServiceImplementationOne)));
    }

    [Test,AutoMoqData]
    public void RegisterType_nongeneric_can_create_a_type_registration([DefaultRegHelper] RegistrationHelper sut)
    {
      // Act
      sut.RegisterType(typeof(SampleServiceImplementationOne));

      // Assert
      var registrations = sut.GetRegistrations();
      Assert.That(registrations, Has.Some.InstanceOf<TypeRegistration>());
      var matchingReg = registrations.FirstOrDefault(x => x is TypeRegistration);
      Assert.That(matchingReg.ServiceType, Is.EqualTo(typeof(SampleServiceImplementationOne)));
    }

    [Test,AutoMoqData]
    public void RegisterType_nongeneric_can_create_an_open_generic_type_registration([DefaultRegHelper] RegistrationHelper sut)
    {
      // Act
      sut.RegisterType(typeof(GenericService<>));

      // Assert
      var registrations = sut.GetRegistrations();
      Assert.That(registrations, Has.Some.InstanceOf<TypeRegistration>());
      var matchingReg = registrations.FirstOrDefault(x => x is TypeRegistration);
      Assert.That(matchingReg.ServiceType, Is.EqualTo(typeof(GenericService<>)));
    }

    [Test,AutoMoqData]
    public void RegisterType_passes_constructor_selector([Frozen] ISelectsConstructor ctorSelector,
                                                         RegistrationHelper sut)
    {
      // Act
      sut.RegisterType(typeof(SampleServiceImplementationOne));

      // Assert
      Mock.Get(ctorSelector)
          .Verify(x => x.SelectConstructor(typeof(SampleServiceImplementationOne)), Times.Once);
    }

    [Test,AutoMoqData]
    public void RegisterInstance_can_create_an_instance_registration([DefaultRegHelper] RegistrationHelper sut,
                                                                     SampleServiceImplementationOne instance)
    {
      // Act
      sut.RegisterInstance(instance);

      // Assert
      var registrations = sut.GetRegistrations();
      Assert.That(registrations, Has.Some.InstanceOf<InstanceRegistration>());
      var matchingReg = registrations.FirstOrDefault(x => x is InstanceRegistration);
      Assert.That(matchingReg.ServiceType, Is.EqualTo(typeof(SampleServiceImplementationOne)));
    }

    [Test,AutoMoqData]
    [Description("The RegisterInstance method should use the object's actual type to perform the registration")]
    public void RegisterInstance_uses_instance_actual_type([DefaultRegHelper] RegistrationHelper sut,
                                                           SampleServiceImplementationOne instance)
    {
      // Act
      ISampleService regInstance = instance;
      sut.RegisterInstance(regInstance);

      // Assert
      var registrations = sut.GetRegistrations();
      Assert.That(registrations, Has.Some.InstanceOf<InstanceRegistration>());
      var matchingReg = registrations.FirstOrDefault(x => x is InstanceRegistration);
      Assert.That(matchingReg.ServiceType, Is.EqualTo(typeof(SampleServiceImplementationOne)));
    }

    [Test,AutoMoqData]
    public void RegisterInstance_raises_exception_if_instance_is_null([DefaultRegHelper] RegistrationHelper sut)
    {
      // Act & assert
      Assert.That(() => sut.RegisterInstance(null), Throws.InstanceOf<ArgumentNullException>());
    }
  }
}
