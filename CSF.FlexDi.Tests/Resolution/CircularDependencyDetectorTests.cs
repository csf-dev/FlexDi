//
//    CircularDependencyDetectorTests.cs
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
using NUnit.Framework;

namespace CSF.FlexDi.Tests.Resolution
{
  [TestFixture,Parallelizable(ParallelScope.All)]
  public class CircularDependencyDetectorTests
  {
    [Test,AutoMoqData]
    public void HasCircularDependency_returns_false_when_there_is_no_circular_dependency(IServiceRegistration registration1,
                                                                                         IServiceRegistration registration2,
                                                                                         IServiceRegistration registration3,
                                                                                         CircularDependencyDetector sut)
    {
      // Arrange
      var path = new ResolutionPath(new [] { registration1, registration2 });

      // Act
      var result = sut.HasCircularDependency(registration3, path);

      // Assert
      Assert.That(result, Is.False);
    }

    [Test,AutoMoqData]
    public void HasCircularDependency_returns_true_when_there_is_a_circular_dependency(IServiceRegistration registration1,
                                                                                       IServiceRegistration registration2,
                                                                                       IServiceRegistration registration3,
                                                                                       CircularDependencyDetector sut)
    {
      // Arrange
      var path = new ResolutionPath(new [] { registration1, registration2, registration3 });

      // Act
      var result = sut.HasCircularDependency(registration2, path);

      // Assert
      Assert.That(result, Is.True);
    }

    [Test,AutoMoqData]
    public void ThrowOnCircularDependency_does_not_throw_when_there_is_no_circular_dependency(IServiceRegistration registration1,
                                                                                              IServiceRegistration registration2,
                                                                                              IServiceRegistration registration3,
                                                                                              CircularDependencyDetector sut)
    {
      // Arrange
      var path = new ResolutionPath(new [] { registration1, registration2 });

      // Act & assert
      Assert.That(() => sut.ThrowOnCircularDependency(registration3, path), Throws.Nothing);
    }

    [Test,AutoMoqData]
    public void ThrowOnCircularDependency_throws_exception_when_there_is_a_circular_dependency(IServiceRegistration registration1,
                                                                                               IServiceRegistration registration2,
                                                                                               IServiceRegistration registration3,
                                                                                               CircularDependencyDetector sut)
    {
      // Arrange
      var path = new ResolutionPath(new [] { registration1, registration2, registration3 });

      // Act & assert
      Assert.That(() => sut.ThrowOnCircularDependency(registration2, path),
                  Throws.InstanceOf<CircularDependencyException>());
    }
  }
}
