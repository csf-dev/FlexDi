#if !BODI_LIMITEDRUNTIME && !BODI_DISABLECONFIGFILESUPPORT
using System;
using System.Configuration;

namespace BoDi
{
  public class BoDiConfigurationSection : ConfigurationSection
  {
    const string CollectionName = "", AddItemKey = "register";

    [ConfigurationProperty(CollectionName, Options = ConfigurationPropertyOptions.IsDefaultCollection)]
    [ConfigurationCollection(typeof(ContainerRegistrationCollection), AddItemName = AddItemKey)]
    public ContainerRegistrationCollection Registrations
    {
      get { return (ContainerRegistrationCollection) this[CollectionName]; }
      set { this[CollectionName] = value; }
    }
  }
}
#endif
