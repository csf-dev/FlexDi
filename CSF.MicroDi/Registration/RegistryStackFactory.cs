using System;
using System.Collections.Generic;

namespace CSF.MicroDi.Registration
{
  public class RegistryStackFactory
  {
    public IServiceRegistrationProvider CreateRegistryStack(IProvidesResolutionInfo resolutionInfo)
    {
      if(resolutionInfo == null)
        throw new ArgumentNullException(nameof(resolutionInfo));
      
      var providers = new List<IServiceRegistrationProvider>();
      var currentResolutionInfo = resolutionInfo;

      while(currentResolutionInfo != null)
      {
        providers.Add(currentResolutionInfo.Registry);
        currentResolutionInfo = currentResolutionInfo.Parent;
      }

      return new StackOfRegistriesRegistrationProvider(providers.ToArray());
    }
  }
}
