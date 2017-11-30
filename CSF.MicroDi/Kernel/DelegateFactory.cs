using System;
using System.Collections.Generic;
using System.Reflection;

namespace CSF.MicroDi.Kernel
{
  public class DelegateFactory : IFactoryAdapter
  {
    readonly Delegate factory;

    public object Execute(object[] parameters)
    {
      return factory.DynamicInvoke(parameters);
    }

    public IReadOnlyList<ParameterInfo> GetParameters()
    {
      return factory.Method.GetParameters();
    }

    public DelegateFactory(Delegate factory)
    {
      if(factory == null)
        throw new ArgumentNullException(nameof(factory));
      this.factory = factory;
    }
  }
}
