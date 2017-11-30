using System;
using System.Reflection;

namespace CSF.MicroDi.Kernel
{
  public class ConstructorWithMostParametersSelector : ISelectsConstructor
  {
    public ConstructorInfo SelectConstructor(Type type)
    {
      throw new NotImplementedException();
    }
  }
}
