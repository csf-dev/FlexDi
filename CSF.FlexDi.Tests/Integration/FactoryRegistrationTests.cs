﻿//
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
using CSF.FlexDi.Tests.Autofixture;
using CSF.FlexDi.Tests.Stubs;
using NUnit.Framework;

namespace CSF.FlexDi.Tests.Integration
{
  [TestFixture,Parallelizable(ParallelScope.All)]
  public class FactoryRegistrationTests
  {
    [Test,AutoMoqData]
    public void Should_be_possible_to_resolve_objects_registered_using_dynamic_factory_registrations([Container] IContainer container,
                                                                                                     string arbitraryString)
    {
      // Arrange
      container.AddRegistrations(x => {
        x.RegisterFactory(() => new ChildServiceOne { AProperty = arbitraryString });
        x.RegisterDynamicFactory(c => new ParentService(c.Resolve<ChildServiceOne>(), null));
      });

      // Act
      var result = container.Resolve<ParentService>();

      // Assert
      Assert.That(result.ChildOne.AProperty, Is.EqualTo(arbitraryString));
    }

    [Test,AutoMoqData]
    public void Should_raise_exception_for_circular_dependencies_in_dynamic_factory_resolutions([Container(ResolveUnregisteredTypes = true)] IContainer container)
    {
      // Arrange
      container.AddRegistrations(x => {
        x.RegisterType<ChildServiceWithCircularDependency>().As<ChildServiceTwo>();
        x.RegisterDynamicFactory(c => new ParentService(c.Resolve<ChildServiceOne>(), c.Resolve<ChildServiceTwo>()));
      });

      // Act & assert
      Assert.That(() => container.Resolve<ParentService>(), Throws.InstanceOf<CircularDependencyException>());
    }
  }
}
