//
//    ResolutionPath.cs
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
using System.Collections.Generic;
using System.Linq;
using CSF.FlexDi.Registration;

namespace CSF.FlexDi.Resolution
{
    /// <summary>
    /// Describes a 'path' taken through the resolution process so far, listing each service/component registration
    /// traversed.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This type is used in order to detect circular dependencies, but also to help describe when exceptions
    /// are raised.  It may be interrogated via the <see cref="GetRegistrations"/> method in order to get an ordered
    /// collection of the registrations which have been traversed.
    /// </para>
    /// <para>
    /// Essentially the pathway represents a description such as "I am resolving component X, which was required
    /// as a dependency by component Y, which itself was required as a dependency to component Z".  So if an error occurs
    /// resolving component Z, but the error relates to component X (which might have no connection to Z), this path
    /// helps trace that relationship back to the source.
    /// </para>
    /// </remarks>
    public class ResolutionPath
    {
        readonly Stack<IServiceRegistration> path;

        /// <summary>
        /// Gets a value indicating whether this <see cref="CSF.FlexDi.Resolution.ResolutionPath"/> is empty.
        /// </summary>
        /// <value><c>true</c> if is empty; otherwise, <c>false</c>.</value>
        public bool IsEmpty => !path.Any();

        /// <summary>
        /// Exposes the current (most recent) registration traversed.
        /// </summary>
        /// <value>The current registration.</value>
        public IServiceRegistration CurrentRegistration => path.Peek();

        /// <summary>
        /// Gets a value indicating whether or not this resolution path instance contains a registration for the given
        /// service/component type.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this path contains a registration for the specified type; <c>false</c> otherwise.
        /// </returns>
        /// <param name="serviceType">Service type.</param>
        public bool Contains(Type serviceType) => Contains(serviceType, null);

        /// <summary>
        /// Gets a value indicating whether or not this resolution path instance contains a registration for the given
        /// resolution request.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this path contains a registration for the specified request; <c>false</c> otherwise.
        /// </returns>
        /// <param name="request">Request.</param>
        public bool Contains(ResolutionRequest request) => Contains(request.ServiceType, request.Name);

        /// <summary>
        /// Gets a value indicating whether or not this resolution path instance contains a registration for the given
        /// service/component type and registration name.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this path contains a registration for the specified type and name; <c>false</c> otherwise.
        /// </returns>
        /// <param name="serviceType">Service type.</param>
        /// <param name="name">Name.</param>
        public bool Contains(Type serviceType, string name)
        {
            if(serviceType == null)
                throw new ArgumentNullException(nameof(serviceType));

            var candidates = path.Where(x => RegistrationMatches(x, serviceType));

            if(name != null)
                candidates = candidates.Where(x => x.Name == name);

            return candidates.Any();
        }

        /// <summary>
        /// Gets a value indicating whether or not this resolution path instance contains a registration for the given
        /// service/component registration.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this path contains the specified registration; <c>false</c> otherwise.
        /// </returns>
        /// <param name="registration">Registration.</param>
        public bool Contains(IServiceRegistration registration)
        {
            if(registration == null)
                throw new ArgumentNullException(nameof(registration));

            var candidates = path.Where(x => RegistrationMatches(x, registration));

            if(registration.Name != null)
                candidates = candidates.Where(x => x.Name == registration.Name);

            return candidates.Any();
        }

        /// <summary>
        /// Exposes an ordered collection of all of the registrations contained within this instance.
        /// </summary>
        /// <returns>The registrations.</returns>
        public IList<IServiceRegistration> GetRegistrations() => path.ToList();

        /// <summary>
        /// Creates a new <see cref="ResolutionPath"/> instance, based upon the current instance but also containing the
        /// specified registration.
        /// </summary>
        /// <returns>The child.</returns>
        /// <param name="registration">Registration.</param>
        public ResolutionPath CreateChild(IServiceRegistration registration)
        {
            if(registration == null)
                throw new ArgumentNullException(nameof(registration));

            return new ResolutionPath(path, registration);
        }

        static bool RegistrationMatches(IServiceRegistration candidate, Type serviceType)
        {
            return candidate.ServiceType == serviceType;
        }

        static bool RegistrationMatches(IServiceRegistration candidate, IServiceRegistration actual)
        {
            var typedCandidate = candidate as TypedRegistration;
            var typedActual = actual as TypedRegistration;
            if(typedCandidate == null || typedActual == null)
                return RegistrationMatches(candidate, actual.ServiceType);

            return (typedCandidate.ServiceType == typedActual.ServiceType
                            && typedCandidate.ImplementationType == typedActual.ImplementationType);
        }

        ResolutionPath(IEnumerable<IServiceRegistration> previousPath, IServiceRegistration nextRegistration)
        {
            if(nextRegistration == null)
                throw new ArgumentNullException(nameof(nextRegistration));
            if(previousPath == null)
                throw new ArgumentNullException(nameof(previousPath));

            path = new Stack<IServiceRegistration>(previousPath);
            path.Push(nextRegistration);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CSF.FlexDi.Resolution.ResolutionPath"/> class.
        /// </summary>
        /// <param name="registrations">The registrations to contain within the path.</param>
        public ResolutionPath(IReadOnlyList<IServiceRegistration> registrations)
        {
            if(registrations == null)
                throw new ArgumentNullException(nameof(registrations));

            path = new Stack<IServiceRegistration>(registrations);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CSF.FlexDi.Resolution.ResolutionPath"/> class.
        /// </summary>
        /// <param name="firstRegistration">An optional initial registration with which to 'prime' the current instance.</param>
        public ResolutionPath(IServiceRegistration firstRegistration = null)
        {
            path = new Stack<IServiceRegistration>();

            if(firstRegistration != null)
                path.Push(firstRegistration);
        }
    }
}
