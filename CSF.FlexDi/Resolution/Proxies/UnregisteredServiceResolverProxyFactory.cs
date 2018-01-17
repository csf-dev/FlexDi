﻿//
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
  public class UnregisteredServiceResolverProxyFactory : ICreatesProxyingResolver
  {
    readonly IServiceRegistrationProvider unregisteredRegistrationProvider;
    readonly IResolvesRegistrations registrationResolver;

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
