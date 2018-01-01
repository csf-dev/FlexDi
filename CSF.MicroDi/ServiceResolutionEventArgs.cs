using System;
using CSF.MicroDi.Registration;

namespace CSF.MicroDi
{
  public class ServiceResolutionEventArgs : EventArgs
  {
    readonly object instance;
    readonly IServiceRegistration registration;

    public object Instance => instance;
    public IServiceRegistration Registration => registration;

    public ServiceResolutionEventArgs(IServiceRegistration registration, object instance)
    {
      if(registration == null)
        throw new ArgumentNullException(nameof(registration));

      this.registration = registration;
      this.instance = instance;
    }
  }
}
