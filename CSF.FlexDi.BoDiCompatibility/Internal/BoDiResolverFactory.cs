//
//    BoDiResolverFactory.cs
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
using CSF.FlexDi;
using CSF.FlexDi.Resolution;
using CSF.FlexDi.Resolution.Proxies;

namespace BoDi.Internal
{
  /// <summary>
  /// A specialisation of <see cref="ResolverFactory"/> which gets an <see cref="IResolver"/> which is compatible with
  /// BoDi's functionality.
  /// </summary>
  public class BoDiResolverFactory : ResolverFactory
  {
    /// <summary>
    /// Configures a collection of <see cref="T:CSF.FlexDi.Resolution.Proxies.ICreatesProxyingResolver" />.  This method should add any number of
    /// such factories to the <paramref name="factories" /> collection.  These will be added, in order, to the 'stack'
    /// of proxies in which to wrap created resolvers.
    /// </summary>
    /// <param name="factories">The resolver proxy factories to use.</param>
    /// <param name="isInnermostResolver">If set to <c>true</c> is innermost resolver.</param>
    /// <param name="coreResolver">The core resolver.</param>
    protected override void ConfigureResolverProxyFactories(IList<ICreatesProxyingResolver> factories,
                                                            bool isInnermostResolver,
                                                            IResolvesRegistrations coreResolver)
    {
      base.ConfigureResolverProxyFactories(factories, isInnermostResolver, coreResolver);

      factories.Add(new DynamicRecursionResolverProxyFactory());
    }
  }
}
