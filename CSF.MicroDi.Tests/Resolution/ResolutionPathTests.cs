//
//    ResolutionPathTests.cs
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
using CSF.MicroDi.Tests.Autofixture;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture.NUnit3;

namespace CSF.MicroDi.Tests.Resolution
{
  [TestFixture,Parallelizable(ParallelScope.All)]
  public class ResolutionPathTests
  {
    [Test, AutoMoqData]
    public void Contains_returns_false_for_type_and_name_when_path_does_not_contain_any_matching_types(IServiceRegistration registration,
                                                                                                       string name)
    {
      // Arrange
      Mock.Get(registration).SetupGet(x => x.ServiceType).Returns(typeof(string));
      var sut = new ResolutionPath(registration);

      // Act
      var result = sut.Contains(typeof(int), name);

      // Assert
      Assert.That(result, Is.False);
    }

    [Test, AutoMoqData]
    public void Contains_returns_false_for_type_and_name_when_registration_in_path_has_different_name(IServiceRegistration registration,
                                                                                                      string name,
                                                                                                      string otherName)
    {
      // Arrange
      Mock.Get(registration).SetupGet(x => x.ServiceType).Returns(typeof(string));
      Mock.Get(registration).SetupGet(x => x.Name).Returns(name);
      var sut = new ResolutionPath(registration);

      // Act
      var result = sut.Contains(typeof(string), otherName);

      // Assert
      Assert.That(result, Is.False);
    }

    [Test, AutoMoqData]
    public void Contains_returns_true_for_type_and_name_when_registration_in_path_matches(IServiceRegistration registration,
                                                                                          string name)
    {
      // Arrange
      Mock.Get(registration).SetupGet(x => x.ServiceType).Returns(typeof(string));
      Mock.Get(registration).SetupGet(x => x.Name).Returns(name);
      var sut = new ResolutionPath(registration);

      // Act
      var result = sut.Contains(typeof(string), name);

      // Assert
      Assert.That(result, Is.True);
    }

    [Test, AutoMoqData]
    public void Contains_returns_true_for_type_and_name_when_request_has_no_name(IServiceRegistration registration,
                                                                                 string name)
    {
      // Arrange
      Mock.Get(registration).SetupGet(x => x.ServiceType).Returns(typeof(string));
      Mock.Get(registration).SetupGet(x => x.Name).Returns(name);
      var sut = new ResolutionPath(registration);

      // Act
      var result = sut.Contains(typeof(string));

      // Assert
      Assert.That(result, Is.True);
    }

    [Test, AutoMoqData]
    public void Contains_returns_false_for_registration_when_path_does_not_contain_any_matching_types(IServiceRegistration registration1,
                                                                                                      IServiceRegistration registration2,
                                                                                                      string name)
    {
      // Arrange
      Mock.Get(registration1).SetupGet(x => x.ServiceType).Returns(typeof(string));
      Mock.Get(registration2).SetupGet(x => x.ServiceType).Returns(typeof(int));
      var sut = new ResolutionPath(registration1);

      // Act
      var result = sut.Contains(registration2);

      // Assert
      Assert.That(result, Is.False);
    }

    [Test, AutoMoqData]
    public void Contains_returns_false_for_registration_when_registration_in_path_has_different_name(IServiceRegistration registration1,
                                                                                                     IServiceRegistration registration2,
                                                                                                     string name,
                                                                                                     string otherName)
    {
      // Arrange
      Mock.Get(registration1).SetupGet(x => x.ServiceType).Returns(typeof(string));
      Mock.Get(registration1).SetupGet(x => x.Name).Returns(name);
      Mock.Get(registration2).SetupGet(x => x.ServiceType).Returns(typeof(int));
      Mock.Get(registration2).SetupGet(x => x.Name).Returns(otherName);
      var sut = new ResolutionPath(registration1);

      // Act
      var result = sut.Contains(registration2);

      // Assert
      Assert.That(result, Is.False);
    }

    [Test, AutoMoqData]
    public void Contains_returns_true_for_registration_when_registration_in_path_matches(IServiceRegistration registration1,
                                                                                         IServiceRegistration registration2,
                                                                                         string name)
    {
      // Arrange
      Mock.Get(registration1).SetupGet(x => x.ServiceType).Returns(typeof(string));
      Mock.Get(registration1).SetupGet(x => x.Name).Returns(name);
      Mock.Get(registration2).SetupGet(x => x.ServiceType).Returns(typeof(string));
      Mock.Get(registration2).SetupGet(x => x.Name).Returns(name);
      var sut = new ResolutionPath(registration1);

      // Act
      var result = sut.Contains(registration2);

      // Assert
      Assert.That(result, Is.True);
    }

    [Test, AutoMoqData]
    public void Contains_returns_true_for_registration_when_request_has_no_name(IServiceRegistration registration1,
                                                                                IServiceRegistration registration2,
                                                                                string name)
    {
      // Arrange
      Mock.Get(registration1).SetupGet(x => x.ServiceType).Returns(typeof(string));
      Mock.Get(registration1).SetupGet(x => x.Name).Returns(name);
      Mock.Get(registration2).SetupGet(x => x.ServiceType).Returns(typeof(string));
      Mock.Get(registration2).SetupGet(x => x.Name).Returns((string) null);
      var sut = new ResolutionPath(registration1);

      // Act
      var result = sut.Contains(registration2);

      // Assert
      Assert.That(result, Is.True);
    }

    [Test, AutoMoqData]
    public void Contains_returns_false_for_typed_registration_when_impl_types_differ(TypedRegistration registration1,
                                                                                     TypedRegistration registration2,
                                                                                     string name)
    {
      // Arrange
      Mock.Get(registration1).SetupGet(x => x.ServiceType).Returns(typeof(string));
      Mock.Get(registration1).SetupGet(x => x.ImplementationType).Returns(typeof(string));
      Mock.Get(registration1).SetupGet(x => x.Name).Returns(name);
      Mock.Get(registration2).SetupGet(x => x.ServiceType).Returns(typeof(string));
      Mock.Get(registration2).SetupGet(x => x.ImplementationType).Returns(typeof(int));
      Mock.Get(registration2).SetupGet(x => x.Name).Returns(name);
      var sut = new ResolutionPath(registration1);

      // Act
      var result = sut.Contains(registration2);

      // Assert
      Assert.That(result, Is.False);
    }

    [Test, AutoMoqData]
    public void Contains_returns_true_for_typed_registration_when_impl_types_match(TypedRegistration registration1,
                                                                                   TypedRegistration registration2,
                                                                                   string name)
    {
      // Arrange
      Mock.Get(registration1).SetupGet(x => x.ServiceType).Returns(typeof(string));
      Mock.Get(registration1).SetupGet(x => x.ImplementationType).Returns(typeof(string));
      Mock.Get(registration1).SetupGet(x => x.Name).Returns(name);
      Mock.Get(registration2).SetupGet(x => x.ServiceType).Returns(typeof(string));
      Mock.Get(registration2).SetupGet(x => x.ImplementationType).Returns(typeof(string));
      Mock.Get(registration2).SetupGet(x => x.Name).Returns(name);
      var sut = new ResolutionPath(registration1);

      // Act
      var result = sut.Contains(registration2);

      // Assert
      Assert.That(result, Is.True);
    }
  }
}
