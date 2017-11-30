using System;
using CSF.MicroDi.Resolution;

namespace CSF.MicroDi.Registration
{
  public abstract class ServiceRegistration : IServiceRegistration
  {
    public virtual Multiplicity Multiplicity { get; set; }

    public virtual string Name { get; set; }

    public virtual Type ServiceType { get; set; }

    public abstract object CreateInstance(IResolutionContext context);
  }
}
