//
//    RegisteredNameInjectingResolverProxy.cs
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

namespace CSF.FlexDi.Resolution.Proxies
{
  /// <summary>
  /// A proxying resolver which resolves a special string value.  If a string parameter is included in an object constructor
  /// or a factory, and that parameter is named <c>registeredName</c>, then it will be resolved using the
  /// <see cref="IServiceRegistration.Name"/> of the service registration which was used to resolve the component which
  /// declared that string as a dependency.
  /// </summary>
  public class RegisteredNameInjectingResolverProxy : ProxyingResolver
  {
    const string RegisteredName = "registeredName";

    /// <summary>
    /// Resolves the given resolution request and returns the result.
    /// </summary>
    /// <param name="request">Request.</param>
    public override ResolutionResult Resolve(ResolutionRequest request)
    {
      if(request == null)
        throw new ArgumentNullException(nameof(request));

      if(request.ServiceType != typeof(string) || request.Name != RegisteredName)
        return ProxiedResolver.Resolve(request);

      var currentResolutionPath = request.ResolutionPath;
      var resolvedName = currentResolutionPath.CurrentRegistration.Name;

      var childResolutionPath = CreateRegisteredNameResolutionPath(currentResolutionPath, resolvedName);
      return ResolutionResult.Success(childResolutionPath, resolvedName);
    }

    ResolutionPath CreateRegisteredNameResolutionPath(ResolutionPath parentPath, string name)
    {
      var registration = new InstanceRegistration(name) {
        Name = RegisteredName,
        ServiceType = typeof(string),
      };
      return parentPath.CreateChild(registration);
    }

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="T:CSF.FlexDi.Resolution.Proxies.RegisteredNameInjectingResolverProxy"/> class.
    /// </summary>
    /// <param name="proxiedResolver">Proxied resolver.</param>
    public RegisteredNameInjectingResolverProxy(IResolver proxiedResolver) : base(proxiedResolver) {}
  }
}
