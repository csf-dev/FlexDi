//
//    CircularDependencyPreventingResolverProxy.cs
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
namespace CSF.FlexDi.Resolution.Proxies
{
  /// <summary>
  /// A proxying resolver which raises exceptions when circular dependencies are detected.
  /// </summary>
  public class CircularDependencyPreventingResolverProxy : ProxyingResolver
  {
    readonly IDetectsCircularDependencies circularDependencyDetector;

    /// <summary>
    /// Resolves the given resolution request and returns the result.
    /// </summary>
    /// <param name="request">Request.</param>
    public override ResolutionResult Resolve(ResolutionRequest request)
    {
      var registration = GetRegistration(request);

      if(registration != null)
        circularDependencyDetector.ThrowOnCircularDependency(registration, request.ResolutionPath);

      return ProxiedResolver.Resolve(request);
    }

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="CSF.FlexDi.Resolution.Proxies.CircularDependencyPreventingResolverProxy"/> class.
    /// </summary>
    /// <param name="proxiedResolver">Proxied resolver.</param>
    /// <param name="circularDependencyDetector">Circular dependency detector.</param>
    public CircularDependencyPreventingResolverProxy(IResolver proxiedResolver,
                                                     IDetectsCircularDependencies circularDependencyDetector)
      : base(proxiedResolver)
    {
      if(circularDependencyDetector == null)
        throw new ArgumentNullException(nameof(circularDependencyDetector));
      
      this.circularDependencyDetector = circularDependencyDetector;
    }
  }
}
