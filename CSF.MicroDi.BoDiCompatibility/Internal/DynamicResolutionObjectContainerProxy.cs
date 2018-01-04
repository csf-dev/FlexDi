//
//    DynamicResolutionObjectContainerProxy.cs
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
using System.Collections.Generic;
using System.Linq;
using CSF.MicroDi.Resolution;

namespace BoDi.Internal
{
  public class DynamicResolutionObjectContainerProxy : IObjectContainer
  {
    readonly ResolutionPath resolutionPath;
    readonly ObjectContainer proxiedContainer;
    readonly ExceptionTransformer exceptionTransformer;

    #region IObjectContainer implementation

    public event Action<object> ObjectCreated;

    void InvokeObjectCreated(object obj)
    {
      ObjectCreated?.Invoke(obj);
    }

    public void RegisterTypeAs<TType, TInterface>(string name = null) where TType : class, TInterface
    {
      proxiedContainer.RegisterTypeAs<TType, TInterface>(name);
    }

    public void RegisterInstanceAs<TInterface>(TInterface instance, string name = null, bool dispose = false) where TInterface : class
    {
      proxiedContainer.RegisterInstanceAs<TInterface>(instance, name, dispose);
    }

    public void RegisterInstanceAs(object instance, Type interfaceType, string name = null, bool dispose = false)
    {
      proxiedContainer.RegisterInstanceAs(instance, interfaceType, name, dispose);
    }

    public void RegisterFactoryAs<TInterface>(Func<IObjectContainer, TInterface> factoryDelegate, string name = null)
    {
      proxiedContainer.RegisterFactoryAs<TInterface>(factoryDelegate, name);
    }

    public T Resolve<T>()
    {
      return exceptionTransformer.TransformExceptions(() => {
        return (T) proxiedContainer
          .GetMicroDiContainer()
          .Resolve(new ResolutionRequest(typeof(T), resolutionPath));
      });
    }

    public T Resolve<T>(string name)
    {
      return exceptionTransformer.TransformExceptions(() => {
        return (T) proxiedContainer
          .GetMicroDiContainer()
          .Resolve(new ResolutionRequest(typeof(T), name, resolutionPath));
      });
    }

    public object Resolve(Type typeToResolve, string name = null)
    {
      return exceptionTransformer.TransformExceptions(() => {
        return proxiedContainer
          .GetMicroDiContainer()
          .Resolve(new ResolutionRequest(typeToResolve, name, resolutionPath));
      });
    }

    public IEnumerable<T> ResolveAll<T>() where T : class
    {
      return exceptionTransformer.TransformExceptions(() => {
        return proxiedContainer
          .GetMicroDiContainer()
          .GetRegistrations(typeof(T))
          .Select(x => Resolve(x.ServiceType, x.Name))
          .Cast<T>()
          .ToArray();
      });
    }

    public bool IsRegistered<T>()
    {
      return proxiedContainer.IsRegistered<T>();
    }

    public bool IsRegistered<T>(string name)
    {
      return proxiedContainer.IsRegistered<T>(name);
    }

    public void Dispose()
    {
      proxiedContainer.Dispose();
    }

    #endregion

    public DynamicResolutionObjectContainerProxy(ObjectContainer proxiedContainer, ResolutionPath resolutionPath)
    {
      if(proxiedContainer == null)
        throw new ArgumentNullException(nameof(proxiedContainer));

      this.proxiedContainer = proxiedContainer;
      this.resolutionPath = resolutionPath ?? new ResolutionPath();
      exceptionTransformer = new ExceptionTransformer();

      proxiedContainer.ObjectCreated += InvokeObjectCreated;
    }
  }
}
