//
//    CircularDependencyPreventingResolverProxyTests.cs
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
using CSF.MicroDi.Registration;
using CSF.MicroDi.Resolution;
using CSF.MicroDi.Resolution.Proxies;
using CSF.MicroDi.Tests.Autofixture;
using CSF.MicroDi.Tests.Stubs;
using Moq;
using NUnit.Framework;

namespace CSF.MicroDi.Tests.Resolution.Proxies
{
  [TestFixture,Parallelizable(ParallelScope.All)]
  public class CircularDependencyPreventingResolverProxyTests
  {
    [Test,AutoMoqData]
    public void Resolve_calls_throw_on_circular_dependency_from_detector_service(IDetectsCircularDependencies detector,
                                                                                 [ResolvesToFailure] IResolver proxiedResolver,
                                                                                 ResolutionPath path,
                                                                                 [Registration] IServiceRegistration registration)
    {
      // Arrange
      var request = new ResolutionRequest(typeof(ISampleService), path);
      Mock.Get(proxiedResolver).Setup(x => x.GetRegistration(request)).Returns(registration);
      var sut = new CircularDependencyPreventingResolverProxy(proxiedResolver, detector);

      // Act
      sut.Resolve(request);

      // Assert
      Mock.Get(detector).Verify(x => x.ThrowOnCircularDependency(registration, path), Times.Once);
    }
  }
}
