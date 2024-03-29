﻿//
//    BoDiFlexDiContainerFactory.cs
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
using CSF.FlexDi;

namespace BoDi.Internal
{
    /// <summary>
    /// A factory service which creates a FlexDi <see cref="IContainer"/> with functionality which mimics that of
    /// BoDi's.
    /// </summary>
    static class BoDiFlexDiContainerFactory
    {
        /// <summary>
        /// Gets the container instance.
        /// </summary>
        /// <returns>The container.</returns>
        internal static IContainer GetContainer()
        {
            return Container
                .CreateBuilder()
                .UseNonPublicConstructors()
                .ResolveUnregisteredTypes()
                .ThrowOnCircularDependencies()
                .UseInstanceCache()
                .SupportResolvingNamedInstanceDictionaries()
                .UseCustomResolverFactory(new BoDiResolverFactory())
                .DoNotSelfRegisterAResolver()
                .DoNotSelfRegisterTheRegistry()
                .DoNotSupportResolvingLazyInstances()
                .BuildContainer();
        }
    }
}
