using System;
namespace CSF.MicroDi.Registration
{
  public interface IAsBuilderWithMultiplicity
  {
    IRegistrationOptionsBuilderWithMultiplicity As<T>() where T : class;
    IRegistrationOptionsBuilderWithMultiplicity As(Type serviceType);
    IRegistrationOptionsBuilderWithMultiplicity AsOwnType();
  }
}
