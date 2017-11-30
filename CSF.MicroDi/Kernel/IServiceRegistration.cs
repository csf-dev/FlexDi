using System;
namespace CSF.MicroDi.Kernel
{
  public interface IServiceRegistration
  {
    Multiplicity Multiplicity { get; }

    Type ServiceType { get; }

    string Name { get; }

    object CreateInstance(IResolutionContext context);
  }
}
