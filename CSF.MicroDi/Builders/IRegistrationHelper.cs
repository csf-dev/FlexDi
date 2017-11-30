using System;
namespace CSF.MicroDi.Builders
{
  public interface IRegistrationHelper
  {
    IAsBuilderWithMultiplicity RegisterType<T>() where T : class;
    IAsBuilderWithMultiplicity RegisterType(Type concreteType);

    IAsBuilderWithMultiplicity RegisterFactory<T>(Func<T> factory) where T : class;
    IRegistrationOptionsBuilderWithMultiplicity RegisterFactory(Delegate factory, Type serviceType);
    IRegistrationOptionsBuilderWithMultiplicity RegisterFactory<TService>(Delegate factory) where TService : class;

    IAsBuilder RegisterInstance(object instance);
  }
}
