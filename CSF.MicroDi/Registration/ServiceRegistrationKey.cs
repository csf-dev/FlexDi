using System;
using CSF.MicroDi.Resolution;

namespace CSF.MicroDi.Registration
{
  public class ServiceRegistrationKey
  {
    public Type ServiceType { get; private set; }

    public string Name { get; private set; }

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
