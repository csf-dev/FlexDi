using System;
using System.Collections.Generic;
using CSF.MicroDi.Registration;

namespace CSF.MicroDi.Builders
{
  public interface IBulkRegistrationProvider
  {
    IReadOnlyCollection<IServiceRegistration> GetRegistrations();
  }
}
