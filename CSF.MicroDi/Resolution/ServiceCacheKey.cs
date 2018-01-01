using System;
using System.Collections.Generic;
using System.Linq;
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

    public static IReadOnlyCollection<ServiceCacheKey> CreateFromRegistrationKeyAndInstance(ServiceRegistrationKey key, object instance)
    {
      if(key == null)
        throw new ArgumentNullException(nameof(key));

      var fromRegistrationOnly = CreateFromRegistrationKey(key);
      if(ReferenceEquals(instance, null))
        return fromRegistrationOnly;

      return fromRegistrationOnly
        .Union(new [] { new ServiceCacheKey(instance.GetType(), key.Name) })
        .ToArray();
    }

    public static IReadOnlyCollection<ServiceCacheKey> CreateFromRegistrationKey(ServiceRegistrationKey key)
    {
      if(key == null)
        throw new ArgumentNullException(nameof(key));

      return new[] { new ServiceCacheKey(key.ServiceType, key.Name) };
    }
  }
}
