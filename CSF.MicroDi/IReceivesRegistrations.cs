using System;
using System.Collections.Generic;
using CSF.MicroDi.Builders;
using CSF.MicroDi.Registration;

namespace CSF.MicroDi
{
  public interface IReceivesRegistrations
  {
    void AddRegistrations(Action<IRegistrationHelper> registrations);
    void AddRegistrations(IEnumerable<IServiceRegistration> registrations);
  }
}
