using System;
using CSF.MicroDi.Registration;

namespace CSF.MicroDi.Resolution
{
  public interface ICachesResolvedServiceInstances
  {
    void Add(ServiceRegistrationKey key, object instance);
    bool Has(ServiceRegistrationKey key);
    bool TryGet(ServiceRegistrationKey key, out object instance);
  }
}
