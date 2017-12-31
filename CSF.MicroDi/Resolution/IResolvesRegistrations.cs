using System;
using CSF.MicroDi.Registration;

namespace CSF.MicroDi.Resolution
{
  public interface IResolvesRegistrations
  {
    ResolutionResult Resolve(ResolutionRequest request, IServiceRegistration registration);
  }
}
