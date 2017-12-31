using System;
using CSF.MicroDi.Registration;

namespace CSF.MicroDi.Resolution
{
  public interface IDetectsCircularDependencies
  {
    bool HasCircularDependency(IServiceRegistration registration, ResolutionPath resolutionPath);
    void ThrowOnCircularDependency(IServiceRegistration registration, ResolutionPath resolutionPath);
  }
}
