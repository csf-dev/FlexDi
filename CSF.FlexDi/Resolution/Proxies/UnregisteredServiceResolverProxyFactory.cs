//
//    UnregisteredServiceResolverProxyFactory.cs
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
  /// Implementation of <see cref="ICreatesProxyingResolver"/> which creates instances of
  /// <see cref="UnregisteredServiceResolverProxy"/>.
  /// </summary>
  public class UnregisteredServiceResolverProxyFactory : ICreatesProxyingResolver
  {
    readonly IServiceRegistrationProvider unregisteredRegistrationProvider;
    readonly IResolvesRegistrations registrationResolver;

    /// <summary>
    /// Creates a resolver which wraps/proxies an inner resolver.
    /// </summary>
    /// <param name="resolutionInfo">Resolution info.</param>
    /// <param name="resolverToProxy">The resolver to proxy.</param>
    public IResolver Create(IProvidesResolutionInfo resolutionInfo, IResolver resolverToProxy)
    {
      if(!resolutionInfo.Options.ResolveUnregisteredTypes)
        return null;

      var provider = GetUnregisteredServiceRegistrationProvider(resolutionInfo);
      var cache = resolutionInfo.Options.UseInstanceCache? resolutionInfo.Cache : null;

      return new UnregisteredServiceResolverProxy(resolverToProxy,
                                                  registrationResolver,
                                                  provider,
                                                  cache);
    }

    IServiceRegistrationProvider GetUnregisteredServiceRegistrationProvider(IProvidesResolutionInfo resolutionInfo)
    {
      if(unregisteredRegistrationProvider != null) return unregisteredRegistrationProvider;

      return new ServiceWithoutRegistrationProvider(resolutionInfo.ConstructorSelector);
    }

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="T:CSF.FlexDi.Resolution.Proxies.UnregisteredServiceResolverProxyFactory"/> class.
    /// </summary>
    /// <param name="registrationResolver">Registration resolver.</param>
    /// <param name="unregisteredRegistrationProvider">Unregistered registration provider.</param>
    public UnregisteredServiceResolverProxyFactory(IResolvesRegistrations registrationResolver,
                                                   IServiceRegistrationProvider unregisteredRegistrationProvider = null)
    {
      if(registrationResolver == null)
        throw new ArgumentNullException(nameof(registrationResolver));

      this.registrationResolver = registrationResolver;
      this.unregisteredRegistrationProvider = unregisteredRegistrationProvider;
    }
  }
}
