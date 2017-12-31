using System;
using CSF.MicroDi.Registration;

namespace CSF.MicroDi.Resolution
{
  public interface IResolver : IFulfilsResolutionRequests
  {
    IServiceRegistration GetRegistration(ResolutionRequest request);
  }
}
