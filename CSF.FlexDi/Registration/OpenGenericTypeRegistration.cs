//
//    OpenGenericTypeRegistration.cs
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
using System.Reflection;

namespace CSF.FlexDi.Registration
{
    /// <summary>
    /// Specialisation of <see cref="TypeRegistration"/> which registers open generic types, such as
    /// <c>MyGenericClass&lt;&gt;</c> without their generic type parameters specified.
    /// </summary>
    public class OpenGenericTypeRegistration : TypeRegistration
    {
        /// <inheritdoc/>
        public override void AssertIsValid()
        {
            AssertCachabilityAndDisposalAreValid();
            AssertImplementationTypeIsAssignableToServiceTypeGeneric();
        }

        /// <summary>
        /// Similar to <see cref="TypedRegistration.AssertImplementationTypeIsAssignableToServiceType"/>, asserts that the
        /// <see cref="ServiceRegistration.ServiceType"/> and <see cref="TypedRegistration.ImplementationType"/> are compatible,
        /// but in an open generic manner.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method will raise an exception if the open generic form of the implementation type is neither equal to nor
        /// derives from the open generic form of the service type.
        /// </para>
        /// </remarks>
        /// <exception cref="InvalidTypeRegistrationException">If the service type and implementation
        /// type have an invalid combination of values.</exception>
        protected void AssertImplementationTypeIsAssignableToServiceTypeGeneric()
        {
            if (ImplementationType.IsAssignableToOpenGeneric(ServiceType)) return;
            var message = String.Format(Resources.ExceptionFormats.InvalidOpenGenericRegistration,
                                        nameof(OpenGenericTypeRegistration),
                                        ImplementationType.FullName,
                                        ServiceType.FullName);
            throw new InvalidTypeRegistrationException(message);
        }

        /// <inheritdoc/>
        public override IFactoryAdapter GetFactoryAdapter(ResolutionRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            if (!request.ServiceType.GetTypeInfo().IsGenericType)
            {
                var message = String.Format(Resources.ExceptionFormats.RequestMustBeForGenericType, request.ServiceType.FullName);
                throw new ArgumentException(message, nameof(request));
            }

            var requestedGenericTypeArgs = request.ServiceType.GetTypeInfo().GenericTypeArguments;
            var implementationTypeWithGenericParams = ImplementationType.MakeGenericType(requestedGenericTypeArgs);

            return GetFactoryAdapter(implementationTypeWithGenericParams);
        }

        /// <inheritdoc/>
        public override bool MatchesKey(ServiceRegistrationKey key)
        {
            if (key == null) return false;
            if (!key.ServiceType.GetTypeInfo().IsGenericType) return false;

            return key.ServiceType.GetGenericTypeDefinition() == ServiceType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGenericTypeRegistration"/> class.
        /// </summary>
        /// <param name="implementationType">Implementation type.</param>
        /// <param name="constructorSelector">Constructor selector.</param>
        public OpenGenericTypeRegistration(Type implementationType, ISelectsConstructor constructorSelector) : base(implementationType, constructorSelector) { }
    }
}
