using System;
using CSF.MicroDi.Resolution;

namespace CSF.MicroDi.Registration
{
  public abstract class ServiceRegistration : IServiceRegistration
  {
    Multiplicity multiplicity;
    bool disposeWithContainer;

    public virtual Multiplicity Multiplicity
    {
      get { return multiplicity; }
      set { multiplicity = value; }
    }

    public virtual string Name { get; set; }

    public virtual Type ServiceType { get; set; }

    public virtual bool DisposeWithContainer
    {
      get { return disposeWithContainer; }
      set { disposeWithContainer = value; }
    }

    public abstract object CreateInstance(IResolutionContext context);

    protected void SetMultiplicity(Multiplicity multiplicity) => this.multiplicity = multiplicity;

    public ServiceRegistration()
    {
      multiplicity = Multiplicity.Shared;
      disposeWithContainer = true;
    }
  }
}
