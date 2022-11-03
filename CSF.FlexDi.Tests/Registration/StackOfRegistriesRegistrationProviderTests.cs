//
//    StackOfRegistriesRegistrationProviderTests.cs
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
using System.Collections.Generic;
using System.Linq;
using CSF.FlexDi.Registration;
using CSF.FlexDi.Resolution;
using CSF.FlexDi.Tests.Autofixture;
using CSF.FlexDi.Tests.Stubs;
using Moq;
using NUnit.Framework;

namespace CSF.FlexDi.Tests.Registration
{
  [TestFixture,Parallelizable(ParallelScope.All)]
  public class StackOfRegistriesRegistrationProviderTests
  {
    [Test,AutoMoqData]
    public void CanFulfilRequest_returns_true_if_any_contained_provider_can_fulfil_the_request(IServiceRegistrationProvider[] providers,
                                                                                               ResolutionRequest request)
    {
      // Arrange
      Mock.Get(providers[0]).Setup(x => x.CanFulfilRequest(request)).Returns(false);
      Mock.Get(providers[1]).Setup(x => x.CanFulfilRequest(request)).Returns(true);
      Mock.Get(providers[2]).Setup(x => x.CanFulfilRequest(request)).Returns(false);
      var sut = new StackOfRegistriesRegistrationProvider(providers);

      // Act
      var result = sut.CanFulfilRequest(request);

      // Assert
      Assert.That(result, Is.True);
    }

    [Test,AutoMoqData]
    public void CanFulfilRequest_returns_false_if_no_contained_provider_can_fulfil_the_request(IServiceRegistrationProvider[] providers,
                                                                                               ResolutionRequest request)
    {
      // Arrange
      Mock.Get(providers[0]).Setup(x => x.CanFulfilRequest(request)).Returns(false);
      Mock.Get(providers[1]).Setup(x => x.CanFulfilRequest(request)).Returns(false);
      Mock.Get(providers[2]).Setup(x => x.CanFulfilRequest(request)).Returns(false);
      var sut = new StackOfRegistriesRegistrationProvider(providers);

      // Act
      var result = sut.CanFulfilRequest(request);

      // Assert
      Assert.That(result, Is.False);
    }

    [Test,AutoMoqData]
    public void HasRegistration_returns_true_if_any_contained_provider_has_the_key(IServiceRegistrationProvider[] providers,
                                                                                   ServiceRegistrationKey key)
    {
      // Arrange
      Mock.Get(providers[0]).Setup(x => x.HasRegistration(key)).Returns(false);
      Mock.Get(providers[1]).Setup(x => x.HasRegistration(key)).Returns(true);
      Mock.Get(providers[2]).Setup(x => x.HasRegistration(key)).Returns(false);
      var sut = new StackOfRegistriesRegistrationProvider(providers);

      // Act
      var result = sut.HasRegistration(key);

      // Assert
      Assert.That(result, Is.True);
    }

    [Test,AutoMoqData]
    public void HasRegistration_returns_false_if_no_contained_provider_has_the_key(IServiceRegistrationProvider[] providers,
                                                                                   ServiceRegistrationKey key)
    {
      // Arrange
      Mock.Get(providers[0]).Setup(x => x.HasRegistration(key)).Returns(false);
      Mock.Get(providers[1]).Setup(x => x.HasRegistration(key)).Returns(false);
      Mock.Get(providers[2]).Setup(x => x.HasRegistration(key)).Returns(false);
      var sut = new StackOfRegistriesRegistrationProvider(providers);

      // Act
      var result = sut.HasRegistration(key);

      // Assert
      Assert.That(result, Is.False);
    }

    [Test,AutoMoqData]
    public void HasRegistration_returns_true_if_any_contained_provider_has_the_registration(IServiceRegistrationProvider[] providers,
                                                                                            IServiceRegistration registration)
    {
      // Arrange
      Mock.Get(providers[0]).Setup(x => x.HasRegistration(registration)).Returns(false);
      Mock.Get(providers[1]).Setup(x => x.HasRegistration(registration)).Returns(true);
      Mock.Get(providers[2]).Setup(x => x.HasRegistration(registration)).Returns(false);
      var sut = new StackOfRegistriesRegistrationProvider(providers);

      // Act
      var result = sut.HasRegistration(registration);

      // Assert
      Assert.That(result, Is.True);
    }

    [Test,AutoMoqData]
    public void HasRegistration_returns_false_if_no_contained_provider_has_the_registration(IServiceRegistrationProvider[] providers,
                                                                                            IServiceRegistration registration)
    {
      // Arrange
      Mock.Get(providers[0]).Setup(x => x.HasRegistration(registration)).Returns(false);
      Mock.Get(providers[1]).Setup(x => x.HasRegistration(registration)).Returns(false);
      Mock.Get(providers[2]).Setup(x => x.HasRegistration(registration)).Returns(false);
      var sut = new StackOfRegistriesRegistrationProvider(providers);

      // Act
      var result = sut.HasRegistration(registration);

      // Assert
      Assert.That(result, Is.False);
    }

