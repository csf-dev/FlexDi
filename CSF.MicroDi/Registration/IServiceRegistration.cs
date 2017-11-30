using System;
using CSF.MicroDi.Resolution;

namespace CSF.MicroDi.Registration
{
  public interface IServiceRegistration
  {
    Multiplicity Multiplicity { get; }

    Type ServiceType { get; }

    string Name { get; }

    object CreateInstance(IResolutionContext context);
  }
}
