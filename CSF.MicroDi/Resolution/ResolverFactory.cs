//
//    ResolverFactory.cs
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
using CSF.MicroDi.Resolution.Proxies;

namespace CSF.MicroDi.Resolution
{
  public class ResolverFactory : ICreatesResolvers
  {
    public virtual IResolver CreateResolver(IProvidesResolutionInfo resolutionInfo)
    {
      return CreateResolver(resolutionInfo, true);
    }

    protected virtual IResolver CreateResolver(IProvidesResolutionInfo resolutionInfo, bool isInnermostResolver)
    {
      AssertResolutionInfoIsValid(resolutionInfo);

      var lateBoundProxy = new LateBoundResolverProxy();

      var coreResolver = GetCoreResolver(resolutionInfo, lateBoundProxy);
      var proxiedResolver = WrapWithConfiguredProxies(coreResolver, isInnermostResolver, resolutionInfo);

      lateBoundProxy.SetProxiedResolver(proxiedResolver);

      return lateBoundProxy;
    }

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

    protected virtual void ConfigureResolverProxyFactories(IList<ICreatesProxyingResolver> factories,
                                                           bool isInnermostResolver,
                                                           IResolvesRegistrations coreResolver)
    {
      factories.Add(new CachingResolverProxyFactory());
      factories.Add(new FallbackToParentResolverProxyFactory(r => CreateResolver(r, false)));

      // Only the innermost resolver (the most deeply nested) can resolve unregistered services
      if(isInnermostResolver)
        factories.Add(new UnregisteredServiceResolverProxyFactory(coreResolver));

      factories.Add(new CircularDependencyPreventingResolverProxyFactory());
      factories.Add(new RegisteredNameInjectingResolverProxyFactory());
      factories.Add(new NamedInstanceDictionaryResolverProxyFactory());
    }

    protected virtual Resolver GetCoreResolver(IProvidesResolutionInfo resolutionInfo,
                                               IResolver outermostResolver)
    {
      var instanceCreator = new InstanceCreator(outermostResolver);
      return new Resolver(resolutionInfo.Registry, instanceCreator);
    }

    protected virtual void AssertResolutionInfoIsValid(IProvidesResolutionInfo resolutionInfo)
    {
      if(resolutionInfo == null)
        throw new ArgumentNullException(nameof(resolutionInfo));
      if(resolutionInfo.Registry == null)
        throw new ArgumentException("The registry provided by the resolution info must not be null.", nameof(resolutionInfo));
      if(resolutionInfo.Options == null)
        throw new ArgumentException("The options provided by the resolution info must not be null.", nameof(resolutionInfo));
    }
  }
}
