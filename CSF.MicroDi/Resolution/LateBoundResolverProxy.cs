using System;
using CSF.MicroDi.Registration;

namespace CSF.MicroDi.Resolution
{
  public class LateBoundResolverProxy : ResolverBase
  {
    IResolver proxiedResolver;

    public IResolver ProxiedResolver => proxiedResolver;

    public override IServiceRegistration GetRegistration(ResolutionRequest request)
      => ProxiedResolver.GetRegistration(request);

    public override ResolutionResult Resolve(ResolutionRequest request)
      => ProxiedResolver.Resolve(request);

    public void ProvideProxiedResolver(IResolver proxiedResolver)
    {
      if(proxiedResolver == null)
        throw new ArgumentNullException(nameof(proxiedResolver));
      if(this.proxiedResolver != null)
        throw new InvalidOperationException("The proxied resolver must be set only once.");

      this.proxiedResolver = proxiedResolver;
      proxiedResolver.ServiceResolved += OnServiceResolved;
    }

    void OnServiceResolved(object sender, ServiceResolutionEventArgs args)
    {
      InvokeServiceResolved(sender, args);
    }
  }
}
