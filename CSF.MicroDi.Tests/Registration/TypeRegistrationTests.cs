//
//    TypeRegistrationTests.cs
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
using System.Collections;
using CSF.MicroDi.Registration;
using CSF.MicroDi.Resolution;
using CSF.MicroDi.Tests.Autofixture;
using CSF.MicroDi.Tests.Stubs;
using Moq;
using NUnit.Framework;

namespace CSF.MicroDi.Tests.Registration
{
  [TestFixture,Parallelizable(ParallelScope.All)]
  public class TypeRegistrationTests : ServiceRegistrationTestBase
  {
    [Test,AutoMoqData]
    public void GetFactoryAdapter_uses_constructor_selector(ISelectsConstructor ctorSelector,
                                                            ResolutionRequest request)
    {
      // Arrange
      var type = typeof(SampleServiceWithConstructorParameters);
      var sut = new TypeRegistration(type, ctorSelector);
      var ctor = type.GetConstructor(new [] { typeof(string), typeof(string)});
      Mock.Get(ctorSelector)
          .Setup(x => x.SelectConstructor(type))
          .Returns(ctor);

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
      var sut = new TypeRegistration(typeof(SampleServiceImplementationOne), ctorSelector) {
        ServiceType = typeof(IEnumerable),
      };

      // Act & assert
      Assert.That(() => sut.AssertIsValid(), Throws.InstanceOf<InvalidTypeRegistrationException>());
    }

    [Test,AutoMoqData]
    public void AssertIsValid_does_not_throw_when_implementation_type_derives_from_service_type(ISelectsConstructor ctorSelector)
    {
      // Arrange
      var sut = new TypeRegistration(typeof(SampleServiceImplementationOne), ctorSelector) {
        ServiceType = typeof(ISampleService),
      };

      // Act & assert
      Assert.That(() => sut.AssertIsValid(), Throws.Nothing);
    }

    [Test,AutoMoqData]
    public void AssertIsValid_does_not_throw_when_implementation_type_is_same_as_service_type(ISelectsConstructor ctorSelector)
    {
      // Arrange
      var sut = new TypeRegistration(typeof(SampleServiceImplementationOne), ctorSelector) {
        ServiceType = typeof(SampleServiceImplementationOne),
      };

      // Act & assert
      Assert.That(() => sut.AssertIsValid(), Throws.Nothing);
    }

    protected override ServiceRegistration GetValidServiceRegistration()
      => new TypeRegistration(typeof(SampleServiceImplementationOne), GetConstructorSelector());
  }
}
