using System;
using System.Collections.Generic;
using CSF.MicroDi.Resolution;

namespace CSF.MicroDi.Registration
{
  public class ServiceWithoutRegistrationProvider : IServiceRegistrationProvider
  {
    public virtual bool CanFulfilRequest(ResolutionRequest request)
    {
      if(request == null)
        throw new ArgumentNullException(nameof(request));

      return true;
    }

    public virtual IServiceRegistration Get(ResolutionRequest request)
    {
      if(request == null)
        throw new ArgumentNullException(nameof(request));
      
      return new TypeRegistration(request.ServiceType) {
        Name = request.Name,
        ServiceType = request.ServiceType,
        Multiplicity = Multiplicity.Shared
      };
    }

    public virtual IReadOnlyCollection<IServiceRegistration> GetAll(Type serviceType)
    {
      if(serviceType == null)
        throw new ArgumentNullException(nameof(serviceType));

      return new [] { Get(new ResolutionRequest(serviceType, null)) };
    }

    public virtual IReadOnlyCollection<IServiceRegistration> GetAll()
    {
      throw new NotSupportedException($"This type does not support use of {nameof(GetAll)} with no parameters.");
    }

    bool IServiceRegistrationProvider.HasRegistration(ServiceRegistrationKey key)
    {
      if(key == null)
        throw new ArgumentNullException(nameof(key));
      
      return true;
    }

    bool IServiceRegistrationProvider.HasRegistration(IServiceRegistration registration)
    {
      if(registration == null)
        throw new ArgumentNullException(nameof(registration));

      return true;
    }
  }
}
