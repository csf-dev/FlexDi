﻿//
//    SubContainerTests.cs
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;

namespace BoDi.Tests
{
    [TestFixture]
    public class SubContainerTests
    {
        [Test]
        public void ShouldBeAbleToResolveFromBaseContainer()
        {
            // given
            var baseContainer = new ObjectContainer();
            baseContainer.RegisterTypeAs<VerySimpleClass, IInterface1>();
            var container = new ObjectContainer(baseContainer);

            // when

            var obj = container.Resolve<IInterface1>();

            // then

            Assert.IsNotNull(obj);
            Assert.IsInstanceOf(typeof(VerySimpleClass), obj);
        }

        [Test]
        public void ShouldBeAbleToResolveFromChildContainer()
        {
            // given
            var baseContainer = new ObjectContainer();
            var container = new ObjectContainer(baseContainer);
            container.RegisterTypeAs<VerySimpleClass, IInterface1>();

            // when

            var obj = container.Resolve<IInterface1>();

            // then

            Assert.IsNotNull(obj);
            Assert.IsInstanceOf(typeof(VerySimpleClass), obj);
        }

        [Test]
        public void ShouldResolveFromBaseContainer()
        {
            // given
            var baseContainer = new ObjectContainer();
            baseContainer.RegisterTypeAs<VerySimpleClass, IInterface1>();
            var container = new ObjectContainer(baseContainer);

            // when
            var objFromChild = container.Resolve<IInterface1>();
            var objFromBase = baseContainer.Resolve<IInterface1>();

            // then
            Assert.AreSame(objFromChild, objFromBase);
        }

        [Test]
        public void ShouldBeAbleToOverrideBaseContainerRegistration()
        {
            // given
            var baseContainer = new ObjectContainer();
            baseContainer.RegisterTypeAs<SimpleClassWithDefaultCtor, IInterface1>();
            var container = new ObjectContainer(baseContainer);
            container.RegisterTypeAs<VerySimpleClass, IInterface1>();

            // when

            var obj = container.Resolve<IInterface1>();
            var baseObj = baseContainer.Resolve<IInterface1>();

            // then

            Assert.IsNotNull(obj);
            Assert.IsInstanceOf(typeof(VerySimpleClass), obj);
            Assert.AreNotEqual(obj, baseObj);
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void BaseContainerMustBeAnObjectContainer()
        {
            var otherContainer = new Mock<IObjectContainer>();

            var container = new ObjectContainer(otherContainer.Object);
        }

        [Test]
        public void ParentObjectPoolShouldNotBeConsideredWhenReRegisteredInChild()
        {
            // given
            var baseContainer = new ObjectContainer();
            baseContainer.RegisterTypeAs<VerySimpleClass, IInterface1>();
            var container = new ObjectContainer(baseContainer);
            container.RegisterTypeAs<VerySimpleClass, IInterface1>();

            // when
            var objFromBase = baseContainer.Resolve<IInterface1>();
            var objFromChild = container.Resolve<IInterface1>();

            // then
            Assert.AreNotSame(objFromChild, objFromBase);
        }



        public interface IParentInterface
        {
            IInterface1 Interface1 { get; }
        }

        private class ParentClass : IParentInterface
        {
            public IInterface1 Interface1 { get; set; }
            public ParentClass(IInterface1 interface1)
            {
                this.Interface1 = interface1;
            }
        }

        public interface IChildInterface
        {
            IInterface1 Interface1 { get; }
        }

        private class ChildClass : IChildInterface
        {
            public IInterface1 Interface1 { get; set; }
            public ChildClass(IInterface1 interface1)
            {
                this.Interface1 = interface1;
            }
        }

        private class DelegatingInterfce1 : IInterface1
        {
            public DelegatingInterfce1(IParentInterface parentInterface)
            {
                
            }
        }

        [Test]
        public void ShouldNotDetectCircularDependencyForOverriddenObjectRegistrations()
        {
            // given
            var baseContainer = new ObjectContainer();
            baseContainer.RegisterTypeAs<VerySimpleClass, IInterface1>();
            baseContainer.RegisterTypeAs<ParentClass, IParentInterface>();
            var container = new ObjectContainer(baseContainer);
            container.RegisterTypeAs<DelegatingInterfce1, IInterface1>();
            container.RegisterTypeAs<ChildClass, IChildInterface>();

            // when
            var objFromChild = container.Resolve<IChildInterface>();
            var objFromParent = container.Resolve<IParentInterface>();

            // then
            Assert.AreNotSame(objFromChild.Interface1, objFromParent.Interface1);
        }
    }
}
