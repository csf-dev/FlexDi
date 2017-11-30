using System;
using System.Collections.Generic;
using CSF.MicroDi.Kernel;

namespace CSF.MicroDi.Registration
{
  public interface IBulkRegistrationProvider
  {
    IReadOnlyCollection<IServiceRegistration> GetRegistrations();
  }
}
