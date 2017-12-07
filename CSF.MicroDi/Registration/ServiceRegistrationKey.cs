using System;
using CSF.MicroDi.Resolution;

namespace CSF.MicroDi.Registration
{
  public class ServiceRegistrationKey : IEquatable<ServiceRegistrationKey>
  {
    public Type ServiceType { get; private set; }

    public string Name { get; private set; }

    public override bool Equals(object obj)
    {
      return Equals(obj as ServiceRegistrationKey);
    }

    public bool Equals(ServiceRegistrationKey other)
    {
      if(ReferenceEquals(other, null))
        return false;
      if(ReferenceEquals(other, this))
        return true;

      return (other.ServiceType == ServiceType
              && other.Name == Name);
    }

    public override int GetHashCode()
    {
      var typeHash = ServiceType.GetHashCode();
      var nameHash = Name?.GetHashCode() ?? 0;

      return typeHash ^ nameHash;
    }

    public ServiceRegistrationKey(Type serviceType, string name)
    {
      if(serviceType == null)
        throw new ArgumentNullException(nameof(serviceType));

      ServiceType = serviceType;
      Name = name;
    }

    public static ServiceRegistrationKey ForRegistration(IServiceRegistration registration)
    {
      if(registration == null)
        throw new ArgumentNullException(nameof(registration));

      return new ServiceRegistrationKey(registration.ServiceType, registration.Name);
    }

    public static ServiceRegistrationKey FromRequest(ResolutionRequest request)
    {
      if(request == null)
        throw new ArgumentNullException(nameof(request));

      return new ServiceRegistrationKey(request.ServiceType, request.Name);
    }
  }
}
