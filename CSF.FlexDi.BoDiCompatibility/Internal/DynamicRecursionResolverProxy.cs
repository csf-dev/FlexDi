//
//    DynamicRecursionResolverProxy.cs
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

namespace BoDi.Internal
{
  public class DynamicRecursionResolverProxy : ProxyingResolver
  {
    public override ResolutionResult Resolve(ResolutionRequest request)
    {
      var result = ProxiedResolver.Resolve(request);

      if(request.ServiceType != typeof(IObjectContainer) || !result.IsSuccess || request.ResolutionPath.IsEmpty)
        return result;

      var container = (IObjectContainer) result.ResolvedObject;
      if(container is DynamicResolutionObjectContainerProxy)
        return result;

      var dynamicContainer = new DynamicResolutionObjectContainerProxy((ObjectContainer) container, request.ResolutionPath);
      var registration = GetFakeContainerRegistration(dynamicContainer, request.Name);

      return ResolutionResult.Success(request.ResolutionPath, dynamicContainer);
    }

    IServiceRegistration GetFakeContainerRegistration(DynamicResolutionObjectContainerProxy instance, string name)
    {
      return new FactoryRegistration(new Func<IObjectContainer>(() => instance)) {
        ServiceType = typeof(IObjectContainer),
        Name = name,
        Cacheable = false,
        DisposeWithContainer = false,
      };
    }

    public DynamicRecursionResolverProxy(IResolver proxiedResolver) : base(proxiedResolver) {}
  }
}
