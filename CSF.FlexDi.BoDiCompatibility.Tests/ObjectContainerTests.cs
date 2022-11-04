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
using System.Collections.Generic;
using System.Linq;
using CSF.FlexDi.BoDiCompatibility.Tests.Autofixture;
using CSF.FlexDi.Builders;
using CSF.FlexDi.Registration;
using Moq;
using NUnit.Framework;

namespace CSF.FlexDi.BoDiCompatibility.Tests
{
    [TestFixture,Parallelizable(ParallelScope.All)]
    public class ObjectContainerTests
    {
        [Test,AutoMoqData]
        [Description("BoDi (NuGet v1.3.0) caches objects resolved from factory registrations. To provide compatibility we must mimic that behaviour. See https://github.com/csf-dev/FlexDi/issues/18")]
        public void RegisterFactoryAs_marks_the_registration_as_cacheable(IContainer innerContainer)
        {
            var sut = new ObjectContainerWithInnerContainerReplacementSupport();
            sut.ReplaceInnerContainer(innerContainer);

            sut.RegisterFactoryAs<ISampleService>(() => new SampleServiceImplementationOne());

            Mock.Get(innerContainer).Verify(x => x.AddRegistrations(It.Is<IEnumerable<IServiceRegistration>>(r => r.Count() == 1 && r.Single().Cacheable)), Times.Once);
        }
    }
}
