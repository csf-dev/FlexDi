using System;
using CSF.MicroDi.Registration;

namespace CSF.MicroDi.Resolution
{
  public interface IResolverWithScope : IResolver
  {
    IResolverWithScope CreateChildScope(ICachesResolvedServiceInstances cache);

    new IResolverWithScope CreateChild(IServiceRegistration registration);
  }
}
