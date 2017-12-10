using System;
namespace CSF.MicroDi.Registration
{
  public abstract class TypedRegistration : ServiceRegistration
  {
    public abstract Type ImplementationType { get; }

    public override bool MatchesKey(ServiceRegistrationKey key)
    {
      if(base.MatchesKey(key))
        return true;
      
      if(key == null)
        return false;

      return key.ServiceType.IsAssignableFrom(ImplementationType) && Name == key.Name;
    }
  }
}
