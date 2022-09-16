//
//    InstanceRegistrationTests.cs
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
using NUnit.Framework;

namespace CSF.FlexDi.Tests.Registration
{
    [TestFixture,Parallelizable(ParallelScope.All)]
    public class InstanceRegistrationTests : ServiceRegistrationTestBase
    {
        [Test]
        public void AssertIsValid_should_throw_if_Cacheable_is_set_to_false()
        {
            var sut = GetValidServiceRegistration();
            sut.Cacheable = false;

            Assert.That(() => sut.AssertIsValid(), Throws.InstanceOf<InvalidRegistrationException>());
        }
        
        [Test]
        public void AssertIsValid_should_not_throw_if_Cacheable_is_set_to_true()
        {
            var sut = GetValidServiceRegistration();
            sut.Cacheable = true;

            Assert.That(() => sut.AssertIsValid(), Throws.Nothing);
        }

        protected override ServiceRegistration GetValidServiceRegistration() => new InstanceRegistration(new object());
    }
}
