//
//    ContainerRegistrationConfigElement.cs
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
  /// A configuration element representing a single service registration.
  /// </summary>
  public class ContainerRegistrationConfigElement : ConfigurationElement
  {
    const string ServiceType = "as", ImplementationType = "type", RegistrationName = "name";

    /// <summary>
    /// Gets or sets the interface/service/component type registered by this element.
    /// </summary>
    /// <value>The interface.</value>
    [ConfigurationProperty(ServiceType, IsRequired = true)]
    public string Interface
    {
      get { return (string) this[ServiceType]; }
      set { this[ServiceType] = value; }
    }

    /// <summary>
    /// Gets or sets the concrete implementation type for this element.
    /// </summary>
    /// <value>The implementation.</value>
    [ConfigurationProperty(ImplementationType, IsRequired = true)]
    public string Implementation
    {
      get { return (string) this[ImplementationType]; }
      set { this[ImplementationType] = value; }
    }

    /// <summary>
    /// Gets or sets an optional registration name for this element.
    /// </summary>
    /// <value>The name.</value>
    [ConfigurationProperty(RegistrationName, IsRequired = false, DefaultValue = null)]
    public string Name
    {
      get { return (string) this[RegistrationName]; }
      set { this[RegistrationName] = value; }
    }
  }
}
#endif