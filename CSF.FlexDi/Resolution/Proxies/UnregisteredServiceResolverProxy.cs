//
//    UnregisteredServiceResolverProxy.cs
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
  /// A proxying resolver which resolves components which have no registration.  It does this by creating a registration
  /// from a special provider instance.
  /// </summary>
  public class UnregisteredServiceResolverProxy : ProxyingResolver
  {
    readonly IServiceRegistrationProvider unregisteredRegistrationProvider;
    readonly IResolvesRegistrations registrationResolver;
    readonly ICachesResolvedServiceInstances cache;
    readonly IRegistersServices registry;

    /// <summary>
    /// Resolves the given resolution request and returns the result.
    /// </summary>
    /// <param name="request">Request.</param>
    public override ResolutionResult Resolve(ResolutionRequest request)
    {
      var output = ProxiedResolver.Resolve(request);
      if(output.IsSuccess)
        return output;

      var registration = GetUnregisteredServiceRegistration(request);
      output = registrationResolver.Resolve(request, registration);
      MakeRegistrationAndResultAvailableToSubsequentResolutions(registration, output);

      return output;
    }

    /// <summary>
    /// Gets the registration which corresponds to a given resolution request.
    /// </summary>
    /// <returns>The registration.</returns>
    /// <param name="request">Request.</param>
    public override IServiceRegistration GetRegistration(ResolutionRequest request)
    {
      var registration = base.GetRegistration(request);
      if(registration != null)
        return registration;

      return GetUnregisteredServiceRegistration(request);
    }

    IServiceRegistration GetUnregisteredServiceRegistration(ResolutionRequest request)
      => unregisteredRegistrationProvider.Get(request);

    void MakeRegistrationAndResultAvailableToSubsequentResolutions(IServiceRegistration registration,
                                                                   ResolutionResult result)
    {
      if(result == null || !result.IsSuccess) return;

      registry.Add(registration);

      if(cache != null)
        cache.Add(registration, result.ResolvedObject);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:CSF.FlexDi.Resolution.Proxies.UnregisteredServiceResolverProxy"/> class.
    /// </summary>
    /// <param name="proxiedResolver">Proxied resolver.</param>
    /// <param name="registrationResolver">Registration resolver.</param>
    /// <param name="unregisteredRegistrationProvider">Unregistered registration provider.</param>
    /// <param name="cache">The service cache.</param>
    /// <param name="registry">The service registry</param>
    public UnregisteredServiceResolverProxy(IResolver proxiedResolver,
                                            IResolvesRegistrations registrationResolver,
                                            IServiceRegistrationProvider unregisteredRegistrationProvider,
                                            ICachesResolvedServiceInstances cache,
                                            IRegistersServices registry)
      : base(proxiedResolver)
    {
      if(registry == null)
        throw new ArgumentNullException(nameof(registry));
      if(unregisteredRegistrationProvider == null)
        throw new ArgumentNullException(nameof(unregisteredRegistrationProvider));
      if(registrationResolver == null)
        throw new ArgumentNullException(nameof(registrationResolver));

      this.cache = cache;
      this.registrationResolver = registrationResolver;
      this.unregisteredRegistrationProvider = unregisteredRegistrationProvider;
      this.registry = registry;
    }
  }
}
