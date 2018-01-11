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
using CSF.MicroDi.Resolution;

namespace CSF.MicroDi.Registration
{
  public class ServiceWithoutRegistrationProvider : IServiceRegistrationProvider
  {
    readonly ISelectsConstructor constructorSelector;

    public virtual bool CanFulfilRequest(ResolutionRequest request)
    {
      if(request == null)
        throw new ArgumentNullException(nameof(request));

      return true;
    }

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

    public virtual IReadOnlyCollection<IServiceRegistration> GetAll(Type serviceType)
    {
      if(serviceType == null)
        throw new ArgumentNullException(nameof(serviceType));

      return new [] { Get(new ResolutionRequest(serviceType, null)) };
    }

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

    public ServiceWithoutRegistrationProvider(ISelectsConstructor constructorSelector)
    {
      if(constructorSelector == null)
        throw new ArgumentNullException(nameof(constructorSelector));

      this.constructorSelector = constructorSelector;
    }
  }
}
