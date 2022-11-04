//
//    DefaultContainerAttribute.cs
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
using System.Reflection;
using AutoFixture;
using AutoFixture.NUnit3;

namespace CSF.FlexDi.Tests.Autofixture
{
    public class ContainerAttribute : CustomizeAttribute
    {
        public bool ResolveUnregisteredTypes { get; set; }

        public override ICustomization GetCustomization(ParameterInfo parameter) => new ContainerCustomization(GetContainer());

        IContainer GetContainer()
            => Container.CreateBuilder().ResolveUnregisteredTypes(ResolveUnregisteredTypes).Build();

        public ContainerAttribute()
        {
            ResolveUnregisteredTypes = ContainerOptions.Default.ResolveUnregisteredTypes;
        }

        class ContainerCustomization : ICustomization
        {
            readonly IContainer container;

            public void Customize(IFixture fixture) => fixture.Inject(container);

            public ContainerCustomization(IContainer container)
            {
                this.container = container ?? throw new ArgumentNullException(nameof(container));
            }
        }
    }
}
