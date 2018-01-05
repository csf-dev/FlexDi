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
using CSF.MicroDi.Registration;

namespace CSF.MicroDi.Resolution.Proxies
{
  public class DynamicRecursionResolverProxy : ProxyingResolver
  {
    public override ResolutionResult Resolve(ResolutionRequest request)
    {
      if(request.ServiceType != typeof(IResolvesServices) || request.ResolutionPath.IsEmpty)
        return ProxiedResolver.Resolve(request);

      var containerRequest = new ResolutionRequest(typeof(IContainer), request.Name, request.ResolutionPath);
      var result = ProxiedResolver.Resolve(containerRequest);

      if(!result.IsSuccess)
        return result;

      var container = (IContainer) result.ResolvedObject;
      if(container is ServiceResolvingContainerProxy)
        return result;

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

    public DynamicRecursionResolverProxy(IResolver proxiedResolver) : base(proxiedResolver) {}
  }
}
