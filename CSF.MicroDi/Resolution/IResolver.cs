using System;
using CSF.MicroDi.Registration;

namespace CSF.MicroDi.Resolution
{
  public interface IResolver : IResolutionContext
  {
    IServiceRegistration GetRegistration(ResolutionRequest request);
  }
}
