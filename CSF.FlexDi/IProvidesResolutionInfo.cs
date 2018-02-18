//
//    IProvidesResolutionInfo.cs
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

using System;
using CSF.FlexDi.Registration;
using CSF.FlexDi.Resolution;

namespace CSF.FlexDi
{
  /// <summary>
  /// Represents an object which provides sufficient information from which to perform component resolution.
  /// </summary>
  /// <remarks>
  /// This is a 'framework' type and unless you are extending FlexDi, it is unlikely you will need to make use of this.
  /// </remarks>
  public interface IProvidesResolutionInfo
  {
    /// <summary>
    /// Gets the cache of resolved services.
    /// </summary>
    /// <value>The cache.</value>
    ICachesResolvedServiceInstances Cache { get; }

    /// <summary>
    /// Gets the registry (the service registrations).
    /// </summary>
    /// <value>The registry.</value>
    IRegistersServices Registry { get; }

    /// <summary>
    /// Gets the options used to construct the current container instance.
    /// </summary>
    /// <value>The options.</value>
    ContainerOptions Options { get; }

    /// <summary>
    /// Gets a reference to the resolution information for the parent container (if one exists).
    /// </summary>
    /// <value>The parent resolution info.</value>
    IProvidesResolutionInfo Parent { get; }

    /// <summary>
    /// Gets a service which is used to select constructors for instantiating new objects.
    /// </summary>
    /// <value>The constructor selector.</value>
    ISelectsConstructor ConstructorSelector { get; }
  }
}
