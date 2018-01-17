//
//    ServiceInstanceDisposerTests.cs
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
using CSF.FlexDi.Tests.Autofixture;
using CSF.FlexDi.Tests.Stubs;
using Moq;
using NUnit.Framework;

namespace CSF.FlexDi.Tests.Resolution
{
  [TestFixture,Parallelizable(ParallelScope.All)]
  public class ServiceInstanceDisposerTests
  {
    [Test,AutoMoqData]
    public void DisposeInstances_does_not_dispose_non_cacheable_items(IServiceRegistrationProvider registrations,
                                                                      ICachesResolvedServiceInstances cache,
                                                                      ServiceInstanceDisposer sut,
                                                                      IServiceRegistration registration,
                                                                      IDisposable disposable)
    {
      // Arrange
      Mock.Get(registration).SetupGet(x => x.DisposeWithContainer).Returns(true);
      Mock.Get(registration).SetupGet(x => x.Cacheable).Returns(false);
      Mock.Get(registrations).Setup(x => x.GetAll()).Returns(new [] { registration });
      object obj = disposable;
      Mock.Get(cache)
          .Setup(x => x.TryGet(registration, out obj))
          .Callback(() => obj = disposable)
          .Returns(true);

      // Act
      sut.DisposeInstances(registrations, cache);

      // Assert
      Mock.Get(disposable).Verify(x => x.Dispose(), Times.Never);
    }

    [Test,AutoMoqData]
    public void DisposeInstances_does_not_dispose_non_disposed_items(IServiceRegistrationProvider registrations,
                                                                     ICachesResolvedServiceInstances cache,
                                                                     ServiceInstanceDisposer sut,
                                                                     IServiceRegistration registration,
                                                                     IDisposable disposable)
    {
      // Arrange
      Mock.Get(registration).SetupGet(x => x.DisposeWithContainer).Returns(false);
      Mock.Get(registration).SetupGet(x => x.Cacheable).Returns(true);
      Mock.Get(registrations).Setup(x => x.GetAll()).Returns(new [] { registration });
      object obj = disposable;
      Mock.Get(cache)
          .Setup(x => x.TryGet(registration, out obj))
          .Callback(() => obj = disposable)
          .Returns(true);

      // Act
      sut.DisposeInstances(registrations, cache);

      // Assert
      Mock.Get(disposable).Verify(x => x.Dispose(), Times.Never);
    }

    [Test,AutoMoqData]
    public void DisposeInstances_does_not_dispose_non_cached_instances(IServiceRegistrationProvider registrations,
                                                                       ICachesResolvedServiceInstances cache,
                                                                       ServiceInstanceDisposer sut,
                                                                       IServiceRegistration registration,
                                                                       IDisposable disposable)
    {
      // Arrange
      Mock.Get(registration).SetupGet(x => x.DisposeWithContainer).Returns(true);
      Mock.Get(registration).SetupGet(x => x.Cacheable).Returns(true);
      Mock.Get(registrations).Setup(x => x.GetAll()).Returns(new [] { registration });
      object obj = disposable;
      Mock.Get(cache)
          .Setup(x => x.TryGet(registration, out obj))
          .Returns(false);

      // Act
      sut.DisposeInstances(registrations, cache);

      // Assert
      Mock.Get(disposable).Verify(x => x.Dispose(), Times.Never);
    }

    [Test,AutoMoqData]
    public void DisposeInstances_does_not_dispose_non_disposable_objects(IServiceRegistrationProvider registrations,
                                                                         ICachesResolvedServiceInstances cache,
                                                                         ServiceInstanceDisposer sut,
                                                                         IServiceRegistration registration,
                                                                         object nonDisposable)
    {
      // Arrange
      Mock.Get(registration).SetupGet(x => x.DisposeWithContainer).Returns(false);
      Mock.Get(registration).SetupGet(x => x.Cacheable).Returns(true);
      Mock.Get(registrations).Setup(x => x.GetAll()).Returns(new [] { registration });
      object obj = nonDisposable;
      Mock.Get(cache)
          .Setup(x => x.TryGet(registration, out obj))
          .Callback(() => obj = nonDisposable)
          .Returns(true);

      // Act
      Assert.That(() => sut.DisposeInstances(registrations, cache), Throws.Nothing);
    }

    [Test,AutoMoqData]
    public void DisposeInstances_disposes_a_cached_disposable_item(IServiceRegistrationProvider registrations,
                                                                   ICachesResolvedServiceInstances cache,
                                                                   ServiceInstanceDisposer sut,
                                                                   IServiceRegistration registration,
                                                                   IDisposable disposable)
    {
      // Arrange
      Mock.Get(registration).SetupGet(x => x.DisposeWithContainer).Returns(true);
      Mock.Get(registration).SetupGet(x => x.Cacheable).Returns(true);
      Mock.Get(registrations).Setup(x => x.GetAll()).Returns(new [] { registration });
      object obj = disposable;
      Mock.Get(cache)
          .Setup(x => x.TryGet(registration, out obj))
          .Callback(() => obj = disposable)
          .Returns(true);

      // Act
      sut.DisposeInstances(registrations, cache);

      // Assert
      Mock.Get(disposable).Verify(x => x.Dispose(), Times.Once);
    }
  }
}
