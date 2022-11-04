//
//    ResolverTests.cs
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
using System.Linq;
using CSF.FlexDi.Registration;
using CSF.FlexDi.Resolution;
using CSF.FlexDi.Tests.Autofixture;
using CSF.FlexDi.Tests.Stubs;
using Moq;
using NUnit.Framework;

namespace CSF.FlexDi.Tests.Resolution
{
  [TestFixture,Parallelizable(ParallelScope.All)]
  public class ResolverTests
  {
    [Test,AutoMoqData]
    public void Resolve_using_request_can_get_registered_service(IServiceRegistrationProvider provider,
                                                                 [Registration] IServiceRegistration registration,
                                                                 ResolutionRequest request)
    {
      // Arrange
      var sut = GetSut(provider);
      var instance = new SampleServiceImplementationOne();
      Mock.Get(provider).Setup(x => x.HasRegistration(request)).Returns(true);
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
                                                                              [Registration] IServiceRegistration registration,
                                                                              ResolutionRequest request)
    {
      // Arrange
      var sut = GetSut(provider);
      var instance = new SampleServiceImplementationOne();
      Mock.Get(provider)
          .Setup(x => x.HasRegistration(request))
          .Returns(false);
      Mock.Get(provider)
          .Setup(x => x.Get(request))
          .Returns((IServiceRegistration) null);
      Mock.Get(provider)
          .Setup(x => x.HasRegistration(It.Is<ServiceRegistrationKey>(r => r.ServiceType == request.ServiceType && r.Name == null)))
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
