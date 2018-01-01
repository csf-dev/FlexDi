using System;
using System.Collections.Generic;
using System.Linq;
using CSF.MicroDi.Registration;

namespace CSF.MicroDi.Resolution
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
        .Where(x => x.Multiplicity == Multiplicity.Shared && x.DisposeWithContainer)
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
