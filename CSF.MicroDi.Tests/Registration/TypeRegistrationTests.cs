//
//    TypeRegistrationTests.cs
//
//    Copyright 2018  Craig Fowler et al
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
using CSF.MicroDi.Tests.Stubs;
using Moq;
using NUnit.Framework;

namespace CSF.MicroDi.Tests.Registration
{
  [TestFixture,Parallelizable(ParallelScope.All)]
  public class TypeRegistrationTests
  {
    [Test,AutoMoqData]
    public void GetFactoryAdapter_uses_constructor_selector(ISelectsConstructor ctorSelector,
                                                            ResolutionRequest request)
    {
      // Arrange
      var type = typeof(SampleServiceWithConstructorParameters);
      var sut = new TypeRegistration(type, ctorSelector);
      var ctor = type.GetConstructor(new [] { typeof(string), typeof(string)});
      Mock.Get(ctorSelector)
          .Setup(x => x.SelectConstructor(type))
          .Returns(ctor);

      // Act
      var result = sut.GetFactoryAdapter(request);

      // Assert
      Assert.That(result, Is.Not.Null);
      Assert.That(result, Is.InstanceOf<ConstructorFactory>());
      var ctorFactory = (ConstructorFactory) result;
      Assert.That(ctorFactory.Constructor, Is.SameAs(ctor));
      Mock.Get(ctorSelector)
          .Verify(x => x.SelectConstructor(type), Times.Once);
    }
  }
}
