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
using System.Reflection;

namespace CSF.FlexDi.Resolution.Proxies
{
  /// <summary>
  /// A special proxy type for an instance of <see cref="IResolvesServices"/>.  This proxy behaves like a normal
  /// resolver, except that it contains a partial resolution path, and thus may detect circular dependencies across
  /// dynamic resolutions.
  /// </summary>
  /// <seealso cref="DynamicRecursionResolverProxy"/>
  public class ServiceResolvingContainerProxy : IResolvesServices
  {
    readonly IContainer proxiedResolver;
    readonly ResolutionPath resolutionPath;

    /// <summary>
    /// Gets the proxied resolver.
    /// </summary>
    /// <value>The proxied resolver.</value>
    public IContainer ProxiedResolver => proxiedResolver;

    /// <summary>
    /// Resolves a component, as specified by a <see cref="CSF.FlexDi.Resolution.ResolutionRequest" /> instance.
    /// </summary>
    /// <param name="request">The resolved component instance.</param>
    public object Resolve(ResolutionRequest request)
    {
      return proxiedResolver.Resolve(new ResolutionRequest(request.ServiceType, request.Name, resolutionPath));
    }

    /// <summary>
    /// Resolves an instance of the specified type.
    /// </summary>
    /// <param name="serviceType">The component type to be resolved.</param>
    public object Resolve(Type serviceType)
    {
      return Resolve(new ResolutionRequest(serviceType));
    }

    /// <summary>
    /// Resolves an instance of the specified type, using the given named registration.
    /// </summary>
    /// <param name="serviceType">The component type to be resolved.</param>
    /// <param name="name">The registration name.</param>
    public object Resolve(Type serviceType, string name)
    {
      return Resolve(new ResolutionRequest(serviceType, name));
    }

    /// <summary>
    /// Resolves an instance of the specified type.
    /// </summary>
    /// <typeparam name="T">The component type to be resolved.</typeparam>
    public T Resolve<T>()
    {
      return (T) Resolve(new ResolutionRequest(typeof(T)));
    }

    /// <summary>
    /// Resolves an instance of the specified type, using the given named registration.
    /// </summary>
    /// <param name="name">The registration name.</param>
    /// <typeparam name="T">The component type to be resolved.</typeparam>
    public T Resolve<T>(string name)
    {
      return (T) Resolve(new ResolutionRequest(typeof(T), name));
    }

    /// <summary>
    /// Creates a collection which contains resolved instances of all of the components registered for a given type.
    /// </summary>
    /// <returns>A collection of resolved components.</returns>
    /// <param name="serviceType">The type of the components to be resolved.</param>
    public IReadOnlyCollection<object> ResolveAll(Type serviceType)
    {
      return proxiedResolver
        .GetRegistrations(serviceType)
        .Select(x => Resolve(x.ServiceType, x.Name))
        .ToArray();
    }

    /// <summary>
    /// Creates a collection which contains resolved instances of all of the components registered for a given type.
    /// </summary>
    /// <returns>A collection of resolved components.</returns>
    /// <typeparam name="T">The type of the components to be resolved.</typeparam>
    public IReadOnlyCollection<T> ResolveAll<T>()
    {
      return ResolveAll(typeof(T))
        .Cast<T>()
        .ToArray();
    }

    /// <summary>
    /// Attempts to resolve a component, as specified by a <see cref="CSF.FlexDi.Resolution.ResolutionRequest" /> instance.
    /// The result indicates whether resolution was successful or not, and if it is, contains a reference to the resolved
    /// component.
    /// </summary>
    /// <returns>A resolution result instance.</returns>
    /// <param name="request">A resolution request specifying what is to be resolved.</param>
    public ResolutionResult TryResolve(ResolutionRequest request)
    {
      return proxiedResolver.TryResolve(new ResolutionRequest(request.ServiceType, request.Name, resolutionPath));
    }

