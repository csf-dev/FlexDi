using System;
namespace CSF.MicroDi.Builders
{
  public interface IAsBuilder
  {
    IRegistrationOptionsBuilder As<T>() where T : class;
    IRegistrationOptionsBuilder As(Type serviceType);
  }
}
