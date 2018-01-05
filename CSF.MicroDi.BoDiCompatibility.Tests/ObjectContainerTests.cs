//
//    ObjectContainerTests.cs
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
using System.Linq;
using BoDi;
using CSF.MicroDi.BoDiCompatibility.Tests.Autofixture;
using CSF.MicroDi.Builders;
using Moq;
using NUnit.Framework;

namespace CSF.MicroDi.BoDiCompatibility.Tests
{
  [TestFixture,Parallelizable(ParallelScope.All)]
  public class ObjectContainerTests
  {
    [Test,AutoMoqData]
    [Description("BoDi (NuGet v1.3.0) caches objects resolved from factory registrations. To provide compatibility we must mimic that behaviour. See https://github.com/csf-dev/MicroDi/issues/18")]
    public void RegisterFactoryAs_marks_the_registration_as_cacheable(IContainer innerContainer,
                                                                      RegistrationHelper registrationHelper)
    {
      // Arrange
      var sut = new ObjectContainerWithInnerContainerReplacementSupport();
      sut.ReplaceInnerContainer(innerContainer);

      Mock.Get(innerContainer)
          .Setup(x => x.AddRegistrations(It.IsAny<Action<IRegistrationHelper>>()))
          .Callback((Action<IRegistrationHelper> action) => action(registrationHelper));

      // Act
      sut.RegisterFactoryAs<ISampleService>(() => new SampleServiceImplementationOne());

      // Assert
      var registration = registrationHelper.GetRegistrations().Single();
      Assert.That(registration.Cacheable, Is.True);
    }
  }
}
