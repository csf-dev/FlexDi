﻿//
//    ContainerTests.cs
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
using CSF.FlexDi.Tests.Autofixture;
using Moq;
using NUnit.Framework;

namespace CSF.FlexDi.Tests
{
    [TestFixture,Parallelizable(ParallelScope.All)]
    public class ContainerTests
    {
        [Test,AutoMoqData]
        public void Constructor_should_self_register_a_resolver_when_options_indicate_to_do_so(ICreatesRegistry registryFactory, IRegistersServices registry)
        {
            IServiceRegistration registration = null;
            Mock.Get(registry)
                    .Setup(x => x.Add(It.IsAny<IServiceRegistration>()))
                    .Callback((IServiceRegistration reg) => {
                        if(reg.ServiceType == typeof(IResolvesServices))
                            registration = reg;
                    });
            Mock.Get(registryFactory).Setup(x => x.GetRegistry()).Returns(registry);

            var container = Container.CreateBuilder().SelfRegisterAResolver().UseCustomRegistryFactory(registryFactory).Build();

            Mock.Get(registry).Verify(x => x.Add(It.Is<IServiceRegistration>(r => r.ServiceType == typeof(IResolvesServices))), Times.Once);
            Assert.That(registration, Is.Not.Null, "Registration must not be null");
            Assert.That(registration, Is.InstanceOf<InstanceRegistration>(), "Registration is an instance registration");
            var instanceReg = (InstanceRegistration) registration;
            Assert.That(instanceReg.Implementation, Is.SameAs(container), "The self-registered resolver is the container itself");
        }

        [Test,AutoMoqData]
        public void Constructor_should_not_self_register_a_resolver_when_options_indicate_not_to_do_so(ICreatesRegistry registryFactory, IRegistersServices registry)
        {
            Mock.Get(registryFactory).Setup(x => x.GetRegistry()).Returns(registry);
            var container = Container.CreateBuilder().DoNotSelfRegisterAResolver().UseCustomRegistryFactory(registryFactory).Build();

            Mock.Get(registry).Verify(x => x.Add(It.Is<IServiceRegistration>(r => r.ServiceType == typeof(IResolvesServices))), Times.Never);
        }

        [Test,AutoMoqData]
        public void Constructor_should_self_register_the_registry_when_options_indicate_to_do_so(ICreatesRegistry registryFactory, IRegistersServices registry)
        {
            IServiceRegistration registration = null;
            Mock.Get(registry)
                    .Setup(x => x.Add(It.IsAny<IServiceRegistration>()))
                    .Callback((IServiceRegistration reg) => {
                if(reg.ServiceType == typeof(IReceivesRegistrations))
                    registration = reg;
            });
            Mock.Get(registryFactory).Setup(x => x.GetRegistry()).Returns(registry);

            var container = Container.CreateBuilder().SelfRegisterTheRegistry().UseCustomRegistryFactory(registryFactory).Build();

            Mock.Get(registry).Verify(x => x.Add(It.Is<IServiceRegistration>(r => r.ServiceType == typeof(IReceivesRegistrations))), Times.Once);
            Assert.That(registration, Is.Not.Null, "Registration must not be null");
            Assert.That(registration, Is.InstanceOf<InstanceRegistration>(), "Registration is an instance registration");
            var instanceReg = (InstanceRegistration) registration;
            Assert.That(instanceReg.Implementation, Is.SameAs(container), "The self-registered resolver is the container itself");
        }

        [Test,AutoMoqData]
        public void Constructor_should_not_self_register_the_registry_when_options_indicate_not_to_do_so(ICreatesRegistry registryFactory, IRegistersServices registry)
        {
            Mock.Get(registryFactory).Setup(x => x.GetRegistry()).Returns(registry);
            var container = Container.CreateBuilder().DoNotSelfRegisterTheRegistry().UseCustomRegistryFactory(registryFactory).Build();

            Mock.Get(registry).Verify(x => x.Add(It.Is<IServiceRegistration>(r => r.ServiceType == typeof(IReceivesRegistrations))), Times.Never);
        }
    }
}
