using System;
using CSF.MicroDi.Registration;
using CSF.MicroDi.Resolution;

namespace CSF.MicroDi
{
  public interface IProvidesResolutionInfo
  {
    ICachesResolvedServiceInstances Cache { get; }

    IRegistersServices Registry { get; }

    ContainerOptions Options { get; }

    IProvidesResolutionInfo Parent { get; }
  }
}
