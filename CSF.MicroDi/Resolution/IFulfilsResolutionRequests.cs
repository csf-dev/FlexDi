using System;
namespace CSF.MicroDi.Resolution
{
  public interface IFulfilsResolutionRequests
  {
    ResolutionResult Resolve(ResolutionRequest request);

    event EventHandler<ServiceResolutionEventArgs> ServiceResolved;
  }
}
