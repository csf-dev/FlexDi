//
//    ResolverFactoryBase.cs
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
using CSF.FlexDi.Resolution.Proxies;

namespace CSF.FlexDi.Resolution
{
  /// <summary>
  /// Convenience base type for implementations of <see cref="ICreatesResolvers"/>.  This provides functionality
  /// for creating a chain of responsibility via the proxy pattern.
  /// </summary>
  public abstract class ResolverFactoryBase : ICreatesResolvers
  {
    /// <summary>
    /// Creates a resolver from the given resolution information.
    /// </summary>
    /// <returns>The resolver.</returns>
    /// <param name="resolutionInfo">Resolution info.</param>
    public virtual IResolver CreateResolver(IProvidesResolutionInfo resolutionInfo)
    {
      return CreateResolver(resolutionInfo, true);
    }

    /// <summary>
    /// Creates a resolver from the given resolution information.
    /// </summary>
    /// <returns>The resolver.</returns>
    /// <param name="resolutionInfo">Resolution info.</param>
    /// <param name="isInnermostResolver">If set to <c>true</c> then the resolver will be configured as the 'innermost' (most deeply nested) resolver.</param>
    protected virtual IResolver CreateResolver(IProvidesResolutionInfo resolutionInfo, bool isInnermostResolver)
    {
      AssertResolutionInfoIsValid(resolutionInfo);

      var lateBoundProxy = new LateBoundResolverProxy();

      var coreResolver = GetCoreResolver(resolutionInfo, lateBoundProxy);
      var proxiedResolver = WrapWithConfiguredProxies(coreResolver, isInnermostResolver, resolutionInfo);

      lateBoundProxy.SetProxiedResolver(proxiedResolver);

      return lateBoundProxy;
    }

    /// <summary>
    /// Wraps the given resolver using the 'stack' of proxies which are added to the factory list by
    /// <see cref="ConfigureResolverProxyFactories"/>.
    /// </summary>
    /// <returns>The outermost resolver instance.</returns>
    /// <param name="coreResolver">The core resolver to wrap in zero or more proxies.</param>
    /// <param name="isInnermostResolver">If set to <c>true</c> then the resolver will be configured as the innermost resolver.</param>
    /// <param name="resolutionInfo">Resolution info.</param>
    protected virtual IResolver WrapWithConfiguredProxies(Resolver coreResolver,
                                                          bool isInnermostResolver,
                                                          IProvidesResolutionInfo resolutionInfo)
    {
      var proxyFactories = new List<ICreatesProxyingResolver>();
      ConfigureResolverProxyFactories(proxyFactories, isInnermostResolver, coreResolver);

      IResolver currentResolver = coreResolver;

      foreach(var proxyFactory in proxyFactories)
      {
        var proxy = proxyFactory.Create(resolutionInfo, currentResolver);

        if(proxy != null)
          currentResolver = proxy;
      }

      return currentResolver;
    }

    /// <summary>
    /// Gets the core implementation of <see cref="Resolver"/> - the <see cref="IResolver"/> which will actually
    /// fulfil resolution requests.
    /// </summary>
    /// <returns>The core resolver.</returns>
    /// <param name="resolutionInfo">Resolution info.</param>
    /// <param name="outermostResolver">Outermost resolver.</param>
    protected virtual Resolver GetCoreResolver(IProvidesResolutionInfo resolutionInfo,
                                               IResolver outermostResolver)
    {
      var instanceCreator = new InstanceCreator(outermostResolver);
      return new Resolver(resolutionInfo.Registry, instanceCreator);
    }

    /// <summary>
    /// Asserts that resolution info is valid.
    /// </summary>
    /// <param name="resolutionInfo">Resolution info.</param>
    protected virtual void AssertResolutionInfoIsValid(IProvidesResolutionInfo resolutionInfo)
    {
      if(resolutionInfo == null)
        throw new ArgumentNullException(nameof(resolutionInfo));
      if(resolutionInfo.Registry == null)
        throw new ArgumentException("The registry provided by the resolution info must not be null.", nameof(resolutionInfo));
      if(resolutionInfo.Options == null)
        throw new ArgumentException("The options provided by the resolution info must not be null.", nameof(resolutionInfo));
    }

    /// <summary>
    /// Configures a collection of <see cref="ICreatesProxyingResolver"/>.  This method should add any number of
    /// such factories to the <paramref name="factories"/> collection.  These will be added, in order, to the 'stack'
    /// of proxies in which to wrap created resolvers.
    /// </summary>
    /// <param name="factories">The resolver proxy factories to use.</param>
    /// <param name="isInnermostResolver">If set to <c>true</c> is innermost resolver.</param>
    /// <param name="coreResolver">The core resolver.</param>
    protected abstract void ConfigureResolverProxyFactories(IList<ICreatesProxyingResolver> factories,
                                                            bool isInnermostResolver,
                                                            IResolvesRegistrations coreResolver);
  }
}
