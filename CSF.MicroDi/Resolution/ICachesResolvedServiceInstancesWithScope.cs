using System;
namespace CSF.MicroDi.Resolution
{
  public interface ICachesResolvedServiceInstancesWithScope : ICachesResolvedServiceInstances
  {
    ICachesResolvedServiceInstancesWithScope CreateChildScope(ICachesResolvedServiceInstances provider);
  }
}
