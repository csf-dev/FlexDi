using System;
using System.Linq;
using CSF.MicroDi.Registration;
using CSF.MicroDi.Resolution;
using CSF.MicroDi.Tests.Autofixture;
using CSF.MicroDi.Tests.Stubs;
using Moq;
using NUnit.Framework;

namespace CSF.MicroDi.Tests.Resolution
{
  [TestFixture,Parallelizable(ParallelScope.All)]
  public class ResolverTests
  {
    [Test,AutoMoqData]
    public void Resolve_using_request_can_get_registered_service(IServiceRegistrationProvider provider,
                                                                 [SampleService] IServiceRegistration registration,
                                                                 ResolutionRequest request)
    {
      // Arrange
      var sut = GetSut(provider);
      var instance = new SampleServiceImplementationOne();
      Mock.Get(provider).Setup(x => x.CanFulfilRequest(request)).Returns(true);
      Mock.Get(provider).Setup(x => x.Get(request)).Returns(registration);
      Mock.Get(registration)
        .Setup(x => x.CreateInstance(sut))
        .Returns(instance);

      // Act
      object result;
      sut.Resolve(request, out result);

      // Assert
      Assert.That(result, Is.SameAs(instance));
    }

    [Test,AutoMoqData]
    public void Resolve_using_request_can_get_registered_service_without_name(IServiceRegistrationProvider provider,
                                                                              [SampleService] IServiceRegistration registration,
                                                                              ResolutionRequest request)
    {
      // Arrange
      var sut = GetSut(provider);
      var instance = new SampleServiceImplementationOne();
      Mock.Get(provider)
          .Setup(x => x.CanFulfilRequest(request))
          .Returns(false);
      Mock.Get(provider)
          .Setup(x => x.Get(request))
          .Returns((IServiceRegistration) null);
      Mock.Get(provider)
          .Setup(x => x.CanFulfilRequest(It.Is<ResolutionRequest>(r => r.ServiceType == request.ServiceType && r.Name == null)))
          .Returns(true);
      Mock.Get(provider)
          .Setup(x => x.Get(It.Is<ResolutionRequest>(r => r.ServiceType == request.ServiceType && r.Name == null)))
          .Returns(registration);
      Mock.Get(registration)
          .Setup(x => x.CreateInstance(sut))
          .Returns(instance);

      // Act
      object result;
      sut.Resolve(request, out result);

      // Assert
      Assert.That(result, Is.SameAs(instance));
    }

    [Test,AutoMoqData]
    public void Resolve_using_request_can_get_registered_service_without_name(IServiceRegistrationProvider provider,
                                                                              IServiceRegistrationProvider unregisteredProvider,
                                                                              [SampleService] IServiceRegistration registration,
                                                                              ResolutionRequest request)
    {
      // Arrange
      var sut = GetSut(provider, unregisteredProvider);
      var instance = new SampleServiceImplementationOne();
      Mock.Get(provider)
          .Setup(x => x.CanFulfilRequest(request))
          .Returns(false);
      Mock.Get(provider)
          .Setup(x => x.Get(It.IsAny<ResolutionRequest>()))
          .Returns((IServiceRegistration) null);
      Mock.Get(unregisteredProvider)
          .Setup(x => x.CanFulfilRequest(request))
          .Returns(true);
      Mock.Get(unregisteredProvider)
          .Setup(x => x.Get(request))
          .Returns(registration);
      Mock.Get(registration)
          .Setup(x => x.CreateInstance(sut))
          .Returns(instance);

      // Act
      object result;
      sut.Resolve(request, out result);

      // Assert
      Assert.That(result, Is.SameAs(instance));
    }

    [Test,AutoMoqData,Category("Integration")]
    public void Resolve_using_factory_adapter_and_unregistered_resolver_can_resolve_services_integration_test(IServiceRegistrationProvider provider)
    {
      // Arrange
      var sut = GetSut(provider);
      Mock.Get(provider)
          .Setup(x => x.CanFulfilRequest(It.IsAny<ResolutionRequest>()))
          .Returns(false);

      // Act
      object result;
      sut.Resolve(new ResolutionRequest(typeof(ParentService)), out result);

      // Assert
      Assert.That(result, Is.Not.Null);
      Assert.That(result, Is.InstanceOf<ParentService>());
      var parentResult = (ParentService) result;
      Assert.That(parentResult.ChildOne, Is.Not.Null);
      Assert.That(parentResult.ChildTwo, Is.Not.Null);
    }

    Resolver GetSut(IServiceRegistrationProvider provider, IServiceRegistrationProvider unregisteredProvider = null)
    {
      return new Resolver(provider, unregisteredProvider);
    }
  }
}
