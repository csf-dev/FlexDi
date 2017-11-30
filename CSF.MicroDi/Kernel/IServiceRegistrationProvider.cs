using System;
using System.Collections.Generic;

namespace CSF.MicroDi.Kernel
{
  public interface IServiceRegistrationProvider
  {
    bool CanFulfilRequest(ResolutionRequest request);

    IServiceRegistration Get(ResolutionRequest request);

    IReadOnlyCollection<IServiceRegistration> GetAll(Type serviceType);
  }
}
