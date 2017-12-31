using System;
using CSF.MicroDi.Registration;

namespace CSF.MicroDi.Resolution
{
  public class CircularDependencyDetector : IDetectsCircularDependencies
  {
    public bool HasCircularDependency(IServiceRegistration registration, ResolutionPath resolutionPath)
    {
      if(resolutionPath == null)
        throw new ArgumentNullException(nameof(resolutionPath));
      if(registration == null)
        throw new ArgumentNullException(nameof(registration));

      return resolutionPath.Contains(registration);
    }

    public void ThrowOnCircularDependency(IServiceRegistration registration, ResolutionPath resolutionPath)
    {
      if(HasCircularDependency(registration, resolutionPath))
      {
        throw new CircularDependencyException("Circular dependency detected; this is not supported.")
        { ResolutionPath = resolutionPath };
      }
    }
  }
}
