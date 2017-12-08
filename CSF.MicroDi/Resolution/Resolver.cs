using System;
using System.Linq;
using System.Reflection;
using CSF.MicroDi.Registration;

namespace CSF.MicroDi.Resolution
{
  public class Resolver : IResolutionContext
  {
    readonly IServiceRegistrationProvider registrationProvider, unregisteredServiceProvider;

    public virtual bool Resolve(ResolutionRequest request, out object output)
    {
      if(request == null)
        throw new ArgumentNullException(nameof(request));

      output = null;
      var registration = GetRegistration(request);
      if(registration == null)
        return false;
      
      output = CreateInstance(registration);
      return true;
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

    protected virtual object Resolve(ResolutionRequest request)
    {
      object output;
      Resolve(request, out output);
      return output;
    }

    protected virtual object CreateInstance(IServiceRegistration registration)
    {
      if(registration == null)
        throw new ArgumentNullException(nameof(registration));
      
      return registration.CreateInstance(this);
    }

    protected virtual IServiceRegistration GetRegistration(ResolutionRequest request)
    {
      if(request == null)
        throw new ArgumentNullException(nameof(request));

      if(registrationProvider.CanFulfilRequest(request))
        return registrationProvider.Get(request);

      var requestWithoutName = request.GetCopyWithoutName();
      if(registrationProvider.CanFulfilRequest(requestWithoutName))
        return registrationProvider.Get(requestWithoutName);

      return unregisteredServiceProvider.Get(request);
    }

    protected virtual ResolutionRequest ConvertToResolutionRequest(ParameterInfo parameter)
    {
      if(parameter == null)
        throw new ArgumentNullException(nameof(parameter));
      
      return new ResolutionRequest(parameter.ParameterType, parameter.Name);
    }

    public Resolver(IServiceRegistrationProvider registrationProvider) : this(registrationProvider, null) {}

    public Resolver(IServiceRegistrationProvider registrationProvider,
                    IServiceRegistrationProvider unregisteredServiceProvider)
    {
      if(registrationProvider == null)
        throw new ArgumentNullException(nameof(registrationProvider));

      this.registrationProvider = registrationProvider;
      this.unregisteredServiceProvider = unregisteredServiceProvider?? new ServiceWithoutRegistrationProvider();
    }
  }
}
