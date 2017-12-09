using System;
using System.Collections.Generic;
using System.Reflection;

namespace CSF.MicroDi.Resolution
{
  public interface IFactoryAdapter
  {
    bool RequiresParameterResolution { get; }

    IReadOnlyList<ParameterInfo> GetParameters();

    object Execute(object[] parameters);
  }
}