    [Test,AutoMoqData]
    public void Get_returns_null_when_no_provider_can_fulfil_the_request(IServiceRegistrationProvider[] providers,
                                                                         ResolutionRequest request)
    {
      // Arrange
      Mock.Get(providers[0]).Setup(x => x.CanFulfilRequest(request)).Returns(false);
      Mock.Get(providers[1]).Setup(x => x.CanFulfilRequest(request)).Returns(false);
      Mock.Get(providers[2]).Setup(x => x.CanFulfilRequest(request)).Returns(false);
      var sut = new StackOfRegistriesRegistrationProvider(providers);

      // Act
      var result = sut.Get(request);

      // Assert
      Assert.That(result, Is.Null);
    }

    [Test,AutoMoqData]
    public void Get_gets_from_the_first_provider_which_can_fulfil_the_request(IServiceRegistrationProvider[] providers,
                                                                              ResolutionRequest request,
                                                                              IServiceRegistration registration)
    {
      // Arrange
      Mock.Get(providers[0]).Setup(x => x.CanFulfilRequest(request)).Returns(false);
      Mock.Get(providers[1]).Setup(x => x.CanFulfilRequest(request)).Returns(true);
      Mock.Get(providers[1]).Setup(x => x.Get(request)).Returns(registration);
      Mock.Get(providers[2]).Setup(x => x.CanFulfilRequest(request)).Returns(false);
      var sut = new StackOfRegistriesRegistrationProvider(providers);

      // Act
      var result = sut.Get(request);

      // Assert
      Assert.That(result, Is.SameAs(registration));
    }

    [Test,AutoMoqData]
    public void Get_only_gets_from_the_first_provider_which_can_fulfil_a_request(IServiceRegistrationProvider[] providers,
                                                                                 ResolutionRequest request,
                                                                                 IServiceRegistration registration)
    {
      // Arrange
      Mock.Get(providers[0]).Setup(x => x.CanFulfilRequest(request)).Returns(false);
      Mock.Get(providers[1]).Setup(x => x.CanFulfilRequest(request)).Returns(true);
      Mock.Get(providers[1]).Setup(x => x.Get(request)).Returns(registration);
      Mock.Get(providers[2]).Setup(x => x.CanFulfilRequest(request)).Returns(true);
      Mock.Get(providers[2]).Setup(x => x.Get(request)).Returns(registration);
      var sut = new StackOfRegistriesRegistrationProvider(providers);

      // Act
      var result = sut.Get(request);

      // Assert
      Mock.Get(providers[1]).Verify(x => x.Get(request), Times.Once);
      Mock.Get(providers[2]).Verify(x => x.Get(request), Times.Never);
    }

    [Test,AutoMoqData]
    public void GetAll_does_not_return_registrations_which_are_overridden_in_child_registries(IServiceRegistrationProvider[] providers,
                                                                                              [Registration(Name = "Same")] IServiceRegistration overriddenRegistration,
                                                                                              [Registration(Name = "Same")] IServiceRegistration overridingRegistration)
    {
      // Arrange
      Mock.Get(providers[2]).Setup(x => x.GetAll(null)).Returns(new [] { overriddenRegistration });
      Mock.Get(providers[0]).Setup(x => x.GetAll(null)).Returns(new [] { overridingRegistration });

      var sut = new StackOfRegistriesRegistrationProvider(providers);

      // Act
      var result = sut.GetAll();

      // Assert
      Assert.That(result, Has.Exactly(1).Items);
      Assert.That(result, Has.One.SameAs(overridingRegistration));
    }

    [Test,AutoMoqData]
    public void GetAll_can_merge_non_conflicting_registrations_from_multiple_providers(IServiceRegistrationProvider[] providers,
                                                                                       [Registration(Name = "One")] IServiceRegistration registration1,
                                                                                       [Registration(Name = "Two")] IServiceRegistration registration2)
    {
      // Arrange
      Mock.Get(providers[2]).Setup(x => x.GetAll(null)).Returns(new [] { registration1 });
      Mock.Get(providers[0]).Setup(x => x.GetAll(null)).Returns(new [] { registration2 });

      var sut = new StackOfRegistriesRegistrationProvider(providers);

      // Act
      var result = sut.GetAll();

      // Assert
      Assert.That(result, Has.Exactly(2).Items);
      Assert.That(result, Has.One.SameAs(registration1));
      Assert.That(result, Has.One.SameAs(registration2));
    }

    [Test,AutoMoqData]
    public void GetAll_can_filter_by_type_into_contained_providers(IServiceRegistrationProvider[] providers)
    {
      // Arrange
      var type = typeof(ISampleService);
      var sut = new StackOfRegistriesRegistrationProvider(providers);

      // Act
      var result = sut.GetAll(type);

      // Assert
      Mock.Get(providers[0]).Verify(x => x.GetAll(type), Times.Once);
      Mock.Get(providers[1]).Verify(x => x.GetAll(type), Times.Once);
      Mock.Get(providers[2]).Verify(x => x.GetAll(type), Times.Once);
    }
  }
}
