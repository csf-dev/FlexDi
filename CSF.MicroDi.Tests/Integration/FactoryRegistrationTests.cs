//
//    FactoryRegistrationTests.cs
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
using CSF.MicroDi.Tests.Autofixture;
using CSF.MicroDi.Tests.Stubs;
using NUnit.Framework;

namespace CSF.MicroDi.Tests.Integration
{
  [TestFixture,Parallelizable(ParallelScope.All)]
  public class FactoryRegistrationTests
  {
    [Test,AutoMoqData]
    public void Should_be_possible_to_resolve_objects_registered_using_dynamic_factory_registrations([DefaultContainer] IContainer container,
                                                                                                     string arbitraryString)
    {
      // Arrange
      container.AddRegistrations(x => {
        x.RegisterFactory(() => new ChildServiceOne { AProperty = arbitraryString });
        x.RegisterFactory(c => new ParentService(c.Resolve<ChildServiceOne>(), null));
      });

      // Act
      var result = container.Resolve<ParentService>();

      // Assert
      Assert.That(result.ChildOne.AProperty, Is.EqualTo(arbitraryString));
    }
  }
}
