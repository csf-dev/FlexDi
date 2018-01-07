//
//    OpenGenericTypeRegistrationTests.cs
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
using CSF.MicroDi.Registration;
using CSF.MicroDi.Resolution;
using CSF.MicroDi.Tests.Autofixture;
using CSF.MicroDi.Tests.Stubs;
using Moq;
using NUnit.Framework;

namespace CSF.MicroDi.Tests.Registration
{
  [TestFixture,Parallelizable(ParallelScope.All)]
  public class OpenGenericTypeRegistrationTests : ServiceRegistrationTestBase
  {
    [Test,AutoMoqData]
    public void GetFactoryAdapter_uses_constructor_selector(ISelectsConstructor ctorSelector)
    {
      // Arrange
      var genericType = typeof(GenericService<>);
      var closedType = typeof(GenericService<string>);
      var sut = new OpenGenericTypeRegistration(genericType, ctorSelector);
      var ctor = closedType.GetConstructor(Type.EmptyTypes);
      Mock.Get(ctorSelector)
          .Setup(x => x.SelectConstructor(closedType))
          .Returns(ctor);
      var request = new ResolutionRequest(closedType);

      // Act
      var result = sut.GetFactoryAdapter(request);

      // Assert
      Assert.That(result, Is.Not.Null);
      Assert.That(result, Is.InstanceOf<ConstructorFactory>());
      var ctorFactory = (ConstructorFactory) result;
      Assert.That(ctorFactory.Constructor, Is.SameAs(ctor));
    }

    [Test,AutoMoqData]
    public void AssertIsValid_throws_exception_if_implementation_type_does_not_derive_from_service_type(ISelectsConstructor ctorSelector)
    {
      // Arrange
      var sut = new OpenGenericTypeRegistration(typeof(GenericService<>), ctorSelector) {
        ServiceType = typeof(IOtherGenericService<>),
      };

      // Act & assert
      Assert.That(() => sut.AssertIsValid(), Throws.InstanceOf<InvalidTypeRegistrationException>());
    }

    [Test,AutoMoqData]
    public void AssertIsValid_does_not_throw_when_implementation_type_derives_from_service_type(ISelectsConstructor ctorSelector)
    {
      // Arrange
      var sut = new OpenGenericTypeRegistration(typeof(GenericService<>), ctorSelector) {
        ServiceType = typeof(IGenericService<>),
      };

      // Act & assert
      Assert.That(() => sut.AssertIsValid(), Throws.Nothing);
    }

    [Test,AutoMoqData]
    public void AssertIsValid_does_not_throw_when_implementation_type_is_same_as_service_type(ISelectsConstructor ctorSelector)
    {
      // Arrange
      var sut = new OpenGenericTypeRegistration(typeof(GenericService<>), ctorSelector) {
        ServiceType = typeof(GenericService<>),
      };

      // Act & assert
      Assert.That(() => sut.AssertIsValid(), Throws.Nothing);
    }

    protected override ServiceRegistration GetValidServiceRegistration()
      => new OpenGenericTypeRegistration(typeof(GenericService<>), GetConstructorSelector());
  }
}
