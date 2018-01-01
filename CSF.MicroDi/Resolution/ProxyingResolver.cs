using System;
using CSF.MicroDi.Registration;

namespace CSF.MicroDi.Resolution
{
  public abstract class ProxyingResolver : ResolverBase
  {
    readonly IResolver proxiedResolver;

    public IResolver ProxiedResolver => proxiedResolver;

    public override IServiceRegistration GetRegistration(ResolutionRequest request)
      => ProxiedResolver.GetRegistration(request);

    void OnServiceResolved(object sender, ServiceResolutionEventArgs args)
    {
      InvokeServiceResolved(sender, args);
    }

    public ProxyingResolver(IResolver proxiedResolver)
    {
      if(proxiedResolver == null)
        throw new ArgumentNullException(nameof(proxiedResolver));

      this.proxiedResolver = proxiedResolver;
      proxiedResolver.ServiceResolved += OnServiceResolved;
    }
  }
}
