//
//    UnregisteredServiceResolverProxyTests.cs
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
using CSF.FlexDi.Registration;
using CSF.FlexDi.Resolution;
using CSF.FlexDi.Resolution.Proxies;
using CSF.FlexDi.Tests.Autofixture;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture.NUnit3;

namespace CSF.FlexDi.Tests.Resolution.Proxies
{
  [TestFixture, Parallelizable(ParallelScope.All)]
  public class UnregisteredServiceResolverProxyTests
  {
    [Test, AutoMoqData]
    public void Resolve_returns_result_from_proxy_when_resolution_is_successful([Frozen, ResolvesToFailure] IResolver proxiedResolver,
                                                                                UnregisteredServiceResolverProxy sut,
                                                                                ResolutionRequest request,
                                                                                ResolutionPath path,
                                                                                object resolved)
    {
      // Arrange
      var successResult = ResolutionResult.Success(path, resolved);
      Mock.Get(proxiedResolver)
          .Setup(x => x.Resolve(request))
          .Returns(successResult);

      // Act
      var result = sut.Resolve(request);

      // Assert
      Assert.That(result, Is.SameAs(successResult));
    }

    [Test, AutoMoqData]
    public void Resolve_resolves_unregistered_service_when_resolution_is_unsuccesful([Frozen, ResolvesToFailure] IResolver proxiedResolver,
                                                                                     [Frozen] IResolvesRegistrations registrationResolver,
                                                                                     [Frozen] IServiceRegistrationProvider unregisteredRegistrationProvider,
                                                                                     UnregisteredServiceResolverProxy sut,
                                                                                     ResolutionPath path,
                                                                                     ResolutionRequest request,
                                                                                     ResolutionResult resolutionResult,
                                                                                     IServiceRegistration registration)
    {
      // Arrange
      Mock.Get(proxiedResolver)
          .Setup(x => x.Resolve(request))
          .Returns(ResolutionResult.Failure(path));
      Mock.Get(unregisteredRegistrationProvider)
          .Setup(x => x.Get(request))
          .Returns(registration);
      Mock.Get(registrationResolver)
          .Setup(x => x.Resolve(request, registration))
          .Returns(resolutionResult);

      // Act
      var result = sut.Resolve(request);

      // Assert
      Assert.That(result, Is.SameAs(resolutionResult));
    }
  }
}
