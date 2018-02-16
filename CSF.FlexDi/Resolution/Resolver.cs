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
using CSF.FlexDi.Registration;

namespace CSF.FlexDi.Resolution
{
  /// <summary>
  /// This is the default implementation of <see cref="IResolver"/> and <see cref="IResolvesRegistrations"/>.
  /// It performs actual resolution of services/components, looking up an appropriate registration and then
  /// getting the object instance using that registration's information.
  /// </summary>
  public class Resolver : ResolverBase, IResolvesRegistrations
  {
    readonly IServiceRegistrationProvider registrationProvider;
    readonly ICreatesObjectInstances instanceCreator;

    /// <summary>
    /// Resolves the given resolution request and returns the result.
    /// </summary>
    /// <param name="request">Request.</param>
    public override ResolutionResult Resolve(ResolutionRequest request)
    {
      var registration = GetRegistration(request);
      return Resolve(request, registration);
    }

    /// <summary>
    /// Gets a registration which matches the given resolution request.
    /// </summary>
    /// <returns>The registration.</returns>
    /// <param name="request">Request.</param>
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

    /// <summary>
    /// Resolves the given resolution request, using the given service registration.
    /// </summary>
    /// <param name="request">Request.</param>
    /// <param name="registration">Registration.</param>
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

    /// <summary>
    /// Asserts that a resolution request is valid.
    /// </summary>
    /// <param name="request">Request.</param>
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

    /// <summary>
    /// Initializes a new instance of the <see cref="T:CSF.FlexDi.Resolution.Resolver"/> class.
    /// </summary>
    /// <param name="registrationProvider">Registration provider.</param>
    /// <param name="instanceCreator">Instance creator.</param>
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
