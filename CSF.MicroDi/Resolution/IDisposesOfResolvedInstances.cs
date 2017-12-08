using System;
using CSF.MicroDi.Registration;

namespace CSF.MicroDi.Resolution
{
  public interface IDisposesOfResolvedInstances
  {
    void DisposeInstances(IServiceRegistrationProvider registrationProvider, ICachesResolvedServiceInstances instances);
  }
}
