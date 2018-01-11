//
//    RegisteredNameInjectingResolverProxyTests.cs
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
using NUnit.Framework;
using CSF.MicroDi.Resolution.Proxies;
using CSF.MicroDi.Tests.Stubs;
using CSF.MicroDi.Tests.Autofixture;
using CSF.MicroDi.Resolution;
using Ploeh.AutoFixture.NUnit3;
using Moq;
using CSF.MicroDi.Registration;

namespace CSF.MicroDi.Tests.Resolution.Proxies
{
  [TestFixture,Parallelizable(ParallelScope.All)]
  public class RegisteredNameInjectingResolverProxyTests
  {
    [Test,AutoMoqData]
    public void Resolve_uses_proxied_resolver_when_request_is_not_for_a_string([Frozen,ResolvesToFailure] IResolver proxiedResolver,
                                                                               RegisteredNameInjectingResolverProxy sut)
    {
      // Arrange
      var request = new ResolutionRequest(typeof(ISampleService));

      // Act
      var result = sut.Resolve(request);

      // Assert
      Mock.Get(proxiedResolver)
          .Verify(x => x.Resolve(request), Times.Once);
    }

    [Test,AutoMoqData]
    public void Resolve_uses_proxied_resolver_when_request_is_for_unusual_string([Frozen,ResolvesToFailure] IResolver proxiedResolver,
                                                                                 RegisteredNameInjectingResolverProxy sut,
                                                                                 string name)
    {
      // Arrange
      var request = new ResolutionRequest(typeof(string), name);

      // Act
      var result = sut.Resolve(request);

      // Assert
      Mock.Get(proxiedResolver)
          .Verify(x => x.Resolve(request), Times.Once);
    }

    [Test,AutoMoqData]
    public void Resolve_gets_registered_name_when_request_is_for_registeredName([Frozen,ResolvesToFailure] IResolver proxiedResolver,
                                                                                RegisteredNameInjectingResolverProxy sut,
                                                                                IServiceRegistration currentReg,
                                                                                string registeredName)
    {
      // Arrange
      var path = new ResolutionPath(currentReg);
      var request = new ResolutionRequest(typeof(string), "registeredName", path);
      Mock.Get(currentReg).SetupGet(x => x.Name).Returns(registeredName);

      // Act
      var result = sut.Resolve(request);

      // Assert
      Mock.Get(proxiedResolver).Verify(x => x.Resolve(request), Times.Never);
      Assert.That(result.ResolvedObject, Is.EqualTo(registeredName));
    }
  }
}
