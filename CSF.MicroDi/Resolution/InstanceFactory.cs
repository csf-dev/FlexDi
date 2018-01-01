using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CSF.MicroDi.Resolution
{
  public class InstanceFactory : IFactoryAdapter
  {
    readonly object instance;

    public object Instance => instance;

    public bool RequiresParameterResolution => false;

    public object Execute(object[] parameters) => instance;

    public IReadOnlyList<ParameterInfo> GetParameters() => Enumerable.Empty<ParameterInfo>().ToArray();

    public InstanceFactory(object instance)
    {
      this.instance = instance;
    }
  }
}
