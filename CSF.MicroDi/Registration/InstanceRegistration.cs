using System;
using CSF.MicroDi.Resolution;

namespace CSF.MicroDi.Registration
{
  public class InstanceRegistration : TypedRegistration
  {
    readonly object implementation;

    public virtual object Implementation => implementation;

    public override Type ImplementationType => Implementation.GetType();

    public override Multiplicity Multiplicity
    {
      get {
        return base.Multiplicity;
      }
      set {
        if(value != Multiplicity.Shared)
          throw new ArgumentException($"The only multiplicity supported by {nameof(InstanceRegistration)} is {Multiplicity.Shared.ToString()}.");
        
        base.Multiplicity = value;
      }
    }

    public override int Priority => 2;

    public override IFactoryAdapter GetFactoryAdapter(ResolutionRequest request) => new InstanceFactory(implementation);

    public override string ToString()
    {
      return $"[Instance registration for `{ServiceType.FullName}', using an instance of `{ImplementationType.FullName}']";
    }

    public InstanceRegistration(object implementation)
    {
      if(implementation == null)
        throw new ArgumentNullException(nameof(implementation));

      this.implementation = implementation;
      SetMultiplicity(Multiplicity.Shared);
    }
  }
}
