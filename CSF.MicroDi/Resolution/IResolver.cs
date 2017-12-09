using System;
using CSF.MicroDi.Registration;

namespace CSF.MicroDi.Resolution
{
  public interface IResolver : IFulfilsResolutionRequests
  {
    ResolutionPath ResolutionPath { get; }

    IServiceRegistration GetRegistration(ResolutionRequest request);

    IResolver CreateChild(IServiceRegistration registration);
  }
}
