using System;
using System.Collections.Generic;
using System.Reflection;

namespace CSF.MicroDi.Resolution
{
  public interface IFactoryAdapter
  {
    IReadOnlyList<ParameterInfo> GetParameters();

    object Execute(object[] parameters);
  }
}
