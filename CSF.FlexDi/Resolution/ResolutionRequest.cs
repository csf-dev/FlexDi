//
//    ResolutionRequest.cs
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
namespace CSF.FlexDi.Resolution
{
  /// <summary>
  /// Represents a request to resolve a component.
  /// </summary>
  public class ResolutionRequest
  {
    /// <summary>
    /// Gets the <c>System.Type</c> of the desired service/component.
    /// </summary>
    /// <value>The type of the service.</value>
    public Type ServiceType { get; private set; }

    /// <summary>
    /// Gets an optional registration name to use when resolving the component.
    /// </summary>
    /// <value>The name.</value>
    public string Name { get; private set; }

    /// <summary>
    /// Gets the current resolution path, at the point at which this request was made.
    /// </summary>
    /// <value>The resolution path.</value>
    public ResolutionPath ResolutionPath { get; private set; }

    /// <summary>
    /// Gets a copy of the current resolution request, but omitting the <see cref="Name"/>.
    /// </summary>
    /// <returns>The copied request.</returns>
    public ResolutionRequest GetCopyWithoutName()
    {
      return new ResolutionRequest(ServiceType, null, ResolutionPath);
    }

    /// <summary>
    /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:CSF.FlexDi.Resolution.ResolutionRequest"/>.
    /// </summary>
    /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:CSF.FlexDi.Resolution.ResolutionRequest"/>.</returns>
    public override string ToString()
    {
      var namePart = (Name != null)? $"('{Name}')" : string.Empty;
      return $"[ResolutionRequest: {ServiceType.FullName}{namePart}]";
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:CSF.FlexDi.Resolution.ResolutionRequest"/> class.
    /// </summary>
    /// <param name="serviceType">Service type.</param>
    /// <param name="resolutionPath">Resolution path.</param>
    public ResolutionRequest(Type serviceType, ResolutionPath resolutionPath = null)
      : this(serviceType, null, resolutionPath) {}

    /// <summary>
    /// Initializes a new instance of the <see cref="T:CSF.FlexDi.Resolution.ResolutionRequest"/> class.
    /// </summary>
    /// <param name="serviceType">Service type.</param>
    /// <param name="name">Name.</param>
    /// <param name="resolutionPath">Resolution path.</param>
    public ResolutionRequest(Type serviceType, string name, ResolutionPath resolutionPath = null)
    {
      if(serviceType == null)
        throw new ArgumentNullException(nameof(serviceType));

      ServiceType = serviceType;
      Name = name;
      ResolutionPath = resolutionPath ?? new ResolutionPath();
    }
  }
}
