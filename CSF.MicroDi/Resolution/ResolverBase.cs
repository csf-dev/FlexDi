using System;
using CSF.MicroDi.Registration;

namespace CSF.MicroDi.Resolution
{
  public abstract class ResolverBase : IResolver
  {
    public abstract IServiceRegistration GetRegistration(ResolutionRequest request);

    public abstract ResolutionResult Resolve(ResolutionRequest request);

    public event EventHandler<ServiceResolutionEventArgs> ServiceResolved;

    protected virtual void InvokeServiceResolved(IServiceRegistration registration, object instance)
    {
      var args = new ServiceResolutionEventArgs(registration, instance);
      InvokeServiceResolved(args);
    }

    protected virtual void InvokeServiceResolved(object sender, ServiceResolutionEventArgs args)
    {
      ServiceResolved?.Invoke(sender, args);
    }

    protected virtual void InvokeServiceResolved(ServiceResolutionEventArgs args)
    {
      ServiceResolved?.Invoke(this, args);
    }
  }
}
