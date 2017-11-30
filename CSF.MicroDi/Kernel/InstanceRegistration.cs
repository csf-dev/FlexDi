using System;
namespace CSF.MicroDi.Kernel
{
  public class InstanceRegistration : TypedRegistration
  {
    readonly object implementation;

    public override Type ImplementationType => implementation.GetType();

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

    public override object CreateInstance(IResolutionContext context)
    {
      return implementation;
    }

    public InstanceRegistration(object implementation)
    {
      if(implementation == null)
        throw new ArgumentNullException(nameof(implementation));

      this.implementation = implementation;
    }
  }
}
