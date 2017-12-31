using System;
using CSF.MicroDi.Registration;

namespace CSF.MicroDi.Resolution
{
  public interface ICreatesObjectInstances
  {
    object CreateFromFactory(IFactoryAdapter factory, ResolutionPath path, IServiceRegistration registration);
  }
}
