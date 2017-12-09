using System;
using System.Collections.Generic;
using CSF.MicroDi.Registration;

namespace CSF.MicroDi
{
  public interface IContainer : IResolvesServices, IReceivesRegistrations, IDisposable
  {
    bool HasRegistration<T>(string name = null);
    bool HasRegistration(Type serviceType, string name = null);

    IReadOnlyCollection<IServiceRegistration> GetRegistrations();
    IReadOnlyCollection<IServiceRegistration> GetRegistrations(Type serviceType);
  }
}
