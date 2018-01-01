using System;
namespace CSF.MicroDi.Builders
{
  public interface IAsBuilderWithMultiplicity
  {
    IRegistrationOptionsBuilderWithMultiplicity As<T>() where T : class;
    IRegistrationOptionsBuilderWithMultiplicity As(Type serviceType);
    IRegistrationOptionsBuilderWithMultiplicity AsOwnType();
  }
}
