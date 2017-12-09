using System;
using CSF.MicroDi.Resolution;

namespace CSF.MicroDi.Registration
{
  public class FactoryRegistration<T> : TypedRegistration
  {
    readonly Func<T> factory;

    public override Type ImplementationType => typeof(T);

    public override IFactoryAdapter GetFactoryAdapter() => new DelegateFactory(factory);

    public override string ToString()
    {
      return $"[Factory registration for `{ServiceType.FullName}', creating an instance of `{ImplementationType.FullName}']";
    }

    public FactoryRegistration(Func<T> factory)
    {
      if(factory == null)
        throw new ArgumentNullException(nameof(factory));

      this.factory = factory;
    }
  }
}
