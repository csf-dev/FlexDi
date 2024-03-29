﻿//
//    TypeRegistration.cs
//
//    Copyright 2018  Craig Fowler et al
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
using CSF.FlexDi.Resolution;

namespace CSF.FlexDi.Registration
{
    /// <summary>
    /// Implementation of a service/component registration which instantiates an instance of a given type in order
    /// to fulfil the resolution.
    /// </summary>
    public class TypeRegistration : TypedRegistration
    {
        /// <summary>
        /// Gets a constructor-selecting service implementation.
        /// </summary>
        /// <value>The constructor selector.</value>
        public ISelectsConstructor ConstructorSelector { get; }

        /// <summary>
        /// Gets a factory adapter instance, for the current registration, from a specified resolution request.
        /// </summary>
        /// <returns>The factory adapter.</returns>
        /// <param name="request">A resolution request.</param>
        public override IFactoryAdapter GetFactoryAdapter(ResolutionRequest request)
            => GetFactoryAdapter(ImplementationType);

        /// <summary>
        /// Gets a factory adapter for the given <c>System.Type</c>
        /// </summary>
        /// <returns>The factory adapter.</returns>
        /// <param name="type">The implementation type.</param>
        protected virtual IFactoryAdapter GetFactoryAdapter(Type type)
            => new ConstructorFactory(ConstructorSelector.SelectConstructor(type));

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="TypeRegistration"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="TypeRegistration"/>.</returns>
        public override string ToString()
        {
            return $"[{nameof(TypeRegistration)} for `{ServiceType.FullName}', using  `{ImplementationType.FullName}']";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeRegistration"/> class.
        /// </summary>
        /// <param name="implementationType">Implementation type.</param>
        /// <param name="constructorSelector">Constructor selector.</param>
        public TypeRegistration(Type implementationType, ISelectsConstructor constructorSelector) : base(implementationType)
        {
            ConstructorSelector = constructorSelector ?? throw new ArgumentNullException(nameof(constructorSelector));
        }
    }
}
