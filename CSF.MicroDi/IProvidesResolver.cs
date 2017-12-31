using System;
using CSF.MicroDi.Resolution;

namespace CSF.MicroDi
{
  public interface IProvidesResolver
  {
    IFulfilsResolutionRequests GetResolver();
  }
}
