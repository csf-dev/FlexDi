//
//    OptionalResolutionTests.cs
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
using CSF.FlexDi.Resolution;
using CSF.FlexDi.Tests.Stubs;
using NUnit.Framework;

namespace CSF.FlexDi.Tests.Integration
{
  [TestFixture,Parallelizable(ParallelScope.All)]
  public class OptionalResolutionTests
  {
    [Test]
    public void Resolving_an_unregistered_interface_throws_resolution_exception_when_optional_resolution_is_off()
    {
      // Arrange
      var container = Container.CreateBuilder().DoNotMakeAllResolutionOptional().BuildContainer();

      // Act & assert
      Assert.That(() => container.Resolve<ISampleService>(), Throws.InstanceOf<ResolutionException>());
    }

    [Test]
    public void Resolving_an_unregistered_interface_returns_null_when_optional_resolution_is_on()
    {
      // Arrange
      var container = Container.CreateBuilder().MakeAllResolutionOptional().BuildContainer();

      // Act
      var result = container.Resolve<ISampleService>();

      // Assert
      Assert.That(result, Is.Null);
    }

    [Test]
    public void Resolving_an_unregistered_dependency_throws_resolution_exception_when_optional_resolution_is_off()
    {
      // Arrange
      var container = Container.CreateBuilder()
                               .DoNotMakeAllResolutionOptional()
                               .DoNotResolveUnregisteredTypes()
                               .BuildContainer();
      container.AddRegistrations(helper => {
        helper.RegisterType<ServiceWithOtherChildDependency>();
      });

      // Act & assert
      Assert.That(() => container.Resolve<ServiceWithOtherChildDependency>(), Throws.InstanceOf<ResolutionException>());
    }

    [Test]
    public void Resolving_an_unregistered_dependency_returns_null_when_optional_resolution_is_on()
    {
      // Arrange
      var container = Container.CreateBuilder()
                               .MakeAllResolutionOptional()
                               .DoNotResolveUnregisteredTypes()
                               .BuildContainer();
      container.AddRegistrations(helper => {
        helper.RegisterType<ServiceWithOtherChildDependency>();
      });

      // Act
      var result = container.Resolve<ServiceWithOtherChildDependency>();

      // Assert
      Assert.That(result.OtherChild, Is.Null);
    }
  }
}
