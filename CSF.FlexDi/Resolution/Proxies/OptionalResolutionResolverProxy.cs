//
//    OptionalResolutionResolverProxy.cs
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
using System.Reflection;

namespace CSF.FlexDi.Resolution.Proxies
{
  /// <summary>
  /// A proxying resolver which makes every single resolution operation optional.  It achieves this by ensuring that
  /// the outcome of every resolution operation has <see cref="ResolutionResult.IsSuccess"/> set to <c>true</c>.
  /// </summary>
  public class OptionalResolutionResolverProxy : ProxyingResolver
  {
    /// <summary>
    /// Resolves the given resolution request and returns the result.
    /// </summary>
    /// <param name="request">Request.</param>
    public override ResolutionResult Resolve(ResolutionRequest request)
    {
      var result = ProxiedResolver.Resolve(request);

      if(result.IsSuccess)
        return result;

      var defaultValue = GetDefaultForType(request.ServiceType);
      return ResolutionResult.Success(request.ResolutionPath, defaultValue);
    }

    object GetDefaultForType(Type serviceType)
      => serviceType.GetTypeInfo().IsValueType ? Activator.CreateInstance(serviceType) : null;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:CSF.FlexDi.Resolution.Proxies.OptionalResolutionResolverProxy"/> class.
    /// </summary>
    /// <param name="proxiedResolver">Proxied resolver.</param>
    public OptionalResolutionResolverProxy(IResolver proxiedResolver) : base(proxiedResolver) {}
  }
}
