//
//    IServiceRegistrationProvider.cs
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
using CSF.FlexDi.Resolution;

namespace CSF.FlexDi.Registration
{
  /// <summary>
  /// A service type which is able to provide access to service/component registrations.
  /// </summary>
  public interface IServiceRegistrationProvider
  {
    /// <summary>
    /// Gets a value which indicates whether or not the current provider can fulfil the given resolution request.
    /// </summary>
    /// <returns><c>true</c>, if the request can be fulfilled, <c>false</c> otherwise.</returns>
    /// <param name="request">A resolution request.</param>
    bool CanFulfilRequest(ResolutionRequest request);

    /// <summary>
    /// Gets a value which indicates whether or not the current provider has a matching registrations.
    /// </summary>
    /// <returns><c>true</c>, if a matching registration is available, <c>false</c> otherwise.</returns>
    /// <param name="key">A registration key.</param>
    bool HasRegistration(ServiceRegistrationKey key);

    /// <summary>
    /// Gets a value which indicates whether or not the current provider has a specified registrations.
    /// </summary>
    /// <returns><c>true</c>, if the registration is contained within this provider, <c>false</c> otherwise.</returns>
    /// <param name="registration">A registration.</param>
    bool HasRegistration(IServiceRegistration registration);

    /// <summary>
    /// Gets a registration.
    /// </summary>
    /// <param name="request">A resolution request.</param>
    IServiceRegistration Get(ResolutionRequest request);

    /// <summary>
    /// Gets all of the registrations which can fulfil a given service/component type.
    /// </summary>
    /// <returns>All of the matching registrations.</returns>
    /// <param name="serviceType">A service type.</param>
    IReadOnlyCollection<IServiceRegistration> GetAll(Type serviceType);

    /// <summary>
    /// Gets all of the registrations available to the current provider
    /// </summary>
    /// <returns>All of the registrations.</returns>
    IReadOnlyCollection<IServiceRegistration> GetAll();
  }
}
