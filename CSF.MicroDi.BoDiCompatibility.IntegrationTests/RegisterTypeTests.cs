//
//    RegisterTypeTests.cs
//
//    Copyright 2015  Gáspár Nagy et al
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
using NUnit.Framework;

namespace BoDi.Tests
{
    [TestFixture]
    public class RegisterTypeTests
    {
        [Test]
        public void ShouldRegisterTypeDynamically()
        {
            // given
            var container = new ObjectContainer();
            container.RegisterTypeAs<IInterface1>(typeof(VerySimpleClass));

            // when

            var obj = container.Resolve<IInterface1>();

            // then

            Assert.IsNotNull(obj);
            Assert.IsInstanceOf(typeof(VerySimpleClass), obj);
        }

        [Test]
        public void ShouldAllowOverrideRegistrationBeforeResolve()
        {
            // given

            var container = new ObjectContainer();
            container.RegisterTypeAs<VerySimpleClass, IInterface1>();

            // when 

            container.RegisterTypeAs<SimpleClassWithDefaultCtor, IInterface1>();

            // then

            var obj = container.Resolve<IInterface1>();
            Assert.IsNotNull(obj);
            Assert.IsInstanceOf(typeof(SimpleClassWithDefaultCtor), obj);
        }

        [Test]
        public void ShouldAllowOverrideInstanceRegistrationBeforeResolve()
        {
            // given

            var container = new ObjectContainer();
            container.RegisterInstanceAs<IInterface1>(new VerySimpleClass());

            // when 

            container.RegisterTypeAs<SimpleClassWithDefaultCtor, IInterface1>();

            // then

            var obj = container.Resolve<IInterface1>();
            Assert.IsNotNull(obj);
            Assert.IsInstanceOf(typeof(SimpleClassWithDefaultCtor), obj);
        }

        [Test, ExpectedException(typeof(ObjectContainerException))]
        public void ShouldNotAllowOverrideRegistrationAfterResolve()
        {
            // given

            var container = new ObjectContainer();
            container.RegisterTypeAs<VerySimpleClass, IInterface1>();
            container.Resolve<IInterface1>();

            // when 

            container.RegisterTypeAs<SimpleClassWithDefaultCtor, IInterface1>();
        }

        [Test, ExpectedException(typeof(ObjectContainerException))]
        public void ShouldNotAllowOverrideInstanceRegistrationAfterResolve()
        {
            // given

            var container = new ObjectContainer();
            container.RegisterInstanceAs<IInterface1>(new VerySimpleClass());
            container.Resolve<IInterface1>();

            // when 

            container.RegisterTypeAs<SimpleClassWithDefaultCtor, IInterface1>();
        }

        [Test]
        public void ShouldRegisterGenericTypeDefinitions()
        {
            // given
            var container = new ObjectContainer();
            container.RegisterTypeAs(typeof(SimpleGenericClass<>), typeof(IGenericInterface<>));

            // when
            var obj = container.Resolve<IGenericInterface<VerySimpleClass>>();

            // then

            Assert.IsNotNull(obj);
            Assert.IsInstanceOf(typeof(SimpleGenericClass<VerySimpleClass>), obj);
        }

        [Test]
        public void ShouldNotRegisterInvalidTypeMapping()
        {
            // given
            var container = new ObjectContainer();

            // then
            Assert.Catch<InvalidOperationException>(() => container.RegisterTypeAs(typeof(SimpleClassExtendingGenericInterface), typeof(IGenericInterface<>)));
        }
    }
}