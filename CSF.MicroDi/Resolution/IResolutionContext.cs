using System;
namespace CSF.MicroDi.Resolution
{
  public interface IResolutionContext
  {
    object Resolve(IFactoryAdapter factory);

    object Resolve(ResolutionRequest request);
  }
}
