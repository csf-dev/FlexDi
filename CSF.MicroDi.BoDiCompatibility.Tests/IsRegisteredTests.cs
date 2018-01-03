//
//    IsRegisteredTests.cs
//
//    Copyright 2017  Gáspár Nagy et al
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

using NUnit.Framework;

namespace BoDi.Tests
{
    [TestFixture]
    public class IsRegisteredTests
    {
        [Test]
        public void ShouldReturnFalseIfInterfaceNotRegistered()
        {
            // given

            var container = new ObjectContainer();

            // then

            bool isRegistered = container.IsRegistered<IInterface1>();

            Assert.IsFalse(isRegistered);
        }

        [Test]
        public void ShouldReturnFalseIfTypeNotRegistered()
        {
            // given

            var container = new ObjectContainer();

            // then

            bool isRegistered = container.IsRegistered<VerySimpleClass>();

            Assert.IsFalse(isRegistered);
        }

        [Test]
        public void ShouldReturnTrueIfInterfaceRegistered()
        {
            // given

            var container = new ObjectContainer();

            // when 

            container.RegisterTypeAs<VerySimpleClass, IInterface1>();

            // then

            bool isRegistered = container.IsRegistered<IInterface1>();
            
            Assert.IsTrue(isRegistered);
        }

        [Test]
        public void ShouldReturnTrueIfTypeRegistered()
        {
            // given

            var container = new ObjectContainer();

            // when 

            container.RegisterInstanceAs(new SimpleClassWithDefaultCtor());

            // then

            bool isRegistered = container.IsRegistered<SimpleClassWithDefaultCtor>();

            Assert.IsTrue(isRegistered);
        }
    }
}
