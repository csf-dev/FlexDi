using System;
namespace CSF.MicroDi.Resolution
{
  public class CircularDependencyPreventingResolverProxy : ProxyingResolver
  {
    readonly IDetectsCircularDependencies circularDependencyDetector;

    public override ResolutionResult Resolve(ResolutionRequest request)
    {
      var registration = GetRegistration(request);
      circularDependencyDetector.ThrowOnCircularDependency(registration, request.ResolutionPath);

      return ProxiedResolver.Resolve(request);
    }

    public CircularDependencyPreventingResolverProxy(IResolver proxiedResolver,
                                                     IDetectsCircularDependencies circularDependencyDetector)
      : base(proxiedResolver)
    {
      this.circularDependencyDetector = circularDependencyDetector?? new CircularDependencyDetector();
    }
  }
}
