using System;
namespace CSF.MicroDi.Kernel
{
  public interface IResolutionContext
  {
    object Resolve(IFactoryAdapter factory);
  }
}
