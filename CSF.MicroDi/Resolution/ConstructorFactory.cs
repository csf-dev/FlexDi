using System;
using System.Collections.Generic;
using System.Reflection;

namespace CSF.MicroDi.Resolution
{
  public class ConstructorFactory : IFactoryAdapter
  {
    readonly ConstructorInfo ctor;

    public object Execute(object[] parameters)
    {
      return ctor.Invoke(parameters);
    }

    public IReadOnlyList<ParameterInfo> GetParameters()
    {
      return ctor.GetParameters();
    }

    public ConstructorFactory(ConstructorInfo ctor)
    {
      if(ctor == null)
        throw new ArgumentNullException(nameof(ctor));

      this.ctor = ctor;
    }
  }
}
