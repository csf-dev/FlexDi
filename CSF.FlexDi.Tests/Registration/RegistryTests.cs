//
//    RegistryTests.cs
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
using CSF.FlexDi.Registration;
using CSF.FlexDi.Tests.Autofixture;
using CSF.FlexDi.Tests.Stubs;
using Moq;
using NUnit.Framework;

namespace CSF.FlexDi.Tests.Registration
{
  [TestFixture,Parallelizable(ParallelScope.All)]
  public class RegistryTests
  {
    [Test,AutoMoqData]
    public void Add_can_add_a_registration(Registry sut,
                                           [Registration] IServiceRegistration registration)
    {
      // Act & assert
      Assert.That(() => sut.Add(registration), Throws.Nothing);
    }

    [Test,AutoMoqData]
    public void Add_can_add_two_differently_named_registrations(Registry sut,
                                                                [Registration(Name = "one")] IServiceRegistration registration1,
                                                                [Registration(Name = "two")] IServiceRegistration registration2)
    {
      // Arrange
      sut.Add(registration1);

      // Act & assert
      Assert.That(() => sut.Add(registration2), Throws.Nothing);
    }


    [Test,AutoMoqData]
    public void Add_does_not_raise_an_exception_when_adding_the_same_registration_twice(Registry sut,
                                                                                        [Registration] IServiceRegistration registration)
    {
      // Act
      sut.Add(registration);

      // Assert
      Assert.That(() => sut.Add(registration), Throws.Nothing);
    }

    [Test,AutoMoqData]
    public void Contains_returns_true_when_a_registration_has_been_added(Registry sut,
                                                                         [Registration] IServiceRegistration registration)
    {
      // Arrange
      sut.Add(registration);

      // Act
      var result = sut.Contains(ServiceRegistrationKey.ForRegistration(registration));

      // Assert
      Assert.That(result, Is.True);
    }

    [Test,AutoMoqData]
    public void Contains_returns_false_when_a_registration_has_not_been_added(Registry sut,
                                                                              [Registration] IServiceRegistration registration)
    {
      // Act
      var result = sut.Contains(ServiceRegistrationKey.ForRegistration(registration));

      // Assert
      Assert.That(result, Is.False);
    }

    [Test,AutoMoqData]
    public void Get_returns_a_registration_which_has_been_added(Registry sut,
                                                                [Registration] IServiceRegistration registration)
    {
      // Arrange
      sut.Add(registration);

      // Act
      var result = sut.Get(ServiceRegistrationKey.ForRegistration(registration));

      // Assert
      Assert.That(result, Is.SameAs(registration));
    }

    [Test,AutoMoqData]
    public void Get_returns_null_for_a_registration_which_has_not_been_added(Registry sut,
                                                                             [Registration] IServiceRegistration registration)

    {
      // Act
      var result = sut.Get(ServiceRegistrationKey.ForRegistration(registration));

      // Assert
      Assert.That(result, Is.Null);
    }

    [Test,AutoMoqData]
    public void GetAll_can_return_multiple_matching_registrations(Registry sut,
                                                                  [Registration(Name = "one")] IServiceRegistration registration1,
                                                                  [Registration(Name = "two")] IServiceRegistration registration2,
                                                                  [Registration(Name = "three")] IServiceRegistration registration3)
    {
      // Arrange
      sut.Add(registration1);
      sut.Add(registration2);
      sut.Add(registration3);

      var expected = new [] { registration1, registration2, registration3 };

      // Act
      var result = sut.GetAll(typeof(ISampleService));

      // Assert
      Assert.That(result, Is.EquivalentTo(expected));
    }

  }
}