    /// <summary>
    /// Attempts to resolve an instance of the specified type, but does not raise an exception if resolution fails.
    /// </summary>
    /// <returns>
    /// <c>true</c>, if resolution was successful, <c>false</c> otherwise.</returns>
    /// <param name="output">The resolved component instance.</param>
    /// <param name="serviceType">The component type to be resolved.</param>
    public bool TryResolve(Type serviceType, out object output)
    {
      return TryResolve(serviceType, null, out output);
    }

    /// <summary>
    /// Attempts to resolve an instance of the specified type and using the given named registration, but
    /// does not raise an exception if resolution fails.
    /// </summary>
    /// <returns>
    /// <c>true</c>, if resolution was successful, <c>false</c> otherwise.</returns>
    /// <param name="output">The resolved component instance.</param>
    /// <param name="name">The registration name.</param>
    /// <param name="serviceType">The component type to be resolved.</param>
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

    /// <summary>
    /// Attempts to resolve an instance of the specified type, but does not raise an exception if resolution fails.
    /// </summary>
    /// <returns>
    /// <c>true</c>, if resolution was successful, <c>false</c> otherwise.</returns>
    /// <param name="output">The resolved component instance.</param>
    /// <typeparam name="T">The component type to be resolved.</typeparam>
    public bool TryResolve<T>(out T output)
    {
      return TryResolve(null, out output);
    }

    /// <summary>
    /// Attempts to resolve an instance of the specified type and using the given named registration, but
    /// does not raise an exception if resolution fails.
    /// </summary>
    /// <returns>
    /// <c>true</c>, if resolution was successful, <c>false</c> otherwise.</returns>
    /// <param name="output">The resolved component instance.</param>
    /// <param name="name">The registration name.</param>
    /// <typeparam name="T">The component type to be resolved.</typeparam>
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

    /// <summary>
    /// Attempts to resolve an instance of the specified type, but returns a <c>null</c> reference if resolution fails.
    /// </summary>
    /// <returns>The resolved component instance, or a <c>null</c> reference if resolution fauls.</returns>
    /// <typeparam name="T">The component type to be resolved.</typeparam>
    public T TryResolve<T>() where T : class => TryResolve<T>(null);

    /// <summary>
    /// Attempts to resolve an instance of the specified type and using the given named registration, but
    /// returns a <c>null</c> reference if resolution fails.
    /// </summary>
    /// <returns>The resolved component instance, or a <c>null</c> reference if resolution fauls.</returns>
    /// <param name="name">The registration name.</param>
    /// <typeparam name="T">The component type to be resolved.</typeparam>
    public T TryResolve<T>(string name) where T : class
    {
      T output;
      if(!TryResolve<T>(name, out output))
        return null;

      return output;
    }

    /// <summary>
    /// Attempts to resolve an instance of the specified type, but returns a <c>null</c> reference if resolution fails.
    /// </summary>
    /// <returns>The resolved component instance, or a <c>null</c> reference if resolution fauls.</returns>
    /// <param name="serviceType">The component type to be resolved.</param>
    public object TryResolve(Type serviceType) => TryResolve(serviceType, null);

    /// <summary>
    /// Attempts to resolve an instance of the specified type and using the given named registration, but
    /// returns a <c>null</c> reference if resolution fails.
    /// </summary>
    /// <returns>The resolved component instance, or a <c>null</c> reference if resolution fauls.</returns>
    /// <param name="name">The registration name.</param>
    /// <param name="serviceType">The component type to be resolved.</param>
    public object TryResolve(Type serviceType, string name)
    {
      if(serviceType == null)
        throw new ArgumentNullException(nameof(serviceType));
      if(serviceType.GetTypeInfo().IsValueType)
        throw new ArgumentException(Resources.ExceptionFormats.TypeToResolveMustBeNullableReferenceType, nameof(serviceType));

      object output;
      if(!TryResolve(serviceType, name, out output))
        return null;

      return output;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.FlexDi.Resolution.Proxies.ServiceResolvingContainerProxy"/> class.
    /// </summary>
    /// <param name="proxiedResolver">Proxied resolver.</param>
    /// <param name="resolutionPath">Resolution path.</param>
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
