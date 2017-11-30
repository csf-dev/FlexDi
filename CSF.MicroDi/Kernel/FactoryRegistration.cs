using System;
namespace CSF.MicroDi.Kernel
{
  public class FactoryRegistration : ServiceRegistration
  {
    readonly Delegate factory;

    public override object CreateInstance(IResolutionContext context)
    {
      if(context == null)
        throw new ArgumentNullException(nameof(context));

      var adapter = GetFactoryAdapter();
      return context.Resolve(adapter);
    }

    IFactoryAdapter GetFactoryAdapter()
      => new DelegateFactory(factory);

    public FactoryRegistration(Delegate factory)
    {
      if(factory == null)
        throw new ArgumentNullException(nameof(factory));

      this.factory = factory;
    }
  }
}
