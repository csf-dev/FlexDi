#if !BODI_LIMITEDRUNTIME && !BODI_DISABLECONFIGFILESUPPORT
using System;
using System.Configuration;

namespace BoDi
{
  public class ContainerRegistrationCollection : ConfigurationElementCollection
  {
    protected override ConfigurationElement CreateNewElement()
    {
      return new ContainerRegistrationConfigElement();
    }

    protected override object GetElementKey(ConfigurationElement element)
    {
      var registrationConfigElement = ((ContainerRegistrationConfigElement)element);
      string elementKey = registrationConfigElement.Interface;
      if (registrationConfigElement.Name != null)
        elementKey = elementKey + "/" + registrationConfigElement.Name;
      return elementKey;
    }

    public void Add(string implementationType, string interfaceType, string name = null)
    {
      BaseAdd(new ContainerRegistrationConfigElement
      {
        Implementation = implementationType,
        Interface = interfaceType,
        Name = name
      });
    }
  }
}
#endif