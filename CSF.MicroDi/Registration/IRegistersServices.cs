using System;
namespace CSF.MicroDi.Registration
{
  public interface IRegistersServices : IServiceRegistrationProvider
  {
    void Add(IServiceRegistration registration);
  }
}
