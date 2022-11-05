//
//    InstanceRegistration.cs
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
    /// Implementation of an instance registration, which always provides a provided object instance as the result
    /// of resolution.
    /// </summary>
    public class InstanceRegistration : TypedRegistration
    {
        /// <summary>
        /// Gets the implementation/instance of the component which will be used to fulfil resolution.
        /// </summary>
        /// <value>The implementation.</value>
        public object Implementation { get; }

        /// <inheritdoc/>
        public override IFactoryAdapter GetFactoryAdapter(ResolutionRequest request) => new InstanceFactory(Implementation);

        /// <inheritdoc/>
        public override void AssertIsValid()
        {
            base.AssertIsValid();
            if(!Cacheable) throw new InvalidRegistrationException($"{nameof(Cacheable)} must not be {Boolean.FalseString}.");
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="InstanceRegistration"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="InstanceRegistration"/>.</returns>
        public override string ToString()
            => $"[{nameof(InstanceRegistration)} for `{ServiceType.FullName}', using an instance of `{ImplementationType.FullName}']";

        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceRegistration"/> class.
        /// </summary>
        /// <param name="implementation">The component instance/implementation.</param>
        public InstanceRegistration(object implementation) : base(implementation?.GetType(), priority: 2, cacheable: true, disposeWithContainer: false)
        {
            Implementation = implementation ?? throw new ArgumentNullException(nameof(implementation));
        }
    }
}
