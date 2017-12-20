using System;
using CSF.MicroDi.Registration;

namespace CSF.MicroDi.Resolution
{
  public interface ICachesResolvedServiceInstancesWithScope : ICachesResolvedServiceInstances
  {
    ICachesResolvedServiceInstancesWithScope CreateChildScope(ICachesResolvedServiceInstances provider,
                                                              IServiceRegistrationProvider registry);
  }
}
