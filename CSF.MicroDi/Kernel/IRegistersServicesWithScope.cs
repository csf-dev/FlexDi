using System;
namespace CSF.MicroDi.Kernel
{
  public interface IRegistersServicesWithScope : IRegistersServices
  {
    IRegistersServicesWithScope CreateChildScope(IRegistersServices provider);
  }
}
