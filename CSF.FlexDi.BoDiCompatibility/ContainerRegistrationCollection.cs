//
//    ContainerRegistrationCollection.cs
//
//    Copyright 2018  Craig Fowler et al
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//
//    For further copyright info, including a complete author/contributor
//    list, please refer to the file NOTICE.txt

#if !BODI_LIMITEDRUNTIME && !BODI_DISABLECONFIGFILESUPPORT
using System;
using System.Configuration;

namespace BoDi
{
  /// <summary>
  /// A configuration collection type which represents the configured DI registrations.
  /// </summary>
  public class ContainerRegistrationCollection : ConfigurationElementCollection
  {
    /// <summary>
    /// Creates a the new configuration element.
    /// </summary>
    /// <returns>The new element.</returns>
    protected override ConfigurationElement CreateNewElement()
    {
      return new ContainerRegistrationConfigElement();
    }

    /// <summary>
    /// Gets the key (a unique identifier for an element) for the specified element.
    /// </summary>
    /// <returns>The element key.</returns>
    /// <param name="element">Element.</param>
    protected override object GetElementKey(ConfigurationElement element)
    {
      var registrationConfigElement = ((ContainerRegistrationConfigElement)element);
      string elementKey = registrationConfigElement.Interface;
      if (registrationConfigElement.Name != null)
        elementKey = elementKey + "/" + registrationConfigElement.Name;
      return elementKey;
    }

    /// <summary>
    /// Adds a new configuration element with the specified implementation type, interface type and name.
    /// </summary>
    /// <param name="implementationType">Implementation type.</param>
    /// <param name="interfaceType">Interface type.</param>
    /// <param name="name">Name.</param>
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