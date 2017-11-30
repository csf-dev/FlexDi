using System;
namespace CSF.MicroDi.Registration
{
  public interface IAsBuilder
  {
    IRegistrationOptionsBuilder As<T>() where T : class;
    IRegistrationOptionsBuilder As(Type serviceType);
  }
}
