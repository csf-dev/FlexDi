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
  public class RegistryStackTests
  {
    [Test,AutoMoqData]
    public void Add_adds_registration_to_current_provider(IRegistersServices provider,
                                                          [SampleService] IServiceRegistration registration)
    {
      // Arrange
      var sut = GetSut(provider);

      // Act
      sut.Add(registration);

      // Assert
      Mock.Get(provider)
          .Verify(x => x.Add(registration), Times.Once);
    }

    [Test,AutoMoqData]
    public void Add_does_not_add_registration_to_wrong_provider(IRegistersServices provider,
                                                                IRegistersServices otherProvider,
                                                                [SampleService] IServiceRegistration registration)
    {
      // Arrange
      var sut = GetSut(provider, otherProvider);

      // Act
      sut.Add(registration);

      // Assert
      Mock.Get(provider)
          .Verify(x => x.Add(registration), Times.Once);
      Mock.Get(otherProvider)
          .Verify(x => x.Add(It.IsAny<IServiceRegistration>()), Times.Never);
    }

    [Theory,AutoMoqData]
    public void CanFulfilRequest_can_return_result_from_single_provider(IRegistersServices provider,
                                                                        ResolutionRequest request,
                                                                        bool canFulfil)
    {
      // Arrange
      var sut = GetSut(provider);
      Mock.Get(provider).Setup(x => x.CanFulfilRequest(request)).Returns(canFulfil);

      // Act
      var result = sut.CanFulfilRequest(request);

      // Assert
      Assert.That(result, Is.EqualTo(canFulfil));
    }

    [Test,AutoMoqData]
    public void CanFulfilRequest_can_return_positive_result_from_other_provider(IRegistersServices provider,
                                                                                IRegistersServices otherProvider,
                                                                                ResolutionRequest request)
    {
      // Arrange
      var sut = GetSut(provider, otherProvider);
      Mock.Get(provider).Setup(x => x.CanFulfilRequest(request)).Returns(false);
      Mock.Get(otherProvider).Setup(x => x.CanFulfilRequest(request)).Returns(true);

      // Act
      var result = sut.CanFulfilRequest(request);

      // Assert
      Assert.That(result, Is.True);
    }

    [Test,AutoMoqData]
    public void CanFulfilRequest_returns_false_if_no_provider_suitable(IRegistersServices provider,
                                                                       IRegistersServices otherProvider,
                                                                       ResolutionRequest request)
    {
      // Arrange
      var sut = GetSut(provider, otherProvider);
      Mock.Get(provider).Setup(x => x.CanFulfilRequest(request)).Returns(false);
      Mock.Get(otherProvider).Setup(x => x.CanFulfilRequest(request)).Returns(false);

      // Act
      var result = sut.CanFulfilRequest(request);

      // Assert
      Assert.That(result, Is.False);
    }

    [Test,AutoMoqData]
    public void Get_can_return_registration_from_current_provider(IRegistersServices provider,
                                                                  [SampleService] IServiceRegistration registration,
                                                                  ResolutionRequest request)
    {
      // Arrange
      var sut = GetSut(provider);
      Mock.Get(provider).Setup(x => x.CanFulfilRequest(request)).Returns(true);
      Mock.Get(provider).Setup(x => x.Get(request)).Returns(registration);

      // Act
      var result = sut.Get(request);

      // Assert
      Assert.That(result, Is.SameAs(registration));
    }

    [Test,AutoMoqData]
    public void Get_returns_registration_from_other_provider_if_current_cannot_fulfil(IRegistersServices provider,
                                                                                      IRegistersServices otherProvider,
                                                                                      [SampleService] IServiceRegistration registration,
                                                                                      ResolutionRequest request)
    {
      // Arrange
      var sut = GetSut(provider, otherProvider);
      Mock.Get(provider).Setup(x => x.CanFulfilRequest(request)).Returns(false);
      Mock.Get(provider).Setup(x => x.Get(request)).Returns((IServiceRegistration) null);
      Mock.Get(otherProvider).Setup(x => x.CanFulfilRequest(request)).Returns(true);
      Mock.Get(otherProvider).Setup(x => x.Get(request)).Returns(registration);

      // Act
      var result = sut.Get(request);

      // Assert
      Assert.That(result, Is.SameAs(registration));
    }

    [Test,AutoMoqData]
    public void Get_does_not_call_get_on_a_provider_which_cannot_provide_the_service(IRegistersServices provider,
                                                                                     IRegistersServices otherProvider,
                                                                                     [SampleService] IServiceRegistration registration,
                                                                                     ResolutionRequest request)
    {
      // Arrange
      var sut = GetSut(provider, otherProvider);
      Mock.Get(provider).Setup(x => x.CanFulfilRequest(request)).Returns(false);
      Mock.Get(provider).Setup(x => x.Get(request)).Returns((IServiceRegistration) null);
      Mock.Get(otherProvider).Setup(x => x.CanFulfilRequest(request)).Returns(true);
      Mock.Get(otherProvider).Setup(x => x.Get(request)).Returns(registration);

      // Act
      var result = sut.Get(request);

      // Assert
      Mock.Get(provider).Verify(x => x.Get(It.IsAny<ResolutionRequest>()), Times.Never);
    }

    [Test,AutoMoqData]
    public void Get_returns_registration_from_first_provider_when_many_can_provide_it(IRegistersServices provider,
                                                                                      IRegistersServices otherProvider,
                                                                                      [SampleService] IServiceRegistration registration,
                                                                                      [SampleService] IServiceRegistration otherRegistration,
                                                                                      ResolutionRequest request)
    {
      // Arrange
      var sut = GetSut(provider, otherProvider);
      Mock.Get(provider).Setup(x => x.CanFulfilRequest(request)).Returns(true);
      Mock.Get(provider).Setup(x => x.Get(request)).Returns(registration);
      Mock.Get(otherProvider).Setup(x => x.CanFulfilRequest(request)).Returns(true);
      Mock.Get(otherProvider).Setup(x => x.Get(request)).Returns(otherRegistration);

      // Act
      var result = sut.Get(request);

      // Assert
      Assert.That(result, Is.SameAs(registration));
    }

    [Test,AutoMoqData]
    public void Get_returns_null_when_no_provider_has_the_registration(IRegistersServices provider,
                                                                       IRegistersServices otherProvider,
                                                                       ResolutionRequest request)
    {
      // Arrange
      var sut = GetSut(provider, otherProvider);
      Mock.Get(provider).Setup(x => x.CanFulfilRequest(request)).Returns(false);
      Mock.Get(otherProvider).Setup(x => x.CanFulfilRequest(request)).Returns(false);

      // Act
      var result = sut.Get(request);

      // Assert
      Assert.That(result, Is.Null);
    }

    [Test,AutoMoqData]
    public void GetAll_returns_many_services_from_one_provider(IRegistersServices provider,
                                                               [SampleService] IServiceRegistration registration,
                                                               [SampleService] IServiceRegistration otherRegistration,
                                                               Type type)
    {
      // Arrange
      var sut = GetSut(provider);
      Mock.Get(provider).Setup(x => x.GetAll(type)).Returns(new [] { registration, otherRegistration });
      var expected = new [] { registration, otherRegistration };

      // Act
      var result = sut.GetAll(type);

      // Assert
      Assert.That(result, Is.EquivalentTo(expected));
    }

    [Test,AutoMoqData]
    public void GetAll_returns_many_services_from_many_providers(IRegistersServices provider,
                                                                 IRegistersServices otherProvider,
                                                                 [SampleService] IServiceRegistration reg1,
                                                                 [SampleService] IServiceRegistration reg2,
                                                                 [SampleService] IServiceRegistration reg3,
                                                                 [SampleService] IServiceRegistration reg4,
                                                                 Type type)
    {
      var sut = GetSut(provider, otherProvider);
      Mock.Get(provider).Setup(x => x.GetAll(type)).Returns(new [] { reg1, reg2 });
      Mock.Get(otherProvider).Setup(x => x.GetAll(type)).Returns(new [] { reg3, reg4 });
      var expected = new [] { reg1, reg2, reg3, reg4 };

      // Act
      var result = sut.GetAll(type);

      // Assert
      Assert.That(result, Is.EquivalentTo(expected));
    }

    RegistryStack GetSut(IRegistersServices currentProvider, params IRegistersServices[] otherProviders)
    {
      return new RegistryStack(otherProviders, currentProvider);
    }
  }
}
