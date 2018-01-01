using System;
namespace CSF.MicroDi.Resolution
{
  public class FallbackResolverProxy : ProxyingResolver
  {
    readonly IResolver fallbackResolver;

    public override ResolutionResult Resolve(ResolutionRequest request)
    {
      var output = ProxiedResolver.Resolve(request);
      if(output.IsSuccess)
        return output;

      return fallbackResolver.Resolve(request);
    }

    public override Registration.IServiceRegistration GetRegistration(ResolutionRequest request)
    {
      return ProxiedResolver.GetRegistration(request) ?? fallbackResolver.GetRegistration(request);
    }

    void OnServiceResolved(object sender, ServiceResolutionEventArgs args)
    {
      InvokeServiceResolved(sender, args);
    }

    public FallbackResolverProxy(IResolver proxiedResolver, IResolver fallbackResolver) : base(proxiedResolver)
    {
      if(fallbackResolver == null)
        throw new ArgumentNullException(nameof(fallbackResolver));
      
      this.fallbackResolver = fallbackResolver;
      fallbackResolver.ServiceResolved += OnServiceResolved;
    }
  }
}
