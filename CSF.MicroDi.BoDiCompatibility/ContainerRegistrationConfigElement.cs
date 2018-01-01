#if !BODI_LIMITEDRUNTIME && !BODI_DISABLECONFIGFILESUPPORT
using System;
using System.Configuration;

namespace BoDi
{
  public class ContainerRegistrationConfigElement : ConfigurationElement
  {
    const string ServiceType = "as", ImplementationType = "type", RegistrationName = "name";

    [ConfigurationProperty(ServiceType, IsRequired = true)]
    public string Interface
    {
      get { return (string) this[ServiceType]; }
      set { this[ServiceType] = value; }
    }

    [ConfigurationProperty(ImplementationType, IsRequired = true)]
    public string Implementation
    {
      get { return (string) this[ImplementationType]; }
      set { this[ImplementationType] = value; }
    }

    [ConfigurationProperty(RegistrationName, IsRequired = false, DefaultValue = null)]
    public string Name
    {
      get { return (string) this[RegistrationName]; }
      set { this[RegistrationName] = value; }
    }
  }
}
#endif