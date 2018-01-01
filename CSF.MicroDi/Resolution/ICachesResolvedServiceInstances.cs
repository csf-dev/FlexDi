using System;
using CSF.MicroDi.Registration;

namespace CSF.MicroDi.Resolution
{
  public interface ICachesResolvedServiceInstances
  {
    void Add(IServiceRegistration registration, object instance);
    bool Has(IServiceRegistration registration);
    bool Has(ServiceRegistrationKey key);
    bool TryGet(IServiceRegistration registration, out object instance);
  }
}
