using System;
using System.Reflection;

namespace CSF.MicroDi.Resolution
{
  public interface ISelectsConstructor
  {
    ConstructorInfo SelectConstructor(Type type);
  }
}
