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
using CSF.MicroDi.Registration;

namespace CSF.MicroDi.Resolution.Proxies
{
  public class UnregisteredServiceResolverProxy : ProxyingResolver
  {
    readonly IServiceRegistrationProvider unregisteredRegistrationProvider;
    readonly IResolvesRegistrations registrationResolver;
    readonly ICachesResolvedServiceInstances cache;

    public override ResolutionResult Resolve(ResolutionRequest request)
    {
      var output = ProxiedResolver.Resolve(request);
      if(output.IsSuccess)
        return output;

      if(cache != null)
      {
        object instance;
        if(cache.TryGet(request.ServiceType, request.Name, out instance))
          return ResolutionResult.Success(request.ResolutionPath, instance);
      }

      var registration = unregisteredRegistrationProvider.Get(request);
      output = registrationResolver.Resolve(request, registration);
      if(output.IsSuccess && cache != null)
        cache.Add(registration, output.ResolvedObject);

      return output;
    }

    public override IServiceRegistration GetRegistration(ResolutionRequest request)
    {
      var registration = base.GetRegistration(request);
      if(registration != null)
        return registration;

      return unregisteredRegistrationProvider.Get(request);
    }

    public UnregisteredServiceResolverProxy(IResolver proxiedResolver,
                                            IResolvesRegistrations registrationResolver,
                                            IServiceRegistrationProvider unregisteredRegistrationProvider,
                                            ICachesResolvedServiceInstances cache)
      : base(proxiedResolver)
    {
      if(unregisteredRegistrationProvider == null)
        throw new ArgumentNullException(nameof(unregisteredRegistrationProvider));
      if(registrationResolver == null)
        throw new ArgumentNullException(nameof(registrationResolver));

      this.cache = cache;
      this.registrationResolver = registrationResolver;
      this.unregisteredRegistrationProvider = unregisteredRegistrationProvider;
    }
  }
}
