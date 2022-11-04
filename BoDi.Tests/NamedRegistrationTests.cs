﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Should;

namespace BoDi.Tests
{
    [TestFixture]
    public class NamedRegistrationTests
    {
        [Test]
        public void ShouldBeAbleToRegisterTypeWithName()
        {
            var container = new ObjectContainer();
            container.RegisterTypeAs<VerySimpleClass, IInterface1>("a_name");
        }

        [Test]
        public void ShouldBeAbleToRegisterTypeWithNameDynamically()
        {
            var container = new ObjectContainer();
            container.RegisterTypeAs<IInterface1>(typeof(VerySimpleClass), "a_name");
        }

/*
        [Test]
        public void SingleNamedRegistrationShouldBehaveLikeWithoutName()
        {
            var container = new ObjectContainer();
            container.RegisterTypeAs<VerySimpleClass, IInterface1>("a_name");

            // when

            var obj = container.Resolve<IInterface1>();

            // then

            obj.ShouldBeType<VerySimpleClass>();
        }

*/
        [Test]
        public void NamedRegistrationShouldNotInflucenceNormalRegistrations()
        {
            var container = new ObjectContainer();
            container.RegisterTypeAs<VerySimpleClass, IInterface1>("a_name");

            // when
            Assert.Throws<ObjectContainerException>(() => container.Resolve<IInterface1>());

        }


        [Test]
        public void ShouldBeAbleToResolveWithName()
        {
            var container = new ObjectContainer();
            container.RegisterTypeAs<VerySimpleClass, IInterface1>("a_name");

            // when

            var obj = container.Resolve<IInterface1>("a_name");

            // then

            obj.ShouldBeType<VerySimpleClass>();
        }

        [Test]
        public void ShouldNotReuseObjectsWithTheSameTypeButResolvedWithDifferentName()
        {
            var container = new ObjectContainer();
            container.RegisterTypeAs<VerySimpleClass, IInterface1>("a_name");
            container.RegisterTypeAs<VerySimpleClass, IInterface1>("another_name");

            // when

            var obj = container.Resolve<IInterface1>("a_name");
            var otherObj = container.Resolve<IInterface1>("another_name");

            // then

            obj.ShouldNotBeSameAs(otherObj);
        }

        [Test]
        public void ShouldBeAbleToRegisterMultipleTypesWithDifferentNames()
        {
            var container = new ObjectContainer();
            container.RegisterTypeAs<VerySimpleClass, IInterface1>("one");
            container.RegisterTypeAs<SimpleClassWithDefaultCtor, IInterface1>("two");

            // when

            var oneObj = container.Resolve<IInterface1>("one");
            var twoObj = container.Resolve<IInterface1>("two");

            // then

            oneObj.ShouldNotBeSameAs(twoObj);
            oneObj.ShouldBeType<VerySimpleClass>();
            twoObj.ShouldBeType<SimpleClassWithDefaultCtor>();
        }

        [Test]
        public void ShouldBeAbleToResolveNamedInstancesAsDictionary()
        {
            var container = new ObjectContainer();
            container.RegisterTypeAs<VerySimpleClass, IInterface1>("one");
            container.RegisterTypeAs<SimpleClassWithDefaultCtor, IInterface1>("two");

            // when

            var instanceDict = container.Resolve<IDictionary<string, IInterface1>>();

            // then

            instanceDict.Keys.ShouldContain("one");
            instanceDict.Keys.ShouldContain("two");
            instanceDict["one"].ShouldBeType<VerySimpleClass>();
            instanceDict["two"].ShouldBeType<SimpleClassWithDefaultCtor>();
        }

        [Test]
        public void ShouldBeAbleToResolveNamedInstancesAsDictionaryEvenIfThereWasNoRegistrations()
        {
            var container = new ObjectContainer();

            // when

            var instanceDict = container.Resolve<IDictionary<string, IInterface1>>();

            // then

            instanceDict.Count.ShouldEqual(0);
        }

        [Test]
        public void ShouldBeAbleToResolveNamedInstancesAsEnumKeyDictionary()
        {
            var container = new ObjectContainer();
            container.RegisterTypeAs<VerySimpleClass, IInterface1>("one");
            container.RegisterTypeAs<SimpleClassWithDefaultCtor, IInterface1>("two");

            // when

            var instanceDict = container.Resolve<IDictionary<MyEnumKey, IInterface1>>();

            // then

            instanceDict.Keys.ShouldContain(MyEnumKey.One);
            instanceDict.Keys.ShouldContain(MyEnumKey.Two);
            instanceDict[MyEnumKey.One].ShouldBeType<VerySimpleClass>();
            instanceDict[MyEnumKey.Two].ShouldBeType<SimpleClassWithDefaultCtor>();
        }

        [Test]
        public void ShouldNotBeAbleToResolveNamedInstancesDictionaryOtherThanStringAndEnumKey()
        {
            var container = new ObjectContainer();

            // when

            Assert.Throws<ObjectContainerException>(() => container.Resolve<IDictionary<int, IInterface1>>());
        }

        [Test]
        public void ShouldBeAbleToInjectResolvedName()
        {
            var container = new ObjectContainer();
            container.RegisterTypeAs<SimpleClassWithRegisteredNameDependency, IInterface1>("a_name");

            // when

            var obj = container.Resolve<IInterface1>("a_name");

            // then

            obj.ShouldBeType<SimpleClassWithRegisteredNameDependency>();
            ((SimpleClassWithRegisteredNameDependency)obj).RegisteredName.ShouldEqual("a_name");
        }

        [Test]
        public void ShouldBeAbleToResolveNamedInstancesWhenDependingOnOther()
        {
            var container = new ObjectContainer();
            container.RegisterTypeAs<Interface1DependingOnAnotherImplementation, IInterface1>("two");
            container.RegisterTypeAs<VerySimpleClass, IInterface1>("one");

            // when
            container.Resolve<IDictionary<string, IInterface1>>();

            // then
            //should not throw Collection was modified; error
        }
    }
}
