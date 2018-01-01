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
        .Setup(x => x.GetFactoryAdapter(request))
        .Returns(new InstanceFactory(instance));

      // Act
      var result = sut.Resolve(request);

      // Assert
      Assert.That(result.ResolvedObject, Is.SameAs(instance));
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
          .Setup(x => x.GetFactoryAdapter(request))
          .Returns(new InstanceFactory(instance));

      // Act
      var result = sut.Resolve(request);

      // Assert
      Assert.That(result.ResolvedObject, Is.SameAs(instance));
    }

    Resolver GetSut(IServiceRegistrationProvider provider, IServiceRegistrationProvider unregisteredProvider = null)
    {
      var instanceCreator = new Mock<ICreatesObjectInstances>();
      instanceCreator
        .Setup(x => x.CreateFromFactory(It.IsAny<IFactoryAdapter>(),
                                        It.IsAny<ResolutionPath>(),
                                        It.IsAny<IServiceRegistration>()))
        .Returns((IFactoryAdapter a, ResolutionPath p, IServiceRegistration r) => CreateInstance(a, p, r));
      
      return new Resolver(provider, instanceCreator.Object);
    }

    object CreateInstance(IFactoryAdapter adapter, ResolutionPath path, IServiceRegistration registration)
      => adapter.Execute(new object[0]);
  }
}
