using System;
using CSF.MicroDi.Resolution;

namespace CSF.MicroDi.Registration
{
  public class FactoryRegistration : ServiceRegistration
  {
    readonly Delegate factory;

    public override IFactoryAdapter GetFactoryAdapter() => new DelegateFactory(factory);

    public override string ToString()
    {
      return $"[Factory registration for `{ServiceType.FullName}']";
    }

    public FactoryRegistration(Delegate factory)
    {
      if(factory == null)
        throw new ArgumentNullException(nameof(factory));

      this.factory = factory;
    }
  }
}
