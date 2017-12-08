using System;
namespace CSF.MicroDi
{
  public interface IContainer : IResolvesServices, IReceivesRegistrations, IDisposable
  {
  }
}
