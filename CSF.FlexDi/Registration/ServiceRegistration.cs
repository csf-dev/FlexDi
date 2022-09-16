//
//    ServiceRegistration.cs
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
    /// Base class for implementations of <see cref="IServiceRegistration"/>.
    /// </summary>
    public abstract class ServiceRegistration : IServiceRegistration
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="T:CSF.FlexDi.Registration.IServiceRegistration" /> is cacheable.
        /// </summary>
        /// <value>
        /// <c>true</c> if the registration is cacheable; otherwise, <c>false</c>.</value>
        public bool Cacheable { get; set; }

        /// <summary>
        /// Gets an optional registration name.
        /// </summary>
        /// <value>The name.</value>
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets the <c>System.Type</c> which will be fulfilled by this registration.
        /// </summary>
        /// <value>The service/component type.</value>
        public virtual Type ServiceType { get; set; }

        /// <summary>
        /// Gets a value indicating whether component instances created from this <see cref="T:CSF.FlexDi.Registration.IServiceRegistration" />
        /// should be disposed with the container which created them.
        /// </summary>
        /// <seealso cref="M:CSF.FlexDi.Builders.IRegistrationOptionsBuilder.DoNotDisposeWithContainer" />
        /// <value>
        /// <c>true</c> if the component should be disposed with the container; otherwise, <c>false</c>.</value>
        public bool DisposeWithContainer { get; set; }

        /// <summary>
        /// Gets a numeric priority for the current registration instance.  Higher numeric priorities take precedence
        /// over lower ones.
        /// </summary>
        /// <value>The priority of this registration.</value>
        public int Priority { get; }

        /// <summary>
        /// Gets a factory adapter instance, for the current registration, from a specified resolution request.
        /// </summary>
        /// <returns>The factory adapter.</returns>
        /// <param name="request">A resolution request.</param>
        public abstract  IFactoryAdapter GetFactoryAdapter(ResolutionRequest request);

        /// <summary>
        /// Asserts that the current registration is valid (fulfils its invariants).  An exception is raised if it does not.
        /// </summary>
        /// <exception cref="InvalidRegistrationException">If the current registration instance is invalid.</exception>
        public virtual void AssertIsValid()
        {
            AssertCachabilityAndDisposalAreValid();
        }

        /// <summary>
        /// Asserts that the values of <see cref="Cacheable"/> and <see cref="DisposeWithContainer"/> are valid.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method will throw an exception if <see cref="DisposeWithContainer"/> if <see langword="true" /> and
        /// <see cref="Cacheable"/> is <see langword="false" />, as this is an impossible scenario.
        /// </para>
        /// </remarks>
        /// <exception cref="InvalidTypeRegistrationException">If the cacheable and disposal properties have
        /// an invalid combination of values.</exception>
        protected void AssertCachabilityAndDisposalAreValid()
        {
            if(!Cacheable && DisposeWithContainer)
            {
                var message = String.Format(Resources.ExceptionFormats.InvalidCacheableAndDisposeWithContainerCombination,
                                                                        nameof(DisposeWithContainer),
                                                                        Boolean.TrueString,
                                                                        nameof(Cacheable),
                                                                        Boolean.FalseString);
                throw new InvalidRegistrationException(message);
            }
        }

        /// <summary>
        /// Gets a value that indicates whether or not the current registration matches the specified registration key or not.
        /// </summary>
        /// <returns>
        /// <c>true</c>, if the current instance matches the specified key, <c>false</c> otherwise.</returns>
        /// <param name="key">The registration key against which to test.</param>
        public virtual bool MatchesKey(ServiceRegistrationKey key)
        {
            if(key == null) return false;
            return ServiceType == key.ServiceType
                && Name == key.Name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceRegistration"/> class.
        /// </summary>
        /// <param name="priority">The priority of the current registration.</param>
        /// <param name="cacheable">A default value indicating whether the registration is cacheable.</param>
        /// <param name="disposeWithContainer">A default value indicating whether instances should be disposed with the container.</param>
        protected ServiceRegistration(int priority = 1, bool cacheable = true, bool disposeWithContainer = true)
        {
            Priority = priority;
            Cacheable = cacheable;
            DisposeWithContainer = disposeWithContainer;
        }
    }
}
