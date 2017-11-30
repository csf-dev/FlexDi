using System;
namespace CSF.MicroDi.Kernel
{
  public class FactoryRegistration<T> : TypedRegistration
  {
    readonly Func<T> factory;

    public override Type ImplementationType => typeof(T);

    public override object CreateInstance(IResolutionContext context)
    {
      return factory();
    }

    public FactoryRegistration(Func<T> factory)
    {
      if(factory == null)
        throw new ArgumentNullException(nameof(factory));

      this.factory = factory;
    }
  }
}
