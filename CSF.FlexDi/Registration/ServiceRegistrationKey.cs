//
//    ServiceRegistrationKey.cs
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
    /// A key which will unambiguously identify a service registration.
    /// </summary>
    public sealed class ServiceRegistrationKey : IEquatable<ServiceRegistrationKey>
    {
        /// <summary>
        /// Gets the service type, that is the type which is depended-upon (or which would be resolved).
        /// </summary>
        /// <value>The type of the service.</value>
        public Type ServiceType { get; private set; }

        /// <summary>
        /// Gets an optional name for the registration.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="ServiceRegistrationKey"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="ServiceRegistrationKey"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="ServiceRegistrationKey"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as ServiceRegistrationKey);
        }

        /// <summary>
        /// Determines whether the specified <see cref="CSF.FlexDi.Registration.ServiceRegistrationKey"/> is equal to the
        /// current <see cref="ServiceRegistrationKey"/>.
        /// </summary>
        /// <param name="other">The <see cref="CSF.FlexDi.Registration.ServiceRegistrationKey"/> to compare with the current <see cref="ServiceRegistrationKey"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="CSF.FlexDi.Registration.ServiceRegistrationKey"/> is equal to the
        /// current <see cref="ServiceRegistrationKey"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(ServiceRegistrationKey other)
        {
            if(ReferenceEquals(other, null))
                return false;
            if(ReferenceEquals(other, this))
                return true;

            return (other.ServiceType == ServiceType
                            && other.Name == Name);
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="ServiceRegistrationKey"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
        public override int GetHashCode()
        {
            var typeHash = ServiceType.GetHashCode();
            var nameHash = Name?.GetHashCode() ?? 0;

            return typeHash ^ nameHash;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceRegistrationKey"/> class.
        /// </summary>
        /// <param name="serviceType">Service type.</param>
        /// <param name="name">Name.</param>
        public ServiceRegistrationKey(Type serviceType, string name)
        {
            if(serviceType == null)
                throw new ArgumentNullException(nameof(serviceType));

            ServiceType = serviceType;
            Name = name;
        }

        /// <summary>
        /// Creates a service registration key based on a given registration instance.
        /// </summary>
        /// <returns>The registration key.</returns>
        /// <param name="registration">Registration.</param>
        public static ServiceRegistrationKey ForRegistration(IServiceRegistration registration)
        {
            if(registration == null)
                throw new ArgumentNullException(nameof(registration));

            return new ServiceRegistrationKey(registration.ServiceType, registration.Name);
        }

        /// <summary>
        /// Creates a service registration key based on a given resolution request.
        /// </summary>
        /// <returns>The registration key.</returns>
        /// <param name="request">Request.</param>
        public static ServiceRegistrationKey FromRequest(ResolutionRequest request)
        {
            if(request == null)
                throw new ArgumentNullException(nameof(request));

            return new ServiceRegistrationKey(request.ServiceType, request.Name);
        }
    }
}
