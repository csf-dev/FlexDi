//
//    LazyInstanceResolutionTests.cs
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
using CSF.FlexDi.Tests.Stubs;
using NUnit.Framework;

namespace CSF.FlexDi.Tests.Integration
{
  [TestFixture,Parallelizable(ParallelScope.All)]
  public class LazyInstanceResolutionTests
  {
    [Test]
    public void Can_resolve_an_instance_lazily()
    {
      // Arrange
      var hasBeenResolved = false;

      var container = Container.CreateBuilder().SupportResolvingLazyInstances().BuildContainer();
      container.AddRegistrations(r => {
        r.RegisterFactory(() => {
            var output = new SampleServiceImplementationOne();
            hasBeenResolved = true;
            return output;
          })
          .As<ISampleService>();
      });

      // Act
      var result = container.Resolve<Lazy<ISampleService>>();

      // Assert
      Assert.That(hasBeenResolved, Is.False);
      Assert.That(result.Value, Is.InstanceOf<SampleServiceImplementationOne>());
      Assert.That(hasBeenResolved, Is.True);
    }
  }
}
