using System;
using System.Reflection;

namespace CSF.MicroDi.Kernel
{
  public interface ISelectsConstructor
  {
    ConstructorInfo SelectConstructor(Type type);
  }
}
