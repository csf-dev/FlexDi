using System;
using CSF.MicroDi.Registration;

namespace CSF.MicroDi.Resolution
{
  public class UnregisteredServiceResolverProxy : ProxyingResolver
  {
    readonly IServiceRegistrationProvider unregisteredRegistrationProvider;
    readonly IResolvesRegistrations registrationResolver;

    public override ResolutionResult Resolve(ResolutionRequest request)
    {
      var output = ProxiedResolver.Resolve(request);
      if(output.IsSuccess)
        return output;

      var registration = unregisteredRegistrationProvider.Get(request);
      return registrationResolver.Resolve(request, registration);
    }

    public override IServiceRegistration GetRegistration(ResolutionRequest request)
    {
      var registration = base.GetRegistration(request);
      if(registration != null)
        return registration;

      return unregisteredRegistrationProvider.Get(request);
    }

    public UnregisteredServiceResolverProxy(IResolver proxiedResolver,
                                            IResolvesRegistrations registrationResolver,
                                            IServiceRegistrationProvider unregisteredRegistrationProvider = null)
      : base(proxiedResolver)
    {
      if(registrationResolver == null)
        throw new ArgumentNullException(nameof(registrationResolver));

      this.registrationResolver = registrationResolver;
      this.unregisteredRegistrationProvider = unregisteredRegistrationProvider ?? new ServiceWithoutRegistrationProvider();
    }
  }
}
