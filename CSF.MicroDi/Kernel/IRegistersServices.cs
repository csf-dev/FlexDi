using System;
namespace CSF.MicroDi.Kernel
{
  public interface IRegistersServices : IServiceRegistrationProvider
  {
    void Add(IServiceRegistration registration);
  }
}
