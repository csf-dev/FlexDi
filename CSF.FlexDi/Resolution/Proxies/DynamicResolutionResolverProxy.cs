//
//    DynamicResolutionResolverProxy.cs
//
//    Copyright 2018  Craig Fowler
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
  /// A proxying resolver which resolves instances of <see cref="IResolvesServices"/> which have been specified as
  /// dependencies in factories or object constructors.  The resolved instance is marked with the resolution path
  /// up to the current point.
  /// </summary>
  /// <remarks>
  /// <para>
  /// This is important to address circular dependency detection when they make use of a service resolver to dynamically
  /// resolve further dependencies.
  /// </para>
  /// <para>
  /// Imagine service type A which depends upon an <see cref="IResolvesServices"/> in its constructor.  Then, also in
  /// its constructor it makes use of that resolver to resolve service B.  Service B declares an instance of service A
  /// in its constructor.
  /// </para>
  /// <para>
  /// In the example above, we have a circular dependency, but if the service resolver which is resolved to fulfil
  /// the constructor of service A were not 'aware' of its resolution path (IE: "service A") then it would be impossible
  /// to detect the circular dependency and it would lead to a stack overflow exception.
  /// </para>
  /// </remarks>
  public class DynamicRecursionResolverProxy : ProxyingResolver
  {
    /// <summary>
    /// Resolves the given resolution request and returns the result.
    /// </summary>
    /// <param name="request">Request.</param>
    public override ResolutionResult Resolve(ResolutionRequest request)
    {
      if(request.ServiceType != typeof(IResolvesServices) || request.ResolutionPath.IsEmpty)
        return ProxiedResolver.Resolve(request);

      var containerRequest = new ResolutionRequest(typeof(IContainer), request.Name, request.ResolutionPath);
      var result = ProxiedResolver.Resolve(containerRequest);

      if(!result.IsSuccess)
        return result;

      var container = (IContainer) result.ResolvedObject;
      var dynamicContainer = new ServiceResolvingContainerProxy(container, request.ResolutionPath);
      var registration = GetFakeContainerRegistration(dynamicContainer, request.Name);

      return ResolutionResult.Success(request.ResolutionPath, dynamicContainer);
    }

    IServiceRegistration GetFakeContainerRegistration(ServiceResolvingContainerProxy instance, string name)
    {
      return new FactoryRegistration(new Func<IResolvesServices>(() => instance)) {
        ServiceType = typeof(IResolvesServices),
        Name = name,
        Cacheable = false,
        DisposeWithContainer = false,
      };
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:CSF.FlexDi.Resolution.Proxies.DynamicRecursionResolverProxy"/> class.
    /// </summary>
    /// <param name="proxiedResolver">Proxied resolver.</param>
    public DynamicRecursionResolverProxy(IResolver proxiedResolver) : base(proxiedResolver) {}
  }
}
