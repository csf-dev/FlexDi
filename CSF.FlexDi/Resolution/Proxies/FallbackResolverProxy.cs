//
//    FallbackResolverProxy.cs
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
namespace CSF.FlexDi.Resolution.Proxies
{
  /// <summary>
  /// A proxying resolver which is able to 'fall back' to use a different resolver, if the primary proxied
  /// resolver is unable to fulfil the resolution request.
  /// </summary>
  public class FallbackResolverProxy : ProxyingResolver
  {
    readonly IResolver fallbackResolver;

    /// <summary>
    /// Gets the fallback resolver.
    /// </summary>
    /// <value>The fallback resolver.</value>
    public IResolver FallbackResolver => fallbackResolver;

    /// <summary>
    /// Resolves the given resolution request and returns the result.
    /// </summary>
    /// <param name="request">Request.</param>
    public override ResolutionResult Resolve(ResolutionRequest request)
    {
      var output = ProxiedResolver.Resolve(request);
      if(output.IsSuccess)
        return output;

      return fallbackResolver.Resolve(request);
    }

    /// <summary>
    /// Gets the registration which corresponds to a given resolution request.
    /// </summary>
    /// <returns>The registration.</returns>
    /// <param name="request">Request.</param>
    public override Registration.IServiceRegistration GetRegistration(ResolutionRequest request)
    {
      return ProxiedResolver.GetRegistration(request) ?? fallbackResolver.GetRegistration(request);
    }

    void OnServiceResolved(object sender, ServiceResolutionEventArgs args)
    {
      InvokeServiceResolved(sender, args);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.FlexDi.Resolution.Proxies.FallbackResolverProxy"/> class.
    /// </summary>
    /// <param name="proxiedResolver">Proxied resolver.</param>
    /// <param name="fallbackResolver">Fallback resolver.</param>
    public FallbackResolverProxy(IResolver proxiedResolver, IResolver fallbackResolver) : base(proxiedResolver)
    {
      if(fallbackResolver == null)
        throw new ArgumentNullException(nameof(fallbackResolver));
      
      this.fallbackResolver = fallbackResolver;
      fallbackResolver.ServiceResolved += OnServiceResolved;
    }
  }
}
