using System;
namespace CSF.MicroDi.Resolution
{
  public interface IFulfilsResolutionRequests
  {
    bool Resolve(ResolutionRequest request, out object output);
  }
}
