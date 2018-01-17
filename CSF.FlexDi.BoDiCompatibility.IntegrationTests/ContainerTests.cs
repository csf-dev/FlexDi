//
//    ContainerTests.cs
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
using System.Threading.Tasks;
using NUnit.Framework;
using Should;

namespace BoDi.Tests
{
    [TestFixture]
    public class ContainerTests
    {
        [Test]
        public void ShouldListRegistrationsInToString()
        {
            // given
            var container = new ObjectContainer();
            container.RegisterTypeAs<VerySimpleClass, IInterface1>();
            container.RegisterInstanceAs<IInterface1>(new SimpleClassWithDefaultCtor { Status = "instance1" });
            container.RegisterTypeAs<VerySimpleClass, IInterface1>("one");
            container.RegisterInstanceAs<IInterface1>(new SimpleClassWithDefaultCtor { Status = "instance2" }, "two");
            container.RegisterInstanceAs<IInterface1>(new SimpleClassWithFailingToString(), "three");

            // when
            var result = container.ToString();
            // Console.WriteLine(result);

            // then 
            result.ShouldContain("BoDi.IObjectContainer -> <self>");
            result.ShouldContain("BoDi.Tests.IInterface1 -> Instance: SimpleClassWithDefaultCtor: instance1");
            result.ShouldContain("BoDi.Tests.IInterface1('one') -> Type: BoDi.Tests.VerySimpleClass");
            result.ShouldContain("BoDi.Tests.IInterface1('two') -> Instance: SimpleClassWithDefaultCtor: instance2");
            result.ShouldContain("BoDi.Tests.IInterface1('three') -> Instance: simulated error");
        }
    }
}
