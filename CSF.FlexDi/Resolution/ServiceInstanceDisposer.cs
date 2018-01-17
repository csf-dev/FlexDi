//
//    ServiceInstanceDisposer.cs
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
  public class ServiceInstanceDisposer : IDisposesOfResolvedInstances
  {
    public void DisposeInstances(IServiceRegistrationProvider registrationProvider,
                                 ICachesResolvedServiceInstances instanceCache)
    {
      if(registrationProvider == null)
        throw new ArgumentNullException(nameof(registrationProvider));
      if(instanceCache == null)
        throw new ArgumentNullException(nameof(instanceCache));

      var registrationsToDispose = GetRegistrationsToDispose(registrationProvider);
      Dispose(registrationsToDispose, instanceCache);
    }

    protected virtual IReadOnlyCollection<IServiceRegistration> GetRegistrationsToDispose(IServiceRegistrationProvider registrationProvider)
    {
      if(registrationProvider == null)
        throw new ArgumentNullException(nameof(registrationProvider));
      
      return registrationProvider
        .GetAll()
        .Where(x => x.Cacheable && x.DisposeWithContainer)
        .ToArray();
    }

    protected virtual void Dispose(IReadOnlyCollection<IServiceRegistration> registrations,
                                   ICachesResolvedServiceInstances instanceCache)
    {
      if(instanceCache == null)
        throw new ArgumentNullException(nameof(instanceCache));
      if(registrations == null)
        throw new ArgumentNullException(nameof(registrations));

      foreach(var reg in registrations)
        Dispose(reg, instanceCache);
    }

    protected virtual void Dispose(IServiceRegistration registration, ICachesResolvedServiceInstances instanceCache)
    {
      if(registration == null)
        throw new ArgumentNullException(nameof(registration));
      if(instanceCache == null)
        throw new ArgumentNullException(nameof(instanceCache));

      object instance;

      if(!instanceCache.TryGet(registration, out instance))
        return;

      if(!(instance is IDisposable))
        return;

      ((IDisposable) instance).Dispose();
    }
  }
}
