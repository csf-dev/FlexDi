//
//    ResolveAllTests.cs
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

namespace BoDi.Tests
{

    public interface IFancy{}
    public class ImFancy : IFancy {}
    public class ImFancier : IFancy {}
    public class ImFanciest : IFancy {}

    [TestFixture]
    public class ResolveAllTests
    {

        [Test]
        public void ShouldResolveTheRightNumberOfRegisteredTypes()
        {
            // given
            var container = new ObjectContainer();
            container.RegisterTypeAs<ImFancy, IFancy>("fancy");
            container.RegisterTypeAs<ImFancier, IFancy>("fancier");
            container.RegisterTypeAs<ImFanciest, IFancy>("fanciest");

            // when
            var results = container.ResolveAll<IFancy>();

            // then
            Assert.AreEqual(3, results.Count());
        }

        [Test]
        public void ShouldResolveTheRightTypes()
        {
            // given
            var container = new ObjectContainer();
            container.RegisterTypeAs<ImFancy, IFancy>("fancy");
            container.RegisterTypeAs<ImFancier, IFancy>("fancier");

            // when
            var results = container.ResolveAll<IFancy>();

            // then
            Assert.IsTrue(results.Contains(container.Resolve<IFancy>("fancy")));
            Assert.IsTrue(results.Contains(container.Resolve<IFancy>("fancier")));
        }

    }

}