//
//    FallbackResolverProxyTests.cs
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
using CSF.FlexDi.Tests.Stubs;
using Moq;
using NUnit.Framework;

namespace CSF.FlexDi.Tests.Resolution.Proxies
{
  [TestFixture,Parallelizable(ParallelScope.All)]
  public class FallbackResolverProxyTests
  {
    [Test,AutoMoqData]
    public void Resolve_uses_fallback_resolver_if_primary_resolver_returns_failure_result([ResolvesToFailure] IResolver primaryResolver,
                                                                                          [ResolvesToFailure] IResolver fallbackResolver,
                                                                                          ResolutionRequest request)
    {
      // Arrange
      var sut = new FallbackResolverProxy(primaryResolver, fallbackResolver);

      // Act
      sut.Resolve(request);

      // Assert
      Mock.Get(primaryResolver).Verify(x => x.Resolve(request), Times.Once);
      Mock.Get(fallbackResolver).Verify(x => x.Resolve(request), Times.Once);
    }

    [Test,AutoMoqData]
    public void Resolve_does_not_use_fallback_resolver_if_primary_resolver_returns_success_result([ResolvesToFailure] IResolver primaryResolver,
                                                                                                  [ResolvesToFailure] IResolver fallbackResolver,
                                                                                                  ResolutionRequest request,
                                                                                                  ResolutionPath path,
                                                                                                  object resolved)
    {
      // Arrange
      var sut = new FallbackResolverProxy(primaryResolver, fallbackResolver);
      Mock.Get(primaryResolver)
          .Setup(x => x.Resolve(request))
          .Returns(ResolutionResult.Success(path, resolved));

      // Act
      sut.Resolve(request);

      // Assert
      Mock.Get(primaryResolver).Verify(x => x.Resolve(request), Times.Once);
      Mock.Get(fallbackResolver).Verify(x => x.Resolve(request), Times.Never);
    }
  }
}
