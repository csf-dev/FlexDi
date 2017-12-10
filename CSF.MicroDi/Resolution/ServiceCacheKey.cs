using System;
using CSF.MicroDi.Registration;

namespace CSF.MicroDi.Resolution
{
  public class ServiceCacheKey : IEquatable<ServiceCacheKey>
  {
    public Type ImplementationType { get; private set; }

    public string Name { get; private set; }

    public override bool Equals(object obj)
    {
      return Equals(obj as ServiceCacheKey);
    }

    public bool Equals(ServiceCacheKey other)
    {
      if(ReferenceEquals(other, null))
        return false;
      if(ReferenceEquals(other, this))
        return true;

      return (other.ImplementationType == ImplementationType
              && other.Name == Name);
    }

    public override int GetHashCode()
    {
      var typeHash = ImplementationType.GetHashCode();
      var nameHash = Name?.GetHashCode() ?? 0;

      return typeHash ^ nameHash;
    }

    public ServiceCacheKey(Type implementationType, string name)
    {
      if(implementationType == null)
        throw new ArgumentNullException(nameof(implementationType));

      ImplementationType = implementationType;
      Name = name;
    }

    public static ServiceCacheKey FromRegistrationKeyAndInstance(ServiceRegistrationKey key, object instance)
    {
      if(key == null)
        throw new ArgumentNullException(nameof(key));

      if(ReferenceEquals(instance, null))
        return FromRegistrationKey(key);

      return new ServiceCacheKey(instance.GetType(), key.Name);
    }

    public static ServiceCacheKey FromRegistrationKey(ServiceRegistrationKey key)
    {
      if(key == null)
        throw new ArgumentNullException(nameof(key));

      return new ServiceCacheKey(key.ServiceType, key.Name);
    }
  }
}
