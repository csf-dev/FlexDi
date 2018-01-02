//
//    Resolver.cs
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

namespace CSF.MicroDi.Resolution
{
  public class Resolver : ResolverBase, IResolvesRegistrations
  {
    readonly IServiceRegistrationProvider registrationProvider;
    readonly ICreatesObjectInstances instanceCreator;

    public override ResolutionResult Resolve(ResolutionRequest request)
    {
      var registration = GetRegistration(request);
      return Resolve(request, registration);
    }

    public override IServiceRegistration GetRegistration(ResolutionRequest request)
    {
      if(request == null)
        throw new ArgumentNullException(nameof(request));

      if(registrationProvider.CanFulfilRequest(request))
        return registrationProvider.Get(request);

      var requestWithoutName = request.GetCopyWithoutName();
      if(registrationProvider.CanFulfilRequest(requestWithoutName))
        return registrationProvider.Get(requestWithoutName);

      return null;
    }

    protected virtual ResolutionResult Resolve(ResolutionRequest request, IServiceRegistration registration)
    {
      if(request == null)
        throw new ArgumentNullException(nameof(request));
      AssertIsValidRequest(request);

      if(registration == null)
        return ResolutionResult.Failure(request.ResolutionPath);

      var factory = registration.GetFactoryAdapter(request);
      if(factory == null)
        return ResolutionResult.Failure(request.ResolutionPath);

      var resolved = instanceCreator.CreateFromFactory(factory, request.ResolutionPath, registration);
      InvokeServiceResolved(registration, resolved);
      return ResolutionResult.Success(request.ResolutionPath, resolved);
    }

    ResolutionResult IResolvesRegistrations.Resolve(ResolutionRequest request, IServiceRegistration registration)
      => Resolve(request, registration);

    protected virtual void AssertIsValidRequest(ResolutionRequest request)
    {
      var serviceType = request.ServiceType;
      if(serviceType.IsPrimitive || serviceType.IsValueType || serviceType == typeof(string))
      {
        var message = $"Primitive types or structs cannot be resolved.{Environment.NewLine}{request.ToString()}";
        throw new InvalidResolutionRequestException(message) {
          ResolutionPath = request.ResolutionPath
        };
      }
    }

    public Resolver(IServiceRegistrationProvider registrationProvider,
                    ICreatesObjectInstances instanceCreator)
    {
      if(instanceCreator == null)
        throw new ArgumentNullException(nameof(instanceCreator));
      if(registrationProvider == null)
        throw new ArgumentNullException(nameof(registrationProvider));

      this.registrationProvider = registrationProvider;
      this.instanceCreator = instanceCreator;
    }
  }
}
