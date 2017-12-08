using System;
using System.Collections.Generic;
using CSF.MicroDi.Resolution;

namespace CSF.MicroDi.Registration
{
  public interface IServiceRegistrationProvider
  {
    bool CanFulfilRequest(ResolutionRequest request);

    IServiceRegistration Get(ResolutionRequest request);

    IReadOnlyCollection<IServiceRegistration> GetAll(Type serviceType);

    IReadOnlyCollection<IServiceRegistration> GetAll();
  }
}
