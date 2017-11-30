using System;
namespace CSF.MicroDi.Kernel
{
  public abstract class TypedRegistration : ServiceRegistration
  {
    public abstract Type ImplementationType { get; }
  }
}
