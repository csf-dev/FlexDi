//
//    DynamicRecursionResolverProxyTests.cs
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
using CSF.FlexDi.Registration;
using CSF.FlexDi.Resolution;
using CSF.FlexDi.Resolution.Proxies;
using CSF.FlexDi.Tests.Autofixture;
using CSF.FlexDi.Tests.Stubs;
using Moq;
using NUnit.Framework;

namespace CSF.FlexDi.Tests.Resolution.Proxies
{
  [TestFixture,Parallelizable(ParallelScope.All)]
  public class DynamicRecursionResolverProxyTests
  {
    [Test,AutoMoqData]
    public void Resolve_resolves_from_proxied_resolver_when_service_type_is_not_IResolvesServices([ResolvesToFailure] IResolver proxiedResolver,
                                                                                                  [Registration] IServiceRegistration registration)
    {
      // Arrange
      var sut = new DynamicRecursionResolverProxy(proxiedResolver);
      var request = new ResolutionRequest(typeof(ISampleService), new ResolutionPath(registration));

      // Act
      sut.Resolve(request);

      // Assert
      Mock.Get(proxiedResolver).Verify(x => x.Resolve(request), Times.Once);
      Mock.Get(proxiedResolver).Verify(x => x.Resolve(It.Is<ResolutionRequest>(r => r != request)), Times.Never);
    }

    [Test,AutoMoqData]
    public void Resolve_resolves_from_proxied_resolver_when_resolution_path_is_empty([ResolvesToFailure] IResolver proxiedResolver)
    {
      // Arrange
      var sut = new DynamicRecursionResolverProxy(proxiedResolver);
      var request = new ResolutionRequest(typeof(IResolvesServices), new ResolutionPath());

      // Act
      sut.Resolve(request);

      // Assert
      Mock.Get(proxiedResolver).Verify(x => x.Resolve(request), Times.Once);
      Mock.Get(proxiedResolver).Verify(x => x.Resolve(It.Is<ResolutionRequest>(r => r != request)), Times.Never);
    }

    [Test,AutoMoqData]
    public void Resolve_returns_container_within_proxy_when_applicable([ResolvesToFailure] IResolver proxiedResolver,
                                                                       [Registration] IServiceRegistration registration,
                                                                       IContainer container)
    {
      // Arrange
      var sut = new DynamicRecursionResolverProxy(proxiedResolver);
      var request = new ResolutionRequest(typeof(IResolvesServices), new ResolutionPath(registration));
      Mock.Get(proxiedResolver)
          .Setup(x => x.Resolve(It.Is<ResolutionRequest>(r => r.ServiceType == typeof(IContainer))))
          .Returns(ResolutionResult.Success(new ResolutionPath(), container));

      // Act
      var result = sut.Resolve(request);

      // Assert
      Assert.That(result.IsSuccess, Is.True);
      Assert.That(result.ResolvedObject, Is.InstanceOf<ServiceResolvingContainerProxy>());
      var proxy = (ServiceResolvingContainerProxy) result.ResolvedObject;
      Assert.That(proxy.ProxiedResolver, Is.SameAs(container));
    }

    [Test,AutoMoqData]
    public void Resolve_returns_failure_result_if_container_cannot_be_resolved([ResolvesToFailure] IResolver proxiedResolver,
                                                                               [Registration] IServiceRegistration registration)
    {
      // Arrange
      var sut = new DynamicRecursionResolverProxy(proxiedResolver);
      var request = new ResolutionRequest(typeof(IResolvesServices), new ResolutionPath(registration));

      // Act
      var result = sut.Resolve(request);

      // Assert
      Assert.That(result.IsSuccess, Is.False);
    }
  }
}
