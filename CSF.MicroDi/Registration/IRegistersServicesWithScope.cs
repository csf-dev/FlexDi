using System;
namespace CSF.MicroDi.Registration
{
  public interface IRegistersServicesWithScope : IRegistersServices
  {
    IRegistersServicesWithScope CreateChildScope(IRegistersServices provider);
  }
}
