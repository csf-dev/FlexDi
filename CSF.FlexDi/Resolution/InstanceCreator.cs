﻿//
//    InstanceCreator.cs
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
using System.Linq;
using System.Reflection;
using CSF.FlexDi.Registration;

namespace CSF.FlexDi.Resolution
{
    /// <summary>
    /// Implementation of <see cref="ICreatesObjectInstances"/> which creates service/component instances.
    /// </summary>
    public class InstanceCreator : ICreatesObjectInstances
    {
        readonly IFulfilsResolutionRequests resolver;

        /// <summary>
        /// Creates a service/component instance from a factory adapter, resolution path and registration.s
        /// </summary>
        /// <returns>The created component instance.</returns>
        /// <param name="factory">The factory adapter from which to create the instance.</param>
        /// <param name="path">The current resolution path.</param>
        /// <param name="registration">The registration for the component to be created.</param>
        public virtual object CreateFromFactory(IFactoryAdapter factory,
                                                ResolutionPath path,
                                                IServiceRegistration registration)
        {
            if(factory == null)
                throw new ArgumentNullException(nameof(factory));
            
            if(!factory.RequiresParameterResolution)
                return factory.Execute(Enumerable.Empty<object>().ToArray());

            var parameters = factory.GetParameters();

            var resolvedParameters = parameters
                .Select(param => ResolveParameter(param, path, registration))
                .ToArray();

            return factory.Execute(resolvedParameters);
        }

        /// <summary>
        /// Resolves a parameter for a <see cref="IFactoryAdapter"/>.
        /// </summary>
        /// <returns>The resolved parameter value.</returns>
        /// <param name="parameter">The parameter.</param>
        /// <param name="path">The resolution path.</param>
        /// <param name="registration">The registration for the service currently being resolved.</param>
        protected virtual object ResolveParameter(ParameterInfo parameter,
                                                  ResolutionPath path,
                                                  IServiceRegistration registration)
        {
            var request = ConvertToResolutionRequest(parameter, path, registration);
            var result = resolver.Resolve(request);

            if(!result.IsSuccess)
            {
                var message = String.Format(Resources.ExceptionFormats.FailedToResolveParameter,
                                                                        parameter.ParameterType.FullName,
                                                                        parameter.Name);
                throw new CannotResolveParameterException(message) {
                    ResolutionPath = path,
                };
            }

            return result.ResolvedObject;
        }

        static ResolutionRequest ConvertToResolutionRequest(ParameterInfo parameter,
                                                            ResolutionPath path,
                                                            IServiceRegistration registration)
        {
            if(parameter == null)
                throw new ArgumentNullException(nameof(parameter));

            var childPath = path.CreateChild(registration);
            return new ResolutionRequest(parameter.ParameterType, parameter.Name, childPath);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CSF.FlexDi.Resolution.InstanceCreator"/> class.
        /// </summary>
        /// <param name="resolver">A service which fulfils resolution requests.</param>
        public InstanceCreator(IFulfilsResolutionRequests resolver)
        {
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
        }
    }
}
