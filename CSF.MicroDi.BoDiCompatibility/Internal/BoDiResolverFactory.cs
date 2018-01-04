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
using CSF.MicroDi;
using CSF.MicroDi.Resolution;

namespace BoDi.Internal
{
  public class BoDiResolverFactory : ResolverFactory
  {
    protected override IResolver CreateResolver(IProvidesResolutionInfo resolutionInfo, bool isInnermostResolver)
    {
      AssertResolutionInfoIsValid(resolutionInfo);

      var output = new LateBoundResolverProxy();

      var coreResolver = GetCoreResolver(resolutionInfo, output);
      IResolver currentResolver = coreResolver;

      currentResolver = GetCachingResolver(resolutionInfo, currentResolver) ?? currentResolver;
      currentResolver = GetParentResolver(resolutionInfo, currentResolver) ?? currentResolver;

      // Only the innermost resolver (the most deeply nested) can resolve unregistered services
      if(isInnermostResolver)
        currentResolver = GetUnregisteredServiceResolver(resolutionInfo, currentResolver, coreResolver) ?? currentResolver;

      currentResolver = GetCircularDependencyProtectingResolver(resolutionInfo, currentResolver) ?? currentResolver;
      currentResolver = GetRegisteredNameInjectingResolver(currentResolver) ?? currentResolver;
      currentResolver = GetNamedInstanceDictionaryResolver(resolutionInfo, currentResolver) ?? currentResolver;
      currentResolver = GetDynamicRecursiveResolver(currentResolver) ?? currentResolver;

      output.ProvideProxiedResolver(currentResolver);

      return output;
    }

    protected virtual IResolver GetDynamicRecursiveResolver(IResolver resolverToProxy)
    {
      return new DynamicRecursionResolverProxy(resolverToProxy);
    }
  }
}
