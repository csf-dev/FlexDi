﻿//
//    NamedRegistrationTests.cs
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
        [ExpectedException(typeof(ObjectContainerException))]
        public void NamedRegistrationShouldNotInflucenceNormalRegistrations()
        {
            var container = new ObjectContainer();
            container.RegisterTypeAs<VerySimpleClass, IInterface1>("a_name");

            // when

            container.Resolve<IInterface1>();
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

        [Test, ExpectedException(typeof(ObjectContainerException))]
        public void ShouldNotBeAbleToResolveNamedInstancesDictionaryOtherThanStringAndEnumKey()
        {
            var container = new ObjectContainer();

            // when

            var instanceDict = container.Resolve<IDictionary<int, IInterface1>>();
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


    }
}
