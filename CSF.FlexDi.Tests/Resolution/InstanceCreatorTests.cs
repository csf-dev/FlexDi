//
//    InstanceCreatorTests.cs
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
using System.Reflection;
using CSF.FlexDi.Registration;
using CSF.FlexDi.Resolution;
using CSF.FlexDi.Tests.Autofixture;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture.NUnit3;

namespace CSF.FlexDi.Tests.Resolution
{
  [TestFixture,Parallelizable(ParallelScope.All)]
  public class InstanceCreatorTests
  {
    [Test, AutoMoqData]
    public void CreateFromFactory_executes_factory_if_it_does_not_need_parameter_resolution([Frozen] IFulfilsResolutionRequests resolver,
                                                                                            InstanceCreator sut,
                                                                                            IFactoryAdapter factory,
                                                                                            ResolutionPath path,
                                                                                            IServiceRegistration registration,
                                                                                            object resolved)
    {
      // Arrange
      Mock.Get(factory).SetupGet(x => x.RequiresParameterResolution).Returns(false);
      Mock.Get(factory).Setup(x => x.Execute(It.Is<object[]>(e => e.Length == 0))).Returns(resolved);

      // Act
      var result = sut.CreateFromFactory(factory, path, registration);

      // Assert
      Mock.Get(factory).Verify(x => x.Execute(It.Is<object[]>(e => e.Length == 0)), Times.Once);
      Assert.That(result,  Is.SameAs(resolved));
    }

    [Test, AutoMoqData]
    public void CreateFromFactory_executes_factory_with_resolved_parameters([Frozen] IFulfilsResolutionRequests resolver,
                                                                            InstanceCreator sut,
                                                                            IFactoryAdapter factory,
                                                                            ResolutionPath path,
                                                                            IServiceRegistration registration,
                                                                            object resolved,
                                                                            [MockParam(typeof(string), Name = "Foo")] ParameterInfo paramOne,
                                                                            object paramOneValue,
                                                                            [MockParam(typeof(int), Name = "Bar")] ParameterInfo paramTwo,
                                                                            object paramTwoValue)
    {
      // Arrange
      Mock.Get(factory).SetupGet(x => x.RequiresParameterResolution).Returns(true);
      Mock.Get(factory).Setup(x => x.GetParameters()).Returns(new [] { paramOne, paramTwo });
      Mock.Get(resolver)
          .Setup(x => x.Resolve(It.Is<ResolutionRequest>(r => r.Name == paramOne.Name && r.ServiceType == paramOne.ParameterType)))
          .Returns(() => ResolutionResult.Success(path, paramOneValue));
      Mock.Get(resolver)
          .Setup(x => x.Resolve(It.Is<ResolutionRequest>(r => r.Name == paramTwo.Name && r.ServiceType == paramTwo.ParameterType)))
          .Returns(() => ResolutionResult.Success(path, paramTwoValue));
      Mock.Get(factory)
          .Setup(x => x.Execute(It.Is<object[]>(e => e.Length == 2 && e[0] == paramOneValue && e[1] == paramTwoValue)))
          .Returns(resolved);

      // Act
      var result = sut.CreateFromFactory(factory, path, registration);

      // Assert
      Mock.Get(factory)
          .Verify(x => x.Execute(It.Is<object[]>(e => e.Length == 2 && e[0] == paramOneValue && e[1] == paramTwoValue)), Times.Once);
      Assert.That(result,  Is.SameAs(resolved));
    }
  }
}
