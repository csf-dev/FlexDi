//
//    LateBoundResolverProxy.cs
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
using CSF.FlexDi.Registration;

namespace CSF.FlexDi.Resolution.Proxies
{
  /// <summary>
  /// A proxying resolver which may be late-bound to the resolver which it proxies.  This is required where a proxied
  /// resolver needs to be able to refer 'higher up' in the chain of responsibility of resolver proxies, but those
  /// higher-up resolvers cannot be created until the bottom of the chain is created.
  /// </summary>
  public class LateBoundResolverProxy : ResolverBase, IProxiesToAnotherResolver
  {
    IResolver proxiedResolver;

    /// <summary>
    /// Gets the wrapped/proxied resolver instance.
    /// </summary>
    /// <value>The proxied resolver.</value>
    public IResolver ProxiedResolver => proxiedResolver;

    /// <summary>
    /// Gets a registration which matches the given resolution request.
    /// </summary>
    /// <returns>The registration.</returns>
    /// <param name="request">Request.</param>
    public override IServiceRegistration GetRegistration(ResolutionRequest request)
      => ProxiedResolver.GetRegistration(request);

    /// <summary>
    /// Resolves the given resolution request and returns the result.
    /// </summary>
    /// <param name="request">Request.</param>
    public override ResolutionResult Resolve(ResolutionRequest request)
      => ProxiedResolver.Resolve(request);

    /// <summary>
    /// Sets the proxied resolver for the current proxy instance.  This is the late-binding method which allows a
    /// resolver to be inserted into the middle or bottom of a chain of responsibility, after many other resolvers have
    /// been added above it.
    /// </summary>
    /// <param name="proxiedResolver">Proxied resolver.</param>
    public void SetProxiedResolver(IResolver proxiedResolver)
    {
      if(proxiedResolver == null)
        throw new ArgumentNullException(nameof(proxiedResolver));
      if(this.proxiedResolver != null)
        throw new InvalidOperationException("The proxied resolver must be set only once.");

      this.proxiedResolver = proxiedResolver;
      proxiedResolver.ServiceResolved += OnServiceResolved;
    }

    void OnServiceResolved(object sender, ServiceResolutionEventArgs args)
    {
      InvokeServiceResolved(sender, args);
    }
  }
}
