//
//    CacheInspectionTests.cs
//
//    Copyright 2020  Craig Fowler
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
using CSF.FlexDi.Tests.Autofixture;
using CSF.FlexDi.Tests.Stubs;
using NUnit.Framework;

namespace CSF.FlexDi.Tests.Integration
{
    [TestFixture, Parallelizable(ParallelScope.All)]
    public class CacheInspectionTests
    {
        [Test, AutoMoqData]
        public void IsResolvedInstanceCached_returns_false_if_instance_has_not_been_resolved_before([Container(ResolveUnregisteredTypes = true)] IContainer container)
        {
            Assert.That(() => container.IsResolvedInstanceCached(typeof(SampleServiceImplementationOne)), Is.False);
        }

        [Test, AutoMoqData]
        public void IsResolvedInstanceCached_returns_true_if_instance_has_been_resolved_before([Container(ResolveUnregisteredTypes = true)] IContainer container)
        {
            container.Resolve<SampleServiceImplementationOne>();
            Assert.That(() => container.IsResolvedInstanceCached(typeof(SampleServiceImplementationOne)), Is.True);
        }

        [Test, AutoMoqData]
        public void IsResolvedInstanceCached_returns_true_if_instance_has_been_resolved_before_with_same_name([Container(ResolveUnregisteredTypes = true)] IContainer container)
        {
            container.Resolve<SampleServiceImplementationOne>("MyName");
            Assert.That(() => container.IsResolvedInstanceCached(typeof(SampleServiceImplementationOne), "MyName"), Is.True);
        }

        [Test, AutoMoqData]
        public void IsResolvedInstanceCached_returns_false_if_instance_has_been_resolved_before_with_different_name([Container(ResolveUnregisteredTypes = true)] IContainer container)
        {
            container.Resolve<SampleServiceImplementationOne>("MyName");
            Assert.That(() => container.IsResolvedInstanceCached(typeof(SampleServiceImplementationOne), "OtherName"), Is.False);
        }

        [Test, AutoMoqData]
        public void IsResolvedInstanceCached_returns_false_if_instance_has_been_resolved_before_without_a_name([Container(ResolveUnregisteredTypes = true)] IContainer container)
        {
            container.Resolve<SampleServiceImplementationOne>();
            Assert.That(() => container.IsResolvedInstanceCached(typeof(SampleServiceImplementationOne), "MyName"), Is.False);
        }

        [Test, AutoMoqData]
        public void IsResolvedInstanceCached_returns_true_if_instance_has_been_resolved_before_with_a_name_but_query_is_for_no_name([Container(ResolveUnregisteredTypes = true)] IContainer container)
        {
            container.Resolve<SampleServiceImplementationOne>("MyName");
            Assert.That(() => container.IsResolvedInstanceCached(typeof(SampleServiceImplementationOne)), Is.True);
        }
    }
}
