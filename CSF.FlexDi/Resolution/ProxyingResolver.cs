//
//    ProxyingResolver.cs
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

namespace CSF.FlexDi.Resolution
{
  /// <summary>
  /// Convenience base type for an <see cref="IResolver"/> which also implements the
  /// <see cref="IProxiesToAnotherResolver"/> interface.
  /// </summary>
  public abstract class ProxyingResolver : ResolverBase, IProxiesToAnotherResolver
  {
    readonly IResolver proxiedResolver;

    /// <summary>
    /// Gets the wrapped/proxied resolver instance.
    /// </summary>
    /// <value>The proxied resolver.</value>
    public IResolver ProxiedResolver => proxiedResolver;

    /// <summary>
    /// Gets the registration which corresponds to a given resolution request.
    /// </summary>
    /// <returns>The registration.</returns>
    /// <param name="request">Request.</param>
    public override IServiceRegistration GetRegistration(ResolutionRequest request)
      => ProxiedResolver.GetRegistration(request);

    void OnServiceResolved(object sender, ServiceResolutionEventArgs args)
    {
      InvokeServiceResolved(sender, args);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:CSF.FlexDi.Resolution.ProxyingResolver"/> class.
    /// </summary>
    /// <param name="proxiedResolver">Proxied resolver.</param>
    public ProxyingResolver(IResolver proxiedResolver)
    {
      if(proxiedResolver == null)
        throw new ArgumentNullException(nameof(proxiedResolver));

      this.proxiedResolver = proxiedResolver;
      proxiedResolver.ServiceResolved += OnServiceResolved;
    }
  }
}
