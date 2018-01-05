//
//    ServiceResolvingContainerProxy.cs
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
using System.Collections.Generic;
using System.Linq;

namespace CSF.MicroDi.Resolution.Proxies
{
  public class ServiceResolvingContainerProxy : IResolvesServices
  {
    readonly IContainer proxiedResolver;
    readonly ResolutionPath resolutionPath;

    public object Resolve(ResolutionRequest request)
    {
      return proxiedResolver.Resolve(new ResolutionRequest(request.ServiceType, request.Name, resolutionPath));
    }

    public object Resolve(Type serviceType)
    {
      return Resolve(new ResolutionRequest(serviceType));
    }

    public object Resolve(Type serviceType, string name)
    {
      return Resolve(new ResolutionRequest(serviceType, name));
    }

    public T Resolve<T>()
    {
      return (T) Resolve(new ResolutionRequest(typeof(T)));
    }

    public T Resolve<T>(string name)
    {
      return (T) Resolve(new ResolutionRequest(typeof(T), name));
    }

    public IReadOnlyCollection<object> ResolveAll(Type serviceType)
    {
      return proxiedResolver
        .GetRegistrations(serviceType)
        .Select(x => Resolve(x.ServiceType, x.Name))
        .ToArray();
    }

    public IReadOnlyCollection<T> ResolveAll<T>()
    {
      return ResolveAll(typeof(T))
        .Cast<T>()
        .ToArray();
    }

    public ResolutionResult TryResolve(ResolutionRequest request)
    {
      return proxiedResolver.TryResolve(new ResolutionRequest(request.ServiceType, request.Name, resolutionPath));
    }

    public bool TryResolve(Type serviceType, out object output)
    {
      return TryResolve(serviceType, null, out output);
    }

    public bool TryResolve(Type serviceType, string name, out object output)
    {
      var result = TryResolve(new ResolutionRequest(serviceType, name));
                              
      if(result.IsSuccess)
      {
        output = result.ResolvedObject;
        return true;
      }
        
      output = null;
      return false;
    }

    public bool TryResolve<T>(out T output)
    {
      return TryResolve(null, out output);
    }

    public bool TryResolve<T>(string name, out T output)
    {
      var result = TryResolve(new ResolutionRequest(typeof(T), name));

      if(result.IsSuccess)
      {
        output = (T) result.ResolvedObject;
        return true;
      }

      output = default(T);
      return false;
    }

    public ServiceResolvingContainerProxy(IContainer proxiedResolver, ResolutionPath resolutionPath)
    {
      if(resolutionPath == null)
        throw new ArgumentNullException(nameof(resolutionPath));
      if(proxiedResolver == null)
        throw new ArgumentNullException(nameof(proxiedResolver));

      this.resolutionPath = resolutionPath;
      this.proxiedResolver = proxiedResolver;
    }
  }
}
