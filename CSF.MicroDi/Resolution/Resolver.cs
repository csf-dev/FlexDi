using System;
using System.Linq;
using System.Reflection;
using CSF.MicroDi.Registration;

namespace CSF.MicroDi.Resolution
{
  public class Resolver : IResolutionContext
  {
    readonly IServiceRegistrationProvider registrationProvider;

    public virtual object Resolve(ResolutionRequest request)
    {
      if(request == null)
        throw new ArgumentNullException(nameof(request));
      
      var registration = GetRegistration(request);
      return registration.CreateInstance(this);
    }

    public virtual object Resolve(IFactoryAdapter factory)
    {
      if(factory == null)
        throw new ArgumentNullException(nameof(factory));

      var parameters = factory.GetParameters();
      var resolvedParameters = parameters
        .Select(ConvertToResolutionRequest)
        .Select(Resolve)
        .ToArray();

      return factory.Execute(resolvedParameters);
    }

    protected virtual IServiceRegistration GetRegistration(ResolutionRequest request)
    {
      if(request == null)
        throw new ArgumentNullException(nameof(request));

      IServiceRegistration output;

      output = registrationProvider.Get(request);
      if(output != null)
        return output;

      output = GetRegistrationWithoutName(request);
      if(output != null)
        return output;

      return GetFallbackRegistration(request);
    }

    protected virtual IServiceRegistration GetRegistrationWithoutName(ResolutionRequest request)
    {
      var requestWithoutName = request.WithoutName();
      return registrationProvider.Get(requestWithoutName);
    }

    protected virtual IServiceRegistration GetFallbackRegistration(ResolutionRequest request)
    {
      return new TypeRegistration(request.ServiceType) {
        Name = request.Name,
        ServiceType = request.ServiceType,
        Multiplicity = Multiplicity.Shared
      };
    }

    protected virtual ResolutionRequest ConvertToResolutionRequest(ParameterInfo parameter)
    {
      if(parameter == null)
        throw new ArgumentNullException(nameof(parameter));
      
      return new ResolutionRequest(parameter.ParameterType, parameter.Name);
    }
  }
}
