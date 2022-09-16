//
//    TypedRegistration.cs
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
using System.Reflection;

namespace CSF.FlexDi.Registration
{
    /// <summary>
    /// Base type for registrations in which the implementation type of the component is known in advance.
    /// </summary>
    public abstract class TypedRegistration : ServiceRegistration
    {
        /// <summary>
        /// Gets the type of the concrete component implementation.  This should be either the same as the
        /// <see cref="ServiceType"/> or a derived type.
        /// </summary>
        /// <value>The implementation type.</value>
        public virtual Type ImplementationType { get; }

        /// <summary>
        /// Gets the <c>System.Type</c> which will be fulfilled by this registration.
        /// </summary>
        /// <remarks>
        /// <para>
        /// In this implementation of <see cref="ServiceRegistration"/>, if this property has not been set then
        /// it will return <see cref="ImplementationType"/> instead, as a fallback value.
        /// </para>
        /// </remarks>
        /// <value>The service/component type.</value>
        public override Type ServiceType
        {
            get => base.ServiceType ?? ImplementationType;
            set => base.ServiceType = value;
        }

        /// <inheritdoc/>
        public override bool MatchesKey(ServiceRegistrationKey key)
        {
            if(base.MatchesKey(key)) return true;
            if(key == null) return false;

            return key.ServiceType.GetTypeInfo().IsAssignableFrom(ImplementationType.GetTypeInfo()) && Name == key.Name;
        }

        /// <inheritdoc/>
        public override void AssertIsValid()
        {
            base.AssertIsValid();
            AssertImplementationTypeIsAssignableToServiceType();
        }

        /// <summary>
        /// Asserts that the <see cref="ServiceType"/> and <see cref="ImplementationType"/> are compatible.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method will raise an exception if the implementation type is neither equal to nor
        /// derives from the service type.
        /// </para>
        /// </remarks>
        /// <exception cref="InvalidTypeRegistrationException">If the service type and implementation
        /// type have an invalid combination of values.</exception>
        protected void AssertImplementationTypeIsAssignableToServiceType()
        {
            if(ServiceType.GetTypeInfo().IsAssignableFrom(ImplementationType.GetTypeInfo())) return;

            var message = String.Format(Resources.ExceptionFormats.ImplementationTypeMustDeriveFromComponentType,
                                        GetType().Name,
                                        ImplementationType.FullName,
                                        ServiceType.FullName);
            throw new InvalidTypeRegistrationException(message);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypedRegistration"/> class.
        /// </summary>
        /// <param name="implementationType"></param>
        /// <param name="priority"></param>
        /// <param name="cacheable"></param>
        /// <param name="disposeWithContainer"></param>
        protected TypedRegistration(Type implementationType,
                                    int priority = 1,
                                    bool cacheable = true,
                                    bool disposeWithContainer = true) : base(priority, cacheable, disposeWithContainer)
        {
            ImplementationType = implementationType ?? throw new ArgumentNullException(nameof(implementationType));
        }
    }
}
