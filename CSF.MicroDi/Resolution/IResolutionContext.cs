using System;
namespace CSF.MicroDi.Resolution
{
  public interface IResolutionContext : IFulfilsResolutionRequests
  {
    object Resolve(IFactoryAdapter factory);
  }
}
