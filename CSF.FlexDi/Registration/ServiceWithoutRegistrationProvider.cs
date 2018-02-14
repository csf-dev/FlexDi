//
//    ServiceWithoutRegistrationProvider.cs
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
  /// A specialised implementation of <see cref="IServiceRegistrationProvider"/> which is used in order to
  /// provide registrations for components which have not been registered.
  /// </summary>
  /// <seealso cref="ContainerOptions.ResolveUnregisteredTypes"/>
  public class ServiceWithoutRegistrationProvider : IServiceRegistrationProvider
  {
    readonly ISelectsConstructor constructorSelector;

    /// <summary>
    /// Gets a value which indicates whether or not the current provider can fulfil the given resolution request.
    /// </summary>
    /// <returns>
    /// <c>true</c>, if the request can be fulfilled, <c>false</c> otherwise.</returns>
    /// <param name="request">A resolution request.</param>
    public virtual bool CanFulfilRequest(ResolutionRequest request)
    {
      if(request == null)
        throw new ArgumentNullException(nameof(request));

      return true;
    }

    /// <summary>
    /// Gets a registration.
    /// </summary>
    /// <param name="request">A resolution request.</param>
    public virtual IServiceRegistration Get(ResolutionRequest request)
    {
      if(request == null)
        throw new ArgumentNullException(nameof(request));
      
      return new TypeRegistration(request.ServiceType, constructorSelector) {
        Name = request.Name,
        ServiceType = request.ServiceType,
        Cacheable = true
      };
    }

    /// <summary>
    /// Gets all of the registrations which can fulfil a given service/component type.
    /// </summary>
    /// <returns>All of the matching registrations.</returns>
    /// <param name="serviceType">A service type.</param>
    public virtual IReadOnlyCollection<IServiceRegistration> GetAll(Type serviceType)
    {
      if(serviceType == null)
        throw new ArgumentNullException(nameof(serviceType));

      return new [] { Get(new ResolutionRequest(serviceType, null)) };
    }

    /// <summary>
    /// Gets all of the registrations available to the current provider
    /// </summary>
    /// <returns>All of the registrations.</returns>
    public virtual IReadOnlyCollection<IServiceRegistration> GetAll()
    {
      throw new NotSupportedException($"This type does not support use of {nameof(GetAll)} with no parameters.");
    }

    bool IServiceRegistrationProvider.HasRegistration(ServiceRegistrationKey key)
    {
      if(key == null)
        throw new ArgumentNullException(nameof(key));
      
      return true;
    }

    bool IServiceRegistrationProvider.HasRegistration(IServiceRegistration registration)
    {
      if(registration == null)
        throw new ArgumentNullException(nameof(registration));

      return true;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceWithoutRegistrationProvider"/> class.
    /// </summary>
    /// <param name="constructorSelector">Constructor selector.</param>
    public ServiceWithoutRegistrationProvider(ISelectsConstructor constructorSelector)
    {
      if(constructorSelector == null)
        throw new ArgumentNullException(nameof(constructorSelector));

      this.constructorSelector = constructorSelector;
    }
  }
}
