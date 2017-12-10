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

    public virtual int Priority => 1;

    public abstract  IFactoryAdapter GetFactoryAdapter(ResolutionRequest request);

    public virtual void AssertIsValid()
    { /* Intentional no-op, derived types may override to perform validation logic */ }

    public virtual bool MatchesKey(ServiceRegistrationKey key)
    {
      if(key == null)
        return false;

      return ServiceType == key.ServiceType && Name == key.Name;
    }

    protected void SetMultiplicity(Multiplicity multiplicity) => this.multiplicity = multiplicity;

    public ServiceRegistration()
    {
      multiplicity = Multiplicity.Shared;
      disposeWithContainer = true;
    }
  }
}
