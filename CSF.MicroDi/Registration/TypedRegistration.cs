using System;
namespace CSF.MicroDi.Registration
{
  public abstract class TypedRegistration : ServiceRegistration
  {
    public abstract Type ImplementationType { get; }
  }
}
